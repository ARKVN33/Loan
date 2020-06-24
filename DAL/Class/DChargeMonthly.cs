using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DChargeMonthly
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DChargeMonthly()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int DPersonnelId { get; set; }

        public string DChargeMonthlyStartDate { get; set; }

        public string DChargeMonthlyEndDate { get; set; }

        public long DChargeMonthlyCharge { get; set; }

        public string DChargeMonthlyDescription { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblChargeMonthly = new tblChargeMonthly
            {
                ChargeMonthly_Personnel_Id= DPersonnelId,
                ChargeMonthlyStartDate= DChargeMonthlyStartDate,
                ChargeMonthlyEndDate= DChargeMonthlyEndDate,
                ChargeMonthlyCharge = DChargeMonthlyCharge,
                ChargeMonthlyDescription= DChargeMonthlyDescription
            };
            _dbLoanEntities.tblChargeMonthly.Add(tblChargeMonthly);
            _dbLoanEntities.SaveChanges();
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblChargeMonthly.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            result.ChargeMonthly_Personnel_Id = DPersonnelId;
            result.ChargeMonthlyStartDate = DChargeMonthlyStartDate;
            result.ChargeMonthlyEndDate = DChargeMonthlyEndDate;
            result.ChargeMonthlyCharge = DChargeMonthlyCharge;
            result.ChargeMonthlyDescription = DChargeMonthlyDescription;
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblChargeMonthly.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            _dbLoanEntities.tblChargeMonthly.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<tblChargeMonthly>> GetData(int personnelId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblChargeMonthly
                .Where(x => x.ChargeMonthly_Personnel_Id == personnelId).ToList());
        }

        #endregion
    }
}
