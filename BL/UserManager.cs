using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain;
using SC.DAL;

namespace SC.BL
{
    public class UserManager : IUserManager
    {
        public readonly IUserRepository repo;

        public UserManager()
        {
            this.repo = new SC.DAL.UserRepository();
        }

        public IEnumerable<ChatUser> Users()
        {
            return repo.ReadUsers();
        }

        public ChatUser GetUser(string connectedId)
        {
            return repo.ReadUser(connectedId);
        }

        public ChatUser AddUser(string connectedId, string name)
        {
            ChatUser user = new ChatUser()
            {
                Name=name,
                ConnectionId=connectedId
            };
            return repo.NewUser(user);
        }

        public void RemoveUser(string connectionId)
        {
            repo.DeleteUser(connectionId);
        }
    }
}
