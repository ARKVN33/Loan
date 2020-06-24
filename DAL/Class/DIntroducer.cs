using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DIntroducer
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DIntroducer()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        public DIntroducer(int cId, int cLoanId, byte cIntroducerTypeId, int? cInfoId, short? cInstitutionId,
            string cDescription)
        {
            _dbLoanEntities = new dbLoanEntities();

            DId = cId;
            DLoanId = cLoanId;
            DIntroducerTypeId = cIntroducerTypeId;
            DInfoId = cInfoId;
            DInstitutionId = cInstitutionId;
            DDescription = cDescription;
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int DLoanId { get; set; }

        public byte DIntroducerTypeId { get; set; }

        public int? DInfoId { get; set; }

        public short? DInstitutionId { get; set; }

        public string DDescription { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblIntroducer = new tblIntroducer
            {
                Introducer_Loan_Id= DLoanId,
                Introducer_IntroducerType_Id= DIntroducerTypeId,
                Introducer_Info_Id= DInfoId,
                Introducer_Institution_Id= DInstitutionId,
                IntroducerDescription= DDescription
            };
            _dbLoanEntities.tblIntroducer.Add(tblIntroducer);
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            var result = _dbLoanEntities.tblIntroducer.SingleOrDefault(x => x.Introducer_Loan_Id == DLoanId);
            if (result == null) return;
            _dbLoanEntities.tblIntroducer.Remove(result);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<tblIntroducer>> GetInfoIntroData(int? infoId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblIntroducer.Where(x => x.Introducer_Info_Id == infoId).ToList());
        }
        #endregion
    }
}