using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using SC.BL;
using SC.BL.Domain;
using SC.BL.Domain.Validation;

namespace UI_CA
{
    class Program
    {
        private static bool quit = false;
        private static readonly ITicketManager mgr = new TicketManager();
        static void Main(string[] args)
        { 
            while (!quit)
            ShowMenu();
        }

        private static void ShowMenu()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("=== HELPDESK - SUPPORT CENTER ===");
            Console.WriteLine("=================================");
            Console.WriteLine("1) Toon alle tickets");
            Console.WriteLine("2) Toon details van een ticket");
            Console.WriteLine("3) Toon de antwoorden van een ticket");
            Console.WriteLine("4) Maak een nieuw ticket");
            Console.WriteLine("5) Geef een antwoord op een ticket");
            Console.WriteLine("6) Markeer ticket als 'Closed'");
            Console.WriteLine("0) Afsluiten");
            DetectMenuAction();
        }

        private static void DetectMenuAction()
        {
            bool inValidAction = false;
            do
            {         
                Console.Write("Keuze: ");
                string input = Console.ReadLine();
                int action;
                if (Int32.TryParse(input, out action))
                {
                    switch (action)
                    {
                        case 1:
                            PrintAllTickets(); break;
                        case 2:
                            ActionShowTicketDetails(); break;
                        case 3:
                            ActionShowTicketResponses(); break;
                        case 4:
                            ActionCreateTicket(); break;
                        case 5:
                            ActionAddResponseToTicket(); break;
                        case 6:
                            ActionCloseTicket(); break;
                        case 0:
                            quit = true; return;
                        default:
                            Console.WriteLine("Geen geldige keuze!");
                            inValidAction = true;
                            break;
                    }
                }
            } while (inValidAction);
        }

        private static void ActionCloseTicket()
        {
            Console.Write("Ticketnummer: ");
            int input = Int32.Parse(Console.ReadLine());
            mgr.ChangeTicketStateToClosed(input);
        }

        private static void ActionAddResponseToTicket()
        {
            Console.Write("Ticketnummer: ");
            int ticketNumber = Int32.Parse(Console.ReadLine());
            Console.Write("Antwoord: ");
            string response = Console.ReadLine();
            mgr.AddTicketResponse(ticketNumber, response, false);
        }

        private static void ActionShowTicketResponses()
        {
            Console.Write("Ticketnummer: ");
            int input = Int32.Parse(Console.ReadLine());
            IEnumerable<TicketResponse> responses = mgr.GetTicketResponses(input);
            if (responses != null) PrintTicketResponses(responses);
        }

        private static void PrintTicketResponses(IEnumerable<TicketResponse> responses)
        {
            foreach (var r in responses)
                Console.WriteLine(r.GetInfo());
        }

        private static void ActionShowTicketDetails()
        {
            Console.Write("Ticketnummer: ");
            int input = Int32.Parse(Console.ReadLine());
            Ticket t = mgr.GetTicket(input);
            PrintTicketDetails(t);
        }

        private static void PrintTicketDetails(Ticket ticket)
        {
            Console.WriteLine("{0,-15}: {1}", "Ticket", ticket.TicketNumber);
            Console.WriteLine("{0,-15}: {1}", "Gebruiker", ticket.AccountId);
            Console.WriteLine("{0,-15}: {1}", "Datum", ticket.DateOpened.ToString("dd/MM/yyyy"));
            Console.WriteLine("{0,-15}: {1}", "Status", ticket.State);
            if (ticket is HardwareTicket)
                Console.WriteLine("{0,-15}: {1}", "Toestel", ((HardwareTicket)ticket).DeviceName);
            Console.WriteLine("{0,-15}: {1}", "Vraag/probleem", ticket.Text);
        }

        private static void ActionCreateTicket()
        {
            string device = "";
            Console.Write("Is het een hardware probleem (j/n)? ");
            bool isHardwareProblem = (Console.ReadLine().ToLower() == "j");
            if (isHardwareProblem)
            {
                Console.Write("Naam van het toestel: ");
                device = Console.ReadLine();
            }
            Console.Write("Gebruikersnummer: ");
            int accountNumber = Int32.Parse(Console.ReadLine());
            Console.Write("Probleem: ");
            string problem = Console.ReadLine();
            if (!isHardwareProblem)
                mgr.AddTicket(accountNumber, problem);
            else
                mgr.AddTicket(accountNumber, device, problem);
        }

        private static void PrintAllTickets()
        {
            foreach (var t in mgr.GetTickets())
                Console.WriteLine(t.GetInfo());
        }
    }
}
