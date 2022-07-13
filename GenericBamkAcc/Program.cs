using GenericBankAcc;
using Newtonsoft.Json;
using Serilog;

using System.Collections;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

List<BankAcc> bankAccs = new List<BankAcc>();

AccFabric<BankAccDepo> acc1 = new AccFabric<BankAccDepo>(new BankAccDepo(111, 111, 0, true, DateTime.Now, DateTime.Now) { });
AccFabric<BankAccDepo> acc2 = new AccFabric<BankAccDepo>(new BankAccDepo(111, 111, 0, true, DateTime.Now, DateTime.Now) { });
AccFabric<BankAccDepo> acc3 = new AccFabric<BankAccDepo>(new BankAccDepo(111, 111, 0, true, DateTime.Now, DateTime.Now) { });
AccFabric<BankAccDepo> acc4 = new AccFabric<BankAccDepo>(new BankAccDepo(111, 111, 0, true, DateTime.Now, DateTime.Now) { });
AccFabric<BankAccMain> acc5 = new AccFabric<BankAccMain>(new BankAccMain(222, 222, 2220, true, DateTime.Now, DateTime.Now, "qqq") { });
AccFabric<BankAccMain> acc6 = new AccFabric<BankAccMain>(new BankAccMain(222, 222, 2220, true, DateTime.Now, DateTime.Now, "qqq") { });
AccFabric<BankAccMain> acc7 = new AccFabric<BankAccMain>(new BankAccMain(222, 222, 2220, true, DateTime.Now, DateTime.Now, "qqq") { });

Log.Information($"log event /r/n{acc7.acc}");
Log.Information($"log event /r{acc7.acc}");
Log.Information($"log event /r{acc7.acc}");

acc1.acc.PushMoneyToAcc(1000);
acc5.acc.PushMoneyToAcc(1000);

IBank<Bank> Bank1 = acc1.acc;
IBank<Bank> Bank2 = acc5.acc;

Bank1.PushMoneyToAcc(2000);
Bank2.PushMoneyToAcc(2000);

MoneyTransferClass moneyTransfer = new MoneyTransferClass();
moneyTransfer.transferMoney((BankAcc)Bank1, (BankAcc)Bank2, 1000);



//Save(acc1.acc, "depoAccRepo.json");
//Save(acc2.acc, "depoAccRepo.json");
//Save(acc3.acc, "depoAccRepo.json");
//Save(acc4.acc, "depoAccRepo.json");
//Save(acc5.acc, "mainAccRepo.json");
//Save(acc6.acc, "mainAccRepo.json");
//Save(acc7.acc, "mainAccRepo.json");


bankAccs.Add(acc1.acc);
bankAccs.Add(acc2.acc);
bankAccs.Add(acc3.acc);
bankAccs.Add(acc4.acc);
bankAccs.Add(acc5.acc);
bankAccs.Add(acc6.acc);
bankAccs.Add(acc7.acc);

//foreach (var item in bankAccs)
//{
//    Console.WriteLine(item);
//}

#region from json
//var list = getAllAccs();

//foreach (var item in list)
//{
//    Console.WriteLine(item);
//}
#endregion

#region manual created

foreach (BankAcc acc in bankAccs)
{
    Console.WriteLine(acc);
}

#endregion

Console.WriteLine(new String('-', 50));
Console.WriteLine("--- kontrvar part ----");

Console.WriteLine($"acc1 amount = {acc1.acc.Amount}");
Console.WriteLine($"acc5 amount = {acc5.acc.Amount}");

IStorageTransferMoney<BankAcc> transferStorage = new BankAccTransferStorage<Bank>();

transferStorage.addAcc = acc5.acc;
transferStorage.addAcc = acc1.acc;

transferStorage.addAcc = acc5.acc;
transferStorage.TransferMoney(100);

Console.WriteLine("transfer 100");

Console.WriteLine($"acc1 amount = {acc1.acc.Amount}");
Console.WriteLine($"acc5 amount = {acc5.acc.Amount}");

List<BankAcc> getAllAccs()
{
    List<BankAcc> accs = new List<BankAcc>();
    accs.AddRange(getAllMainAccs());
    accs.AddRange(getAllDepoAccs());
    return accs;
}

List<BankAccDepo> getAllDepoAccs()
{
    List<BankAccDepo> accs = new List<BankAccDepo>();
    string bancAccRepo = "depoAccRepo.json";
    using (var sr = new StreamReader(bancAccRepo, new UTF8Encoding()))
    {
        var ser = new Newtonsoft.Json.JsonSerializer();
        var reader = new JsonTextReader(sr);
        while (reader.Read())
        {
            reader.CloseInput = false;
            reader.SupportMultipleContent = true;

            var accObj = ser.Deserialize<BankAccDepo>(reader);
            accs.Add(accObj);
        }
    }
    return accs;
}

List<BankAccMain> getAllMainAccs()
{
    List<BankAccMain> accs = new List<BankAccMain>();
    string bancAccRepo = "mainAccRepo.json";
    using (var sr = new StreamReader(bancAccRepo, new UTF8Encoding()))
    {
        var ser = new Newtonsoft.Json.JsonSerializer();
        var reader = new JsonTextReader(sr);
        while (reader.Read())
        {
            reader.CloseInput = false;
            reader.SupportMultipleContent = true;

            var accObj = ser.Deserialize<BankAccMain>(reader);
            accs.Add(accObj);
        }
    }
    return accs;
}

void Save<T>(T acc, string bankAccRepo)
{
    var options = new JsonSerializerOptions
    {
        WriteIndented = true, //фрматированный json
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    using (FileStream fs = new FileStream(bankAccRepo, FileMode.Append))
    {
        System.Text.Json.JsonSerializer.SerializeAsync<T>(fs, acc, options);
    }
}

Console.ReadKey();

class AccFabric<T> where T : BankAcc
{
    public T acc;
    public AccFabric(T arg)
    {
        acc = arg;
    }
}