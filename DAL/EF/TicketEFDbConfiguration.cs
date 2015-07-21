using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace SC.DAL.EF
{
    class TicketEFDbConfiguration : DbConfiguration
    {
        public TicketEFDbConfiguration()
        {
            this.SetDefaultConnectionFactory(new SqlConnectionFactory());
            this.SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            this.SetDatabaseInitializer<TicketEFDbContext>(new TicketEFDbInitializer());
        }
    }
}
