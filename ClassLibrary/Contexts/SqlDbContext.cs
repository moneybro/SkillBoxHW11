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
            optionsBuilder.UseSqlServer(@"Server=megaserver; initial catalog=SkillBoxBankSqlDb;User Id=SkillboxBankApp;Password=SkillboxBankApp1234567890!@#; integrated security=false;");

            //optionsBuilder.UseSqlServer(@"Server=tcp:sbb.fenske.ru,1433; initial catalog=SkillBoxBankSqlDb;User Id=SkillboxBankApp;Password=SkillboxBankApp1234567890!@#; integrated security=false;");

            //optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; initial catalog=SkillBoxBankSqlDb; integrated security=true;");
            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);  // отключение отслежвания наборов
        }


    }
}
