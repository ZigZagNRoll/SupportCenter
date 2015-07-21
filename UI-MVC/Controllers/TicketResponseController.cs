using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SC.BL;
using SC.BL.Domain;

namespace SC.UI.Web.MVC.Controllers
{
    [Authorize]
    public class TicketResponseController : Controller
    {
        TicketManager mgr = new TicketManager();

        public string CreateRPC(int ticketNumber, string responseText, bool isClientResponse)
        {
            if (User.IsInRole("Admin"))
            {
                isClientResponse = false;
            }
            else
                isClientResponse = true;

            TicketResponse response = mgr.AddTicketResponse(ticketNumber, responseText, isClientResponse);

            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Id = response.Id,
                Text = response.Text,
                Date = response.Date,
                IsClientResponse = response.IsClientResponse,
                TicketNumberOfTicket = response.Ticket.TicketNumber
            });
        }
    }
}