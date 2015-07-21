    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SC.BL.Domain;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.DAL.EF
{
    [DbConfigurationType(typeof(TicketEFDbConfiguration))]
    class TicketEFDbContext : DbContext
    {
        public TicketEFDbContext() :base("SupportCenterDB_EFCodeFirst")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasKey(t=> t.TicketNumber);
            modelBuilder.Entity<Ticket>().Property(t => t.State).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<HardwareTicket> HardwareTickets { get; set; }
        public DbSet<TicketResponse> TicketResponses { get; set; }
    }
}
