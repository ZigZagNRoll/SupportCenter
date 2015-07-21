using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain;

namespace SC.BL
{
    public  interface ITicketManager
    {
        IEnumerable<Ticket> GetTickets();
        Ticket GetTicket(int ticketNumber);
        Ticket AddTicket(int acountID, string question);
        Ticket AddTicket(int acountID, string device, string problem);
        void ChangeTicket(Ticket ticket);
        void RemoveTicket(int ticketNumber);
        IEnumerable<TicketResponse> GetTicketResponses(int ticketNumber);
        TicketResponse AddTicketResponse(int ticketNumber, string response, bool isClientResponse);
        void ChangeTicketStateToClosed(int ticketNumber);

    }
}
