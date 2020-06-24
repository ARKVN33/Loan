using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DChMoPay
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DChMoPay()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int DChargeMonthlyId { get; set; }

        public int DAccountId { get; set; }

        public long DDueAmount { get; set; }

        public string DDueDate { get; set; }

        public short DDelayMonth { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblChMoPay = new tblChMoPay
            {
                ChMoPay_ChargeMonthly_Id = DChargeMonthlyId,
                ChMoPay_Account_Id = DAccountId,
                ChMoPayDueAmount = DDueAmount,
                ChMoPayDueDate = DDueDate,
                ChMoPayDelayMonth = DDelayMonth
            };
            _dbLoanEntities.tblChMoPay.Add(tblChMoPay);
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblChMoPay.SingleOrDefault(x => x.ChMoPay_Account_Id == DAccountId);
            if (result == null) return;
            _dbLoanEntities.tblChMoPay.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<spSelectChMoPayInfo_Result>> GetData(int personnelId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spSelectChMoPayInfo(personnelId).ToList());
        }

        public static Task<List<tblChMoPay>> GetChMoPayData(int chMoId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblChMoPay.Where(x => x.ChMoPay_ChargeMonthly_Id == chMoId).ToList());
        }
        #endregion
    }
}
