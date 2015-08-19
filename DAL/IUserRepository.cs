using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain;

namespace SC.DAL
{
    public interface IUserRepository
    {
        ChatUser NewUser(ChatUser user);
        IEnumerable<ChatUser> ReadUsers();
        ChatUser ReadUser(string connectionId);
        void DeleteUser(string connectionId);
    }
}
