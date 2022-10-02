using ClassLibrary.Contexts;
using ClassLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class ClientActionsSql : IClientActions
    {
        public static SqlDbContext db = new SqlDbContext();
        //public ClientActionsSql()
        //{
        //    //db = new SqlDbContext();
        //}
        //public ClientActionsSql(DbContext dbContext)
        //{
        //    //db = (SqlDbContext?)dbContext;
        //}
        public List<Client> GetClients()
        {
            //todo для каждого работника хранится набор данных, из-за этого данные после редактирования одним работником для других работников не отображаются, каждый видит свои последние данные, а обновленные данные не загружаются из БД. требуется принудительно перечитать данные их БД. Только для реализации с EF Core, связано с отслеживанием объектов EF
            
            return db.Clients.Where(c => c.Status != "deleted").ToList();
        }
        public long getNewClientId()
        {
            var newClId = (long)(db.Clients.Max(c => c.ID)) + 1;
            return newClId;
        }
        public bool UpdateClient(Client client)
        {
            if (client == null) return false;

            //var cl = db.Clients.FirstOrDefault(c => c.ID == client.ID);
            //cl.LastName = client.LastName;
            //cl.FirstName = client.FirstName;
            //cl.Patronymic = client.Patronymic;
            //cl.MobPhone = client.MobPhone;
            //cl.PaspSeria = client.PaspSeria;
            //cl.PaspNum = client.PaspNum;
            //cl.LastChangeDate = client.LastChangeDate;
            //cl.ChangeType = "edited";
            //cl.EmployeeType = client.EmployeeType;
            //cl.Status = client.Status;
            //client = null;
            //db.Clients.Update(cl);
            //cl = null;
            //db.SaveChanges();
            //return true;

            client.ChangeType = "edited";
            db.Clients.Update(client);
            db.SaveChanges();
            return true;
        }
        public bool DeleteClient(Client client, string employeeType)
        {
            Client clientToDel = client;
            if (clientToDel != null)
            {
                clientToDel.Status = "deleted";
                clientToDel.EmployeeType = employeeType;
                if (UpdateClient(clientToDel))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool SaveNewClient(Client client)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Clients ON");

                db.Clients.Add(client);
                db.SaveChanges();

                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Clients OFF");
                tr.Commit();
            }
            return true;
        }
    }
}
