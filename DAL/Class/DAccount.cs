using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DAccount
    {

        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DAccount()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int? DPersonnelId { get; set; }

        public byte? DPaymentTypeId { get; set; }

        public byte? DTransactionTypeId { get; set; }

        public long DAmount { get; set; }

        public string DReceiptNumber { get; set; }

        public long DCurrentBalance { get; set; }

        public string DPaymentDate { get; set; }

        public string DDescription { get; set; }

        #endregion

        #region Methods

        public Task<int> Add()
        {
            var tblAccount = new tblAccount
            {
                Account_Personnel_Id = DPersonnelId,
                Account_PaymentType_Id = DPaymentTypeId,
                Account_TransactionType_Id = DTransactionTypeId,
                AccountAmount = DAmount,
                AccountReceiptNumber = DReceiptNumber,
                AccountCurrentBalance = DCurrentBalance,
                AccountPaymentDate = DPaymentDate,
                AccountDescription = DDescription
            };
            _dbLoanEntities.tblAccount.Add(tblAccount);
            _dbLoanEntities.SaveChanges();
            var id = Task.Run(() => tblAccount.Id);
            _dbLoanEntities.spSortAccount(DPersonnelId);
            return id;
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblAccount.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            result.Account_Personnel_Id = DPersonnelId;
            result.Account_PaymentType_Id = DPaymentTypeId;
            result.Account_TransactionType_Id = DTransactionTypeId;
            result.AccountAmount = DAmount;
            result.AccountReceiptNumber = DReceiptNumber;
            result.AccountCurrentBalance = DCurrentBalance;
            result.AccountPaymentDate = DPaymentDate;
            result.AccountDescription = DDescription;
            _dbLoanEntities.SaveChanges();
            _dbLoanEntities.spSortAccount(DPersonnelId);
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblAccount.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            _dbLoanEntities.tblAccount.Remove(result);
            _dbLoanEntities.SaveChanges();
            _dbLoanEntities.spSortAccount(DPersonnelId);
        }

        public static Task<List<spSelectAccountInfo_Result>> GetData(int personnelId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spSelectAccountInfo(personnelId).ToList());
        }

        public static Task<long?> CanReceive(int personnelId, int? infoId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spPerCanGetMoney(personnelId, infoId).FirstOrDefault());
        }
        #endregion
    }
}
