using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Text;
using System.IO;
using SC.BL;
using System.Collections.Generic;
using SC.BL.Domain;
namespace SC.UI.Web.Hubs
{
    public class ChatHub : Hub
    {        
        private readonly IUserManager mgr = new UserManager();

        public void Connect(string userName){
            var id = Context.ConnectionId;
                IEnumerable<ChatUser>users = mgr.Users();
                
                // send to caller
                foreach(ChatUser user in users){
                    Clients.Caller.onConnected(user.Name);
                }
                mgr.AddUser(id, userName);
                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(userName);
                
        }

        public void Send(string currentDateTime, string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(currentDateTime, name, message);
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
                var id = Context.ConnectionId;
                mgr.RemoveUser(id);
                Clients.All.removeActiveUser();
                IEnumerable<ChatUser> users = mgr.Users();
                foreach (ChatUser user in users)
                {
                    Clients.All.fillActiveUser(user.Name);
                }

            return base.OnDisconnected();
        }
    }
}