using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.BL.Domain;

namespace SC.BL
{
    public interface IUserManager
    {
        IEnumerable<ChatUser> Users();
        ChatUser GetUser(string connectedId);
        ChatUser AddUser(string connectedId, string name);
        void RemoveUser(string connectionId);
    }
}
