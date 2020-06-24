using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DLicense
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DLicense()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public string DAppLicense { get; set; }

        public string DAppVersion { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblLicense = new tblLicense
            {
                Id = 1,
                AppLicense = DAppLicense,
                AppVersion = DAppVersion
            };
            _dbLoanEntities.tblLicense.Add(tblLicense);
            _dbLoanEntities.SaveChanges();
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblLicense.SingleOrDefault(x => x.Id == 1);
            if (result == null)return;
            result.AppLicense = DAppLicense;
            result.AppVersion = DAppVersion;
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblLicense.SingleOrDefault(x => x.Id == 1);
            if (result == null) return;
            _dbLoanEntities.tblLicense.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<tblLicense>> GetData()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblLicense.Where(x => x.Id == 1).ToList());
        }
        #endregion
    }
}
