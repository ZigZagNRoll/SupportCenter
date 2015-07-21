using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Configuration;
using SC.BL.Domain;

namespace SC.DAL.OleDb
{
    public class TicketRepository : ITicketRepository
    {
        private OleDbConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SupportCenterDB_OleDb"].ConnectionString;
            return new OleDbConnection(connectionString);
        }

        public BL.Domain.Ticket CreateTicket(BL.Domain.Ticket ticket)
        {
            string insertStatement = "INSERT INTO Ticket(AccountId, [Text],DateOpened, State, DeviceName) VALUES(@accountId, @text, @dateOpened, @state,@deviceName)";
            using (var connection = this.GetConnection())
            {
                OleDbCommand command = new OleDbCommand(insertStatement, connection);
                command.Parameters.AddWithValue("@accountId", ticket.AccountId);
                command.Parameters.AddWithValue("@text", ticket.Text);
                command.Parameters.AddWithValue("@dateOpened",
                ticket.DateOpened.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@state", (byte)ticket.State);
                if (ticket is HardwareTicket)
                    command.Parameters.AddWithValue("@deviceName",
                    ((HardwareTicket)ticket).DeviceName);
                else
                    command.Parameters.AddWithValue("@deviceName", DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
                string retrievalQuery = "Select @@Identity";
                OleDbCommand retrievalOfNewNbrCommand = new OleDbCommand(retrievalQuery, connection);
                ticket.TicketNumber = Convert.ToInt32(retrievalOfNewNbrCommand.ExecuteScalar());

                connection.Close();
            } return ticket;
        }

        public IEnumerable<BL.Domain.Ticket> ReadTickets()
        {
            List<Ticket> tickets = new List<Ticket> { };

            string selectStatement = "SELECT TicketNumber, AccountId, [Text], DateOpened, State, DeviceName FROM Ticket";
            using (var connection = this.GetConnection()){
                OleDbCommand command = new OleDbCommand(selectStatement, connection);
                connection.Open();

                using(OleDbDataReader reader = command.ExecuteReader()){
                     int ticketNumberOrdinal = reader.GetOrdinal("TicketNumber");
                     int accountIdOrdinal = reader.GetOrdinal("AccountId");
                     int textOrdinal = reader.GetOrdinal("Text");
                     int dateOpenedOrdinal = reader.GetOrdinal("DateOpened");
                     int stateOrdinal = reader.GetOrdinal("State");
                     int deviceNameOrdinal = reader.GetOrdinal("DeviceName");

                    while (reader.Read()){
                        Ticket ticket;
                        string deviceName = reader.IsDBNull(deviceNameOrdinal) ? null : reader.GetString(deviceNameOrdinal);
                        if (deviceName == null){
                            ticket = new Ticket();
                        }else
                            ticket = new HardwareTicket() { DeviceName = deviceName};

                         ticket.TicketNumber = reader.GetInt32(ticketNumberOrdinal);
                         ticket.AccountId = reader.GetInt32(accountIdOrdinal);
                         ticket.Text = reader.GetString(textOrdinal);
                         ticket.DateOpened = reader.GetDateTime(dateOpenedOrdinal);
                         ticket.State = (TicketState)reader.GetByte(stateOrdinal);

                        tickets.Add(ticket);

                    }reader.Close();
                } connection.Close();
            }
            return tickets;
        }

        public BL.Domain.Ticket ReadTicket(int ticketNumber)
        {
            Ticket ticket = null;

            string selectStatement = "SELECT TicketNumber, AccountId,[Text], DateOpened, State, DeviceNameFROM Ticket WHERE TicketNumber =@ticketNumber";

            using (var connection = this.GetConnection())
            {
                OleDbCommand command = new OleDbCommand(selectStatement, connection);
                command.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                connection.Open();

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    int ticketNumberOrdinal = reader.GetOrdinal("TicketNumber");
                    int accountIdOrdinal = reader.GetOrdinal("AccountId");
                    int textOrdinal = reader.GetOrdinal("Text");
                    int dateOpenedOrdinal = reader.GetOrdinal("DateOpened");
                    int stateOrdinal = reader.GetOrdinal("State");
                    int deviceNameOrdinal = reader.GetOrdinal("DeviceName");

                    if (reader.Read())
                    {

                        string deviceName = reader.IsDBNull(deviceNameOrdinal) ? null : reader.GetString(deviceNameOrdinal);
                        if (deviceName == null)
                        {
                            ticket = new Ticket();
                        }
                        else
                            ticket = new HardwareTicket() { DeviceName = deviceName };

                        ticket.TicketNumber = reader.GetInt32(ticketNumberOrdinal);
                        ticket.AccountId = reader.GetInt32(accountIdOrdinal);
                        ticket.Text = reader.GetString(textOrdinal);
                        ticket.DateOpened = reader.GetDateTime(dateOpenedOrdinal);
                        ticket.State = (TicketState)reader.GetByte(stateOrdinal);
                    } reader.Close();
                } connection.Close();
            }
            return ticket;
        }


        public void UpdateTicket(BL.Domain.Ticket ticket)
        {
            string updateStatement = "UPDATE Ticket SET AccountId =@accountId, [Text] = @text, DateOpened =@dateOpened, State = @state, DeviceName= @deviceName WHERE TicketNumber =@ticketNumber";
            using (var connection = this.GetConnection())
            {
                OleDbCommand command = new OleDbCommand(updateStatement, connection);
                command.Parameters.AddWithValue("@ticketNumber", ticket.TicketNumber);
                command.Parameters.AddWithValue("@accountId", ticket.AccountId);
                command.Parameters.AddWithValue("@text", ticket.Text);
                command.Parameters.AddWithValue("@dateOpened", ticket.DateOpened.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@state", (byte)ticket.State);
                if (ticket is HardwareTicket)
                    command.Parameters.AddWithValue("@deviceName", ((HardwareTicket)ticket).DeviceName);
                else
                    command.Parameters.AddWithValue("@deviceName", DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeleteTicket(int ticketNumber)
        {
            string deleteTicketStatement = "DELETE FROM Ticket WHERE TicketNumber = @ticketNumber";
            string deleteResponsesOfTicketStatement = "DELETE FROM TicketResponse"
            + " WHERE Ticket_TicketNumber = @ticketNumber";
            using (var connection = this.GetConnection())
            {
                var ticketCmd = new OleDbCommand(deleteTicketStatement, connection);
                ticketCmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                var responsesCmd = new OleDbCommand(deleteResponsesOfTicketStatement, connection);
                responsesCmd.Parameters.AddWithValue("@ticketNumber", ticketNumber);

                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    responsesCmd.Transaction = transaction;
                    ticketCmd.Transaction = transaction;
                    responsesCmd.ExecuteNonQuery();
                    ticketCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                connection.Close();
            }
        }

        public IEnumerable<BL.Domain.TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber)
        {
            List<TicketResponse> requestedTicketResponses = new List<TicketResponse>();
            string sql = "SELECT TicketResponse.Id AS rId, TicketResponse.[Text] AS rText, [Date]"
            + ", IsClientResponse FROM TicketResponse INNER JOIN Ticket"
            + " ON Ticket.TicketNumber = TicketResponse.Ticket_TicketNumber"
            + " WHERE Ticket.TicketNumber = @ticketNumber";
            using (var connection = this.GetConnection())
            {
                 OleDbCommand command = new OleDbCommand(sql, connection);
                 command.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                 connection.Open();

                using (OleDbDataReader reader = command.ExecuteReader())
                 {
                     int idOrdinal = reader.GetOrdinal("rId");
                     int responseOrdinal = reader.GetOrdinal("rText");
                     int dateOrdinal = reader.GetOrdinal("Date");
                     int isClientOrdinal = reader.GetOrdinal("IsClientResponse");
                     while (reader.Read())
                     {
                         requestedTicketResponses.Add(new TicketResponse()
                         {
                             Id = reader.GetInt32(idOrdinal)
                         ,
                             Text = reader.GetString(responseOrdinal)
                         ,
                             Date = reader.GetDateTime(dateOrdinal)
                         ,
                             IsClientResponse = reader.GetBoolean(isClientOrdinal)
                         });
                     }
                     reader.Close(); // good practice!
                 }
                connection.Close(); // good practice!
            }
            return requestedTicketResponses;    
        }

        public BL.Domain.TicketResponse CreateTicketResponse(BL.Domain.TicketResponse response)
        {
            if (response.Ticket != null)
            {
                string insertStatement = "INSERT INTO TicketResponse([Text], [Date], IsClientResponse"
                + ", Ticket_TicketNumber) VALUES (@text, @date"
                + ", @isClientResponse, @tickedNumber)";
                using (var connection = this.GetConnection())
                 {
                    OleDbCommand command = new OleDbCommand(insertStatement, connection);
                    command.Parameters.AddWithValue("@text", response.Text);
                    command.Parameters.AddWithValue("@date", response.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@isClientResponse", response.IsClientResponse);
                    command.Parameters.AddWithValue("@tickedNumber", response.Ticket.TicketNumber);
                    connection.Open();
                    command.ExecuteNonQuery();
                    // Retrieve primary key ‘Id' of inserted ticketresponse
                    //When retrieving this data from Access, we must create and execute 2 commands
                    string retrievalQuery = "Select @@Identity";
                    OleDbCommand retrievalOfNewNbrCommand = new OleDbCommand(retrievalQuery, connection);
                    response.Id = Convert.ToInt32(retrievalOfNewNbrCommand.ExecuteScalar());
                    connection.Close(); // good practice!
                 }
                return response;
            }
            else
                throw new ArgumentException("The ticketresponse has no ticket attached to it");
        }


        public void UpdateTicketStateToClosed(int ticketNumber)
        {
            using (var connection = this.GetConnection())
            {
                OleDbCommand command = new OleDbCommand("sp_CloseTicket", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ticketNumber", ticketNumber);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close(); // good practice!
            }

        }
    }
}
