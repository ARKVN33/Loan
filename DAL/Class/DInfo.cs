using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DInfo
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DInfo()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public string DInfoFirstName { get; set; }

        public string DInfoLastName { get; set; }

        public string DInfoFatherName { get; set; }

        public string DInfoGender { get; set; }

        public string DInfoNationalCode { get; set; }

        public string DInfoCode { get; set; }

        public string DInfoBirthDay { get; set; }

        public string DInfoBirthPlace { get; set; }

        public string DInfoMarried { get; set; }

        public string DInfoTell { get; set; }

        public string DInfoMobile { get; set; }

        public string DInfoEmail { get; set; }

        public string DInfoPostalCode { get; set; }

        public string DInfoAddress { get; set; }

        public string DInfoJobName { get; set; }

        public string DInfoJobPlaceName { get; set; }

        public string DInfoJobTell { get; set; }

        public string DInfoJobFax { get; set; }

        public string DInfoJobAddress { get; set; }

        public string DInfoImage { get; set; }

        public string DInfoDescription { get; set; }
        public int? InfoId { get; set; }

        #endregion

        #region Methods

        public Task<int> Add()
        {
            tblInfo tblInfo = new tblInfo
            {
                InfoFirstName= DInfoFirstName,
                InfoLastName= DInfoLastName,
                InfoFatherName= DInfoFatherName,
                InfoNationalCode= DInfoNationalCode,
                InfoCode= DInfoCode,
                InfoGender= DInfoGender,
                InfoBirthDay= DInfoBirthDay,
                InfoBirthPlace= DInfoBirthPlace,
                InfoMarried= DInfoMarried,
                InfoTell= DInfoTell,
                InfoMobile= DInfoMobile,
                InfoEmail= DInfoEmail,
                InfoPostalCode= DInfoPostalCode,
                InfoAddress= DInfoAddress,
                InfoJobName= DInfoJobName,
                InfoJobPlaceName= DInfoJobPlaceName,
                InfoJobTell= DInfoJobTell,
                InfoJobFax= DInfoJobFax,
                InfoJobAddress= DInfoJobAddress,
                InfoImage= DInfoImage,
                InfoDescription= DInfoDescription
            };
            _dbLoanEntities.tblInfo.Add(tblInfo);
            _dbLoanEntities.SaveChanges();
            return
                Task.Run(() => tblInfo.Id);
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblInfo.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            result.InfoFirstName = DInfoFirstName;
            result.InfoLastName = DInfoLastName;
            result.InfoFatherName = DInfoFatherName;
            result.InfoNationalCode = DInfoNationalCode;
            result.InfoCode = DInfoCode;
            result.InfoGender = DInfoGender;
            result.InfoBirthDay = DInfoBirthDay;
            result.InfoBirthPlace = DInfoBirthPlace;
            result.InfoMarried = DInfoMarried;
            result.InfoTell = DInfoTell;
            result.InfoMobile = DInfoMobile;
            result.InfoEmail = DInfoEmail;
            result.InfoPostalCode = DInfoPostalCode;
            result.InfoAddress = DInfoAddress;
            result.InfoJobName = DInfoJobName;
            result.InfoJobPlaceName = DInfoJobPlaceName;
            result.InfoJobTell = DInfoJobTell;
            result.InfoJobFax = DInfoJobFax;
            result.InfoJobAddress = DInfoJobAddress;
            result.InfoImage = DInfoImage;
            result.InfoDescription = DInfoDescription;
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblInfo.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            _dbLoanEntities.tblInfo.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<tblInfo>> CheckNationalCode(string nationalCode)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblInfo.Where(x => x.InfoNationalCode == nationalCode).ToList());
        }

        public static Task<List<tblInfo>> GetData()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblInfo.ToList());
        }

        #endregion
    }
}
