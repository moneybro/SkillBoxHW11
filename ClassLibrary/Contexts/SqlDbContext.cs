using ClassLibrary.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Contexts
{
    public class SqlDbContext : DbContext
    {
        public DbSet<Client> Clients {get; set;}
        public DbSet<BankAccMain> MainAccs {get; set;}
        public DbSet<BankAccDepo> DepoAccs {get; set;}
        public DbSet<BankAccTransaction> Transactions {get; set;}

        public SqlDbContext() { }

        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; initial catalog=SkillBoxBankSqlDb; integrated security=true;");
            optionsBuilder.EnableSensitiveDataLogging();
        }


    }
}
