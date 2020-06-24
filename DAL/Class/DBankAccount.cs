using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DBankAccount
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DBankAccount()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public string DBankName { get; set; }

        public string DBranchName { get; set; }

        public string DAccountNum { get; set; }

        public string DCardNum { get; set; }

        public long DInitialBalance { get; set; }

        public string DDescription { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblBankAccount = new tblBankAccount()
            {
                BankAccountBankName= DBankName,
                BankAccountBranchName= DBranchName,
                BankAccountNum= DAccountNum,
                BankAccountCardNum= DCardNum,
                BankAccountInitialBalance = DInitialBalance,
                BankAccountDescription= DDescription
            };
            _dbLoanEntities.tblBankAccount.Add(tblBankAccount);
            _dbLoanEntities.SaveChanges();
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblBankAccount.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            result.BankAccountBankName=DBankName;
            result.BankAccountBranchName=DBranchName;
            result.BankAccountNum=DAccountNum;
            result.BankAccountCardNum=DCardNum;
            result.BankAccountInitialBalance=DInitialBalance;
            result.BankAccountDescription=DDescription;
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblBankAccount.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            _dbLoanEntities.tblBankAccount.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<tblBankAccount>> GetData()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblBankAccount.ToList());
        }

        #endregion
    }
}
