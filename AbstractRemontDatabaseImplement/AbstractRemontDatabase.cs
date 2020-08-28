using AbstractRemontDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace AbstractRemontDatabaseImplement
{
    public class AbstractRemontDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=LIZA\SQLEXPRESS;Initial Catalog=AbstractRemontDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Component> Components { set; get; }
        public virtual DbSet<Ship> Ships { set; get; }
        public virtual DbSet<ShipComponent> ShipComponents { set; get; }
        public virtual DbSet<Remont> Remonts { set; get; }
        public virtual DbSet<Client> Clients { set; get; }
        public virtual DbSet<Implementer> Implementers { set; get; }
        public virtual DbSet<MessageInfo> MessageInfos { set; get; }
    }
}
