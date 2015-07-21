using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain;
using System.Data.Entity;

namespace SC.DAL.EF
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketEFDbContext ctx;

        public TicketRepository()
        {
            ctx = new TicketEFDbContext();
        }
        public BL.Domain.Ticket CreateTicket(BL.Domain.Ticket ticket)
        {
            ctx.Tickets.Add(ticket);
            ctx.SaveChanges();
            return ticket;
        }

        public IEnumerable<BL.Domain.Ticket> ReadTickets()
        {
            return ctx.Tickets
                .Include(t => t.Responses)
                .AsEnumerable();
        }

        public BL.Domain.Ticket ReadTicket(int ticketNumber)
        {
            return ctx.Tickets.Find(ticketNumber);
        }

        public void UpdateTicket(BL.Domain.Ticket ticket)
        {
            ctx.Entry(ticket).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteTicket(int ticketNumber)
        {
            ctx.Tickets.Remove(ctx.Tickets.Find(ticketNumber));
            ctx.SaveChanges();
        }

        public IEnumerable<BL.Domain.TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber)
        {
            return ctx.TicketResponses.Where(r => r.Ticket.TicketNumber == ticketNumber).AsEnumerable();
        }

        public BL.Domain.TicketResponse CreateTicketResponse(BL.Domain.TicketResponse response)
        {
            ctx.TicketResponses.Add(response);
            ctx.SaveChanges();
            return response;
        }

        public void UpdateTicketStateToClosed(int ticketNumber)
        {
            ctx.Tickets.Find(ticketNumber).State = BL.Domain.TicketState.Closed;
            ctx.SaveChanges();
        }
    }
}
