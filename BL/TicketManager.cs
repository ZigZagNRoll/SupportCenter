using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.DAL;
using SC.DAL.OleDb;
using SC.DAL.SqlClient;
using SC.BL.Domain;
using System.ComponentModel.DataAnnotations;

namespace SC.BL
{
    public class TicketManager : ITicketManager
    {
        
        public readonly ITicketRepository repo;

        public TicketManager()
        {
            this.repo = new SC.DAL.EF.TicketRepository();
        }

        public IEnumerable<Domain.Ticket> GetTickets()
        {
            return repo.ReadTickets();
        }

        public Domain.Ticket AddTicket(int acountID, string question)
        {
            Domain.Ticket t = new Domain.Ticket()
            {
                AccountId = acountID,
                Text = question,
                DateOpened = DateTime.Now,
                State = Domain.TicketState.Open,
            };
            this.Validate(t);
            return repo.CreateTicket(t);
        }

        public Domain.Ticket AddTicket(int acountID, string device, string problem)
        {
            Ticket t = new HardwareTicket()
            {
                AccountId = acountID,
                Text = problem,
                DateOpened = DateTime.Now,
                State = TicketState.Open,
                DeviceName = device
            };
            this.Validate(t);
            return repo.CreateTicket(t);
        }

        //private void Validate(Ticket ticket)
        //{
        //    Validator.ValidateObject(ticket, new ValidationContext(ticket)
        //    , validateAllProperties: true);
        //}

        private void Validate(TicketResponse response)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(response, new ValidationContext(response)
            , errors, validateAllProperties: true);
            if (!valid)
                throw new ValidationException("TicketResponse not valid!");
        }   


        public Ticket GetTicket(int ticketNumber)
        {
            return repo.ReadTicket(ticketNumber);
        }

        public void ChangeTicket(Ticket ticket)
        {
            this.Validate(ticket);
            repo.UpdateTicket(ticket);
        }

        private void Validate(Ticket ticket)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(ticket, new ValidationContext(ticket), errors
            , validateAllProperties: true);
            foreach (ValidationResult e in errors)
                Console.WriteLine(e);
            if (!valid)
                throw new ValidationException("TicketResponse not valid!");
        }

        public void RemoveTicket(int ticketNumber)
        {
            repo.DeleteTicket(ticketNumber);
        }

        public IEnumerable<TicketResponse> GetTicketResponses(int ticketNumber)
        {
            return repo.ReadTicketResponsesOfTicket(ticketNumber);
        }

        public TicketResponse AddTicketResponse(int ticketNumber, string response, bool isClientResponse)
        {
            Ticket ticketToAddResponseTo = this.GetTicket(ticketNumber);
             if (ticketToAddResponseTo != null)
             {
                 // Create response
                 TicketResponse newTicketResponse = new TicketResponse();
                 newTicketResponse.Date = DateTime.Now;
                 newTicketResponse.Text = response;
                 newTicketResponse.IsClientResponse = isClientResponse;
                 newTicketResponse.Ticket = ticketToAddResponseTo;

                 // Add response to ticket
                 var responses = this.GetTicketResponses(ticketNumber);
                 if (responses != null)
                     ticketToAddResponseTo.Responses = responses.ToList();
                 else
                    ticketToAddResponseTo.Responses = new List<TicketResponse>();

                 ticketToAddResponseTo.Responses.Add(newTicketResponse);

                 // Change state of ticket
                 if (isClientResponse)
                     ticketToAddResponseTo.State = TicketState.ClientAnswer;
                 else
                     ticketToAddResponseTo.State = TicketState.Answered;

                 this.Validate(newTicketResponse);
                 this.Validate(ticketToAddResponseTo);
                 // Save changes to repository
                 repo.CreateTicketResponse(newTicketResponse);
                 repo.UpdateTicket(ticketToAddResponseTo);
                 return newTicketResponse;
              }
                 else
                     throw new ArgumentException("Ticketnumber '" + ticketNumber + "' not found!");
        }


        public void ChangeTicketStateToClosed(int ticketNumber)
        {
            repo.UpdateTicketStateToClosed(ticketNumber);
        }
    }
}
