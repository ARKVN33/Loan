using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DLoanFund
    {
        private readonly dbLoanEntities _dbLoanEntities;

        public DLoanFund()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #region Properties

        public int DId { get; set; }

        public string DName { get; set; }

        public string DTell1 { get; set; }

        public string DTell2 { get; set; }

        public string DFax { get; set; }

        public string DEmail { get; set; }

        public string DPostalCode { get; set; }

        public string DAddress { get; set; }

        public long? DInitialBalance { get; set; }

        public long? DPenalty { get; set; }

        public string DWagePercent { get; set; }

        public string DLogo { get; set; }
        #endregion

        #region Methods

        public void Add()
        {
            var tblLoanFund = new tblLoanFund
            {
                LoanFundName = DName,
                LoanFundTell1 = DTell1,
                LoanFundTell2 = DTell2,
                LoanFundFax = DFax,
                LoanFundEmail = DEmail,
                LoanFundPostalCode = DPostalCode,
                LoanFundAddress = DAddress,
                LoanFundInitialBalance = DInitialBalance,
                LoanFundPenalty = DPenalty,
                LoanFundWagePercent = DWagePercent,
                LoanFundLogo = DLogo
            };
            _dbLoanEntities.tblLoanFund.Add(tblLoanFund);
            _dbLoanEntities.SaveChanges();
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblLoanFund.SingleOrDefault(x => x.Id == 1);
            if (result == null) return;
            result.LoanFundName = DName;
            result.LoanFundTell1 = DTell1;
            result.LoanFundTell2 = DTell2;
            result.LoanFundFax = DFax;
            result.LoanFundEmail = DEmail;
            result.LoanFundPostalCode = DPostalCode;
            result.LoanFundAddress = DAddress;
            result.LoanFundInitialBalance = DInitialBalance;
            result.LoanFundPenalty = DPenalty;
            result.LoanFundWagePercent = DWagePercent;
            result.LoanFundLogo = DLogo;
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<tblLoanFund>> GetData()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblLoanFund.ToList());
        }
        #endregion
    }
}