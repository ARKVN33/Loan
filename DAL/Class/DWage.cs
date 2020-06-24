using System.Linq;

namespace DAL.Class
{
    public class DWage
    {
        private dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DWage()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int DLoanId { get; set; }

        public byte DWageTypeId { get; set; }

        public byte? DWageCalculationTypeId { get; set; }

        public string DPercent { get; set; }

        public long DAmount { get; set; }

        public string DDescription { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblWage = new tblWage
            {
                Wage_Loan_Id = DLoanId,
                Wage_WageType_Id = DWageTypeId,
                Wage_WageCalculationType_Id = DWageCalculationTypeId,
                WagePercent = DPercent,
                WageAmount = DAmount,
                WageDescription = DDescription
            };
            _dbLoanEntities.tblWage.Add(tblWage);
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblWage.SingleOrDefault(x => x.Wage_Loan_Id == DLoanId);
            if (result == null) return;
            _dbLoanEntities.tblWage.Remove(result);
            _dbLoanEntities.SaveChanges();
        }
        #endregion
    }
}
