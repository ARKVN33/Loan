using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DLoan
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DLoan()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int DPersonnelId { get; set; }

        public int DAccountId { get; set; }

        public byte DLoanTypeId { get; set; }

        public long DAmount { get; set; }

        public string DDate { get; set; }

        public byte DInstallmentNum { get; set; }

        public string DInstallmentFirstPayDate { get; set; }

        public byte DInstallmentInterspace { get; set; }

        public bool DPayOff { get; set; }

        public string DDescription { get; set; }

        #endregion

        #region Methods

        public Task<int> Add()
        {
            var tblLoan = new tblLoan
            {
                Loan_Personnel_Id = DPersonnelId,
                Loan_LoanType_Id = DLoanTypeId,
                Loan_Account_Id = DAccountId,
                LoanAmount = DAmount,
                LoanDate = DDate,
                LoanInstallmentNum = DInstallmentNum,
                LoanInstallmentFirstPayDate = DInstallmentFirstPayDate,
                LoanInstallmentInterspace = DInstallmentInterspace,
                LoanPayOff = DPayOff,
                LoanDescription = DDescription
            };
            _dbLoanEntities.tblLoan.Add(tblLoan);
            _dbLoanEntities.SaveChanges();
            return Task.Run(() => tblLoan.Id);
        }

        public void EditPayOff()
        {
            var tblLoan = new tblLoan
            {
                Id = DId,
                LoanPayOff = DPayOff
            };
            using (var dbLoanEntities = new dbLoanEntities())
            {
                dbLoanEntities.tblLoan.Attach(tblLoan);
                dbLoanEntities.Entry(tblLoan).Property(x => x.LoanPayOff).IsModified = true;
                dbLoanEntities.SaveChanges();
            }
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblLoan.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            _dbLoanEntities.tblLoan.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<spSelectLoanInfo_Result>> GetLoanInfoData(int personnelId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spSelectLoanInfo(personnelId).ToList());
        }

        public static Task<List<tblLoan>> GetLoanData(int personnelId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblLoan.Where(x=>x.Loan_Personnel_Id==personnelId).ToList());
        }

        public static Task<List<string>> GetGuaranteeType()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblGuaranteeType.Select(x=>x.GuaranteeType).ToList());
        }

        public static Task<List<tblInstitution>> GetInstitution()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblInstitution.ToList());
        }
        #endregion
    }
}
