using ClassLibrary.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibrary.Classes
{
    public class BankAccDepo : BankAccForClient, IPushMoney<BankAccForClient>
    {
        bool _canAddMoney;

        /// <summary>
        /// конструктор для создания нового счета
        /// </summary>
        /// <param name="clId">идентификатор клиента</param> 
        public BankAccDepo(long clId)
        {
            this.ClientId = clId;
            this.CanAddMoney = true;
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
        public BankAccDepo(
            long clId,
            long accNum,
            decimal amount,
            bool active,
            DateTime createDate,
            DateTime updateDate,
            bool canAddMoney
            )
        {
            this.ClientId = clId;
            this.AccNumber = accNum;
            this.Amount = amount;
            this.Active = active;
            this.CreateDate = createDate;
            this.UpdateDate = updateDate;
            this.CanAddMoney = canAddMoney;
        }

        public bool CanAddMoney { get { return _canAddMoney; } set { _canAddMoney = value; } }

        public BankAccForClient PushMoneyToAcc(decimal summ)
        {
            this.Amount += summ;
            return this;
        }

        public override string ToString()
        {
            return $"ClientId: {ClientId}\n" +
                $"AccNumber: {AccNumber}\n" +
                $"Acc type: Depo\n" +
                $"Amount: {Amount}\n" +
                $"Active: {Active}\n" +
                $"Create date: {CreateDate}\n";
        }
    }
}
