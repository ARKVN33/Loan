using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DFeeIncome
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DFeeIncome()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public byte DFeeIncomeTypeId { get; set; }

        public byte DPaymentTypeId { get; set; }

        public string DDate { get; set; }

        public long DAmount { get; set; }

        public string DReceiptNumber { get; set; }

        public string DDescription { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblFeeIncome = new tblFeeIncome
            {
                FeeIncome_FeeIncomeType_Id = DFeeIncomeTypeId,
                FeeIncome_PaymentType_Id = DPaymentTypeId,
                FeeIncomeDate = DDate,
                FeeIncomeAmount = DAmount,
                FeeIncomeReceiptNumber = DReceiptNumber,
                FeeIncomeDescription = DDescription
            };
            _dbLoanEntities.tblFeeIncome.Add(tblFeeIncome);
            _dbLoanEntities.SaveChanges();
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblFeeIncome.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            result.FeeIncome_FeeIncomeType_Id = DFeeIncomeTypeId;
            result.FeeIncome_PaymentType_Id = DPaymentTypeId;
            result.FeeIncomeDate = DDate;
            result.FeeIncomeAmount = DAmount;
            result.FeeIncomeReceiptNumber = DReceiptNumber;
            result.FeeIncomeDescription = DDescription;
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblFeeIncome.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            _dbLoanEntities.tblFeeIncome.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<spSelectFeeIncomeInfo_Result>> GetData()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spSelectFeeIncomeInfo().ToList());
        }

        #endregion
    }
}
