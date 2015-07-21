using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using SC.BL;
using SC.BL.Domain;

namespace SC.UI.Web.MVC.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ITicketManager mgr = new TicketManager();

        // GET: Ticket
        public ActionResult Index()
        {
            IEnumerable<Ticket> tickets = mgr.GetTickets();
            if (!User.IsInRole("Admin"))
               tickets = tickets.Where(t => t.AccountId == WebSecurity.CurrentUserId);

            return View(tickets);
        }

        // GET: Ticket/Details/5
        
        public ActionResult Details(int id)
        {
            Ticket ticket = mgr.GetTicket(id);
            if (User.IsInRole("Admin") || ticket.AccountId == WebMatrix.WebData.WebSecurity.CurrentUserId)
            {
                ticket.Responses = new List<TicketResponse>(mgr.GetTicketResponses(id));
                return View(ticket);
            }
            return View("Error");
        }

        // GET: Ticket/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ticket/Create
        [HttpPost]
        public ActionResult Create(Ticket ticket)
        {
            try
            {
                // TODO: Add insert logic here
                ticket = mgr.AddTicket(ticket.AccountId, ticket.Text);
                return RedirectToAction("Details", new { id=ticket.TicketNumber});
            }
            catch
            {
                return View();
            }
        }

        // GET: Ticket/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Ticket ticket = mgr.GetTicket(id);
            return View(ticket);
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Ticket ticket)
        {
            try
            {
                // TODO: Add update logic here
                mgr.ChangeTicket(ticket);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ticket/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Ticket ticket = mgr.GetTicket(id);
            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                mgr.RemoveTicket(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
