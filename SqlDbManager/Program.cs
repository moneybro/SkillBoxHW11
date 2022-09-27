using ClassLibrary.Classes;
using ClassLibrary.Contexts;
using ClassLibrary.Methods;
using Microsoft.EntityFrameworkCore;
using SqlDbManager;

SqlDbContext db = new SqlDbContext();

db.Database.EnsureDeleted();
db.Database.EnsureCreated();

var actions = new BankAccActionsJSON();

var depos = actions.getAllDepoAccs();
var mains = actions.getAllMainAccs();
var clients = ClientCommonMethods.GetClientsAllData();
var trans = actions.getAllTransactions();

foreach (var item in depos)
{
    db.DepoAccs.Add(item);
}

foreach (var item in mains)
{
    db.MainAccs.Add(item);
}

foreach (var item in clients)
{
    if (item.Status == "active" && !db.Clients.Contains(item))
    {
        db.Clients.Add(item);
    }
}

//db.SaveChanges();

using (var tr = db.Database.BeginTransaction())
{
    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Clients ON");

    foreach (var item in trans)
    {
        db.Transactions.Add(item);
    }
    db.SaveChanges();
    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Clients off");
    tr.Commit();
}

