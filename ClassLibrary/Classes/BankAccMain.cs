using ClassLibrary.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ClassLibrary.Classes
{
    public class BankAccMain : BankAccForClient, IPushMoney<BankAccForClient>
    {
        public string AccType => "Main";

        public BankAccMain()
        {
        }

            /// <summary>
            /// конструктор для создания нового счета
            /// </summary>
            /// <param name="clId">идентификатор клиента</param> 
            public BankAccMain(long clId)
        {
            this.ClientId = clId;
            this.Active = true;
        }

        /// <summary>
        /// конструктор для десериализации и для загрузки объектов из БД
        /// </summary>
        /// <param name="clId">идентификатор клиента</param>
        /// <param name="accNum">номер счета</param>
        /// <param name="amount">баланс</param>
        /// <param name="active">состояние счета (активен/не активен (закрыт))</param>
        /// <param name="createDate">дата создания</param>
        /// <param name="updateDate">дата обновления</param>
        [JsonConstructor]
        public BankAccMain(
             long clId,
             long accNum,
             decimal amount,
             bool active,
             DateTime createDate,
             DateTime updateDate
             )
        {
            this.ClientId = clId;
            this.AccNumber = accNum;
            this.Amount = amount;
            this.Active = active;
            this.CreateDate = createDate;
            this.UpdateDate = updateDate;
        }

        public BankAccForClient PushMoneyToAcc(decimal summ)
        {
            this.Amount += summ;
            return this;
        }
        public override string ToString()
        {
            return $"ClientId: {ClientId}\n" +
                $"AccNumber: {AccNumber}\n" +
                $"Acc type: NoDepo\n" +
                $"Amount: {Amount}\n" +
                $"Active: {Active}\n" +
                $"Create date: {CreateDate}\n";
        }

        
    }
}
