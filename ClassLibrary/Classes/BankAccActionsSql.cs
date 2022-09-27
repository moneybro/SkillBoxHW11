using ClassLibrary.Classes;
using ClassLibrary.Contexts;
using ClassLibrary.Interfaces;
using ClassLibrary.Methods;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class BankAccActionsSql :
        IBankAccActions,
        IBankAccGetByAccNum,
        ITransactionsActions
    {
        SqlDbContext db;
        public BankAccActionsSql()
        {
            db = new SqlDbContext();
        }
        #region создание главного и депозитного счета
        public BankAccMain GetNewMainAcc(long clId)
        {
            BankAccMain newMainAcc = BankAccFabricStat.getNewMainAcc(clId);
            newMainAcc.AccNumber = GetNewAccNumber();
            if (newMainAcc.AccNumber == 0) return null;
            return newMainAcc;
        }
        public BankAccDepo GetNewDepoAcc(long clId)
        {
            BankAccDepo newDepoAcc = BankAccFabricStat.getNewDepoAcc(clId);
            newDepoAcc.AccNumber = GetNewAccNumber();
            if (newDepoAcc.AccNumber == 0) return null;
            DateTime now = DateTime.Now;
            if (SaveAcc(newDepoAcc, GlobalVarsAndActions.DepoAccsRepoPath, now, now))
            {
                return newDepoAcc;
            }
            else
            {
                return null;
            }
            return newDepoAcc;
        }
        #endregion

        #region получение нового номера счета для создания нового банковского счета

        /// <summary>
        /// получение нового номера счета для создания нового банковского счета
        /// </summary>
        /// <returns>номер счета</returns>        
        long GetNewAccNumber()
        {
            var accs = GetAllBankAccs();
            long newAccNumber = 0;
            if (accs.Count == 0)
            {
                newAccNumber = 1;
            }
            else
            {
                newAccNumber = accs.Max(a => a.AccNumber) + 1;
            }
            return newAccNumber;
        }
        #endregion

        #region сохранение информации об изменении параметров счетов в бд sql (таблица содержит не сами счета, а изменения счетов, ключевой параметр не ID счета, а ID изменения счета)
        /// <summary>
        /// метод сохранения для счета
        /// </summary>
        /// <typeparam name="T">тип объекта счета (обобщенный)</typeparam>
        /// <param name="acc">объект счета</param>
        /// <returns>истина если сохранение прошло успешно, ложь - сохранение не удалось</returns>
        public bool SaveAcc<T>(T acc, DateTime dateTime) where T : BankAccForClient
        {
            if (acc != null)
            {
                if(SaveAcc(acc, "", default, dateTime)) return true;
                else return false;
            }
            return false;
        }

        /// <summary>
        /// метод определения типа счета и сохранения счета в соответствующую таблицу
        /// </summary>
        /// <typeparam name="T">тип счета</typeparam>
        /// <param name="acc">счет</param>
        /// <param name="bankAccRepo">здесь уже не нужен</param> 
        /// <returns></returns>
        //todo убрать параметр bankAccRepo, который предназначен для хранения в JSON, требуется разделение интерфеса
        //todo подумать над архитектурой, если перечень типов счетов будет расширяться
        //если обновлять счет, то не будет видно всех движений по счету, это странная логика, но сейчас она пока такая
        //чтобы ее изменить нужно менять модель хранения
        //
        public bool SaveAcc<T>(T acc, string bankAccRepo, DateTime createDateTime, DateTime updateDateTime) where T : BankAccForClient
        {
            if (acc != null)
            {
                if (createDateTime == updateDateTime) // если дата создания и изменения равны, то это создание счета, если нет, то изменение
                {
                    acc.CreateDate = createDateTime;
                }
                acc.UpdateDate = updateDateTime;

                // пришел mainAcc
                if (acc.GetType() == typeof(BankAccMain))
                {
                    //было так. но если обновлять (update) счет, то не будет видно всех движений по счету, это странная логика, но сейчас она пока такая, она избыточна
                    //чтобы ее изменить нужно менять модель хранения
                    //
                    //if (db.MainAccs.Contains(acc as BankAccMain))
                    //{
                    //    db.MainAccs.Update(acc as BankAccMain);
                    //}
                    //else
                    //{
                    //    db.MainAccs.Add(acc as BankAccMain);
                    //}

                    using (var tr = db.Database.BeginTransaction())
                    {
                        acc.Id = db.MainAccs.Max(a => a.Id) + 1;
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.MainAccs ON");

                        db.MainAccs.Add(acc as BankAccMain);
                        db.SaveChanges();
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.MainAccs off");
                        tr.Commit();
                    }
                    return true;
                }

                // пришел DepoAcc
                if (acc.GetType() == typeof(BankAccDepo))
                {
                    using (var tr = db.Database.BeginTransaction())
                    {
                        acc.Id = db.DepoAccs.Max(a => a.Id) + 1;
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.DepoAccs ON");

                        db.DepoAccs.Add(acc as BankAccDepo);
                        db.SaveChanges();
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.DepoAccs off");
                        tr.Commit();
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region закрытие счета
        /// <summary>
        /// метод для закрытия счета: поле active переводится в false
        /// </summary>
        /// <param name="accNum"></param>
        /// <returns></returns>
        public bool CloseAcc(long accNum)
        {
            var accs = GetAllBankAccs();
            var accToClose = accs.Find(a => a.AccNumber == accNum);
            if (accToClose == null) { return false; }
            else
            {
                accToClose.Active = false;
                if (SaveAcc(accToClose, DateTime.Now)) return true;
                else return false;
                return true;
            }
        }
        #endregion

        #region получение всех счетов, независимо от типа

        /// <summary>
        /// получение всех счетов, независимо от типа
        /// </summary>
        /// <returns>список счетов</returns>
        List<BankAccForClient> GetAllBankAccs()
        {
            List<BankAccForClient> accs = new List<BankAccForClient>();
            accs.AddRange(db.MainAccs);
            accs.AddRange(db.DepoAccs);
            return accs;
        }
        #endregion

        #region получение активных счетов, принадлежащих клиенту
        
        /// <summary>
        /// метод для получения активных счетов, принадлежащих клиенту
        /// </summary>
        /// <param name="clId"></param>
        /// <returns></returns>
        public List<BankAccForClient> GetClientAccs(long clId)
        {
            List<BankAccForClient> bankAccList = new List<BankAccForClient>();

            var clientAccs = GetAllBankAccs()
                .Where(a => a.ClientId == clId)
                .ToList();

            var uniqAccs = clientAccs
                .GroupBy(a => a.AccNumber)
                .ToDictionary(a => a.Key, a => a.ToList());

            foreach (var acc in uniqAccs)
            {
                var lastEl = acc.Value.Last();
                if (lastEl.Active)
                {
                    bankAccList.Add(lastEl);
                }
            }
            return bankAccList;
        }
        #endregion

        #region получение активного счета по его номеру
        public BankAccForClient GetAccByNum(long accNumber)
        {
            var accs = GetAllBankAccs().Where(a => a.AccNumber == accNumber);
            foreach (var acc in accs.Reverse())
            {
                if (acc.Active == true)
                {
                    return acc;
                }
                else continue;
            }
            return null;
        }
        #endregion

        #region получение перечня движений по счету
        public List<BankAccForClient> GetAccTransactions(long accNum)
        {
            var allAccs = GetAllBankAccs();
            var result = allAccs.Where(a => a.AccNumber == accNum).ToList();
            return result;
        }
        #endregion

        #region сохранение транзакции
        public bool SaveTransaction(BankAccTransaction tr)
        {
            try
            {
                db.Transactions.Add(tr);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region получение всех транзакций
        List<BankAccTransaction> getAllTransactions()
        {
            var trs = db.Transactions.ToList();
            return trs;
        }
        #endregion
        #region получение коллекции расширенных транзакций, которые состоят из изменения счета + описание движения
        public List<BankAccTransactionFull> GetBankAccTransactionsFull(long accNum)
        {
            List<BankAccTransactionFull> trsf = new List<BankAccTransactionFull>();
            var accs = GetAccTransactions(accNum);
            var trs = getAllTransactions();
            foreach (var acc in accs)
            {
                foreach (var tr in trs)
                {
                    if ((tr.AccNumberSource == accNum || tr.AccNumberTarget == accNum) && acc.UpdateDate == tr.Date)
                    {
                        trsf.Add(new BankAccTransactionFull() { Acc = acc, Tr = tr });
                    }
                }
            }
            return trsf;
        }
        #endregion
    }
}
