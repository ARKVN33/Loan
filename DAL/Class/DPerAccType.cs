using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DPerAccType
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DPerAccType()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int DPersonnelId { get; set; }

        public int DAccountTypeId { get; set; }

        public string DAccountNumber { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblPerAccType = new tblPerAccType
            {
                PerAccType_Personnel_Id = DPersonnelId,
                PerAccType_AccountType_Id = DAccountTypeId,
                PerAccTypeAccountNumber = DAccountNumber
            };
            _dbLoanEntities.tblPerAccType.Add(tblPerAccType);
            _dbLoanEntities.SaveChanges();
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblPerAccType.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            result.PerAccType_Personnel_Id = DPersonnelId;
            result.PerAccType_AccountType_Id = DAccountTypeId;
            result.PerAccTypeAccountNumber = DAccountNumber;
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblPerAccType.SingleOrDefault(x => x.PerAccType_Personnel_Id == DPersonnelId);
            if (result == null) return;
            _dbLoanEntities.tblPerAccType.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<tblPerAccType>> GetData(int personnelId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblPerAccType.Where(x=>x.PerAccType_Personnel_Id==personnelId).ToList());
        }

        public static Task<bool> CheckAccountNumber(string accountNumber)
        {
            var dbLoanEntities = new dbLoanEntities();
            var result = dbLoanEntities.tblPerAccType.SingleOrDefault(x => x.PerAccTypeAccountNumber == accountNumber);
            return result == null ? Task.Run(() => true) : Task.Run(() => false);
        }

        #endregion
    }
}
