using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DPersonnel
    {

        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DPersonnel()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        public DPersonnel(int cId, int cInfoId, string cPersonnelId, string cPersonnelMembership,
            string cPersonnelMembershipDate,
            string cPersonnelBarCode, string cPersonnelQrCode, string cPersonnelSignature)
        {
            _dbLoanEntities = new dbLoanEntities();

            DId = cId;
            DInfoId = cInfoId;
            DPersonnelId = cPersonnelId;
            DPersonnelMembership = cPersonnelMembership;
            DPersonnelMembershipDate = cPersonnelMembershipDate;
            DPersonnelBarCode = cPersonnelBarCode;
            DPersonnelQrCode = cPersonnelQrCode;
            DPersonnelSignature = cPersonnelSignature;
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int DInfoId { get; set; }

        public string DPersonnelId { get; set; }

        public string DPersonnelMembership { get; set; }

        public string DPersonnelMembershipDate { get; set; }

        public string DPersonnelBarCode { get; set; }

        public string DPersonnelQrCode { get; set; }

        public string DPersonnelSignature { get; set; }


        #endregion

        #region Methods

        public Task<int> Add()
        {
            var tblPersonnel = new tblPersonnel
            {
                Personnel_Info_Id = DInfoId,
                PersonnelId = DPersonnelId,
                PersonnelMembership = DPersonnelMembership,
                PersonnelMembershipDate = DPersonnelMembershipDate,
                PersonnelBARCode = DPersonnelBarCode,
                PersonnelQRCode = DPersonnelQrCode,
                PersonnelSignature = DPersonnelSignature
            };
            _dbLoanEntities.tblPersonnel.Add(tblPersonnel);
            _dbLoanEntities.SaveChanges();
            return
                Task.Run(() => tblPersonnel.Id);
        }

        public void Edit()
        {
            var result = _dbLoanEntities.tblPersonnel.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            result.PersonnelId = DPersonnelId;
            result.PersonnelMembership = DPersonnelMembership;
            result.PersonnelMembershipDate = DPersonnelMembershipDate;
            result.PersonnelBARCode = DPersonnelBarCode;
            result.PersonnelQRCode = DPersonnelQrCode;
            result.PersonnelSignature = DPersonnelSignature;
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblPersonnel.SingleOrDefault(x => x.Id == DId);
            if (result == null) return;
            _dbLoanEntities.tblPersonnel.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<bool> CheckPersonnelId(string personnelId)
        {
            var dbLoanEntities = new dbLoanEntities();
            var result = dbLoanEntities.tblPersonnel.SingleOrDefault(x => x.PersonnelId == personnelId);
            return result == null ? Task.Run(() => true) : Task.Run(() => false);
        }

        public static Task<List<spSelectPersonnelInfo_Result>> GetData()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spSelectPersonnelInfo().ToList());
        }

        public static Task<string> GetPersonnelId()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spAutoPersonnelId().FirstOrDefault().ToString());
        }

        public static Task<string> GetAccountId()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spAutoAccountId().FirstOrDefault().ToString());
        }

        #endregion
    }
}
