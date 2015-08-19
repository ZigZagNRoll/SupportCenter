using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain;

namespace SC.DAL
{
    public class UserRepository : IUserRepository
    {
        private List<ChatUser> users;

        public UserRepository()
        {
            users = new List<ChatUser>();
            Seed();
        }

        private void Seed()
        {
            // Create first ticket with three responses
            ChatUser user1 = new ChatUser()
            {
                ConnectionId="1",
                Name="Henk"
                
            };
            users.Add(user1);

            // Create second ticket with one response
            ChatUser user2 = new ChatUser()
            {
                ConnectionId = "2",
                Name = "Sam"
            };
            users.Add(user2);
        }

        public ChatUser NewUser(ChatUser user)
        {
            users.Add(user);
            return user;
        }

        public IEnumerable<ChatUser> ReadUsers()
        {
            return users;
        }


        public ChatUser ReadUser(string connectionId)
        {
            return users.Find(t => t.ConnectionId == connectionId);
        }
        
        public void DeleteUser(string connectionId)
        {
            users.Remove(ReadUser(connectionId));
        }
    }
}
