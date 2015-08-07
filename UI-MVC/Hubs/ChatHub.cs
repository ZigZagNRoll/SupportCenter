using System;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SC.UI.Web.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
        public void SendNewUser(string name)
        {
            Clients.All.addNewActiveUser(name);
        }
        public void SendRemoveUser(string name)
        {
            Clients.All.removeActiveUser(name);
        }
    }
}