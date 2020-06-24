using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DGuarantor
    {
        #region Constructor

        private dbLoanEntities _dbLoanEntities;

        public DGuarantor()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties
        public int DId { get; set; }

        public int DLoanId { get; set; }

        public int DInfoId { get; set; }

        public byte DGuaranteeTypeId { get; set; }

        public byte DBlockTypeId { get; set; }

        public string DReceiptNumber { get; set; }

        public long? DAmount { get; set; }

        public long? DBlockAmount { get; set; }

        public bool DBlock { get; set; }

        public string DDescription { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            var tblGuarantor = new tblGuarantor
            {
                Guarantor_Loan_Id= DLoanId,
                Guarantor_Info_Id= DInfoId,
                Guarantor_GuaranteeType_Id= DGuaranteeTypeId,
                Guarantor_BlockType_Id= DBlockTypeId,
                GuarantorReceiptNumber= DReceiptNumber,
                GuarantorAmount= DAmount,
                GuarantorBlockAmount= DBlockAmount,
                GuarantorBlock= DBlock,
                GuarantorDescription= DDescription
            };
            _dbLoanEntities.tblGuarantor.Add(tblGuarantor);
            _dbLoanEntities.SaveChanges();
        }

        public void Delete()
        {
            _dbLoanEntities.tblGuarantor.RemoveRange(_dbLoanEntities.tblGuarantor.Where(x => x.Guarantor_Loan_Id == DLoanId));
            _dbLoanEntities.SaveChanges();
        }

        public void EditGuarantorBlock()
        {
            var tblGuarantor = new tblGuarantor
            {
                id = DId,
                GuarantorBlock = DBlock
            };
            using (var dbLoanEntities = new dbLoanEntities())
            {
                dbLoanEntities.tblGuarantor.Attach(tblGuarantor);
                dbLoanEntities.Entry(tblGuarantor).Property(x => x.GuarantorBlock).IsModified = true;
                dbLoanEntities.SaveChanges();
            }
        }

        public static Task<bool> CheckGuaBlock(int infoId)
        {
            var dbLoanEntities = new dbLoanEntities();
            var result = dbLoanEntities.tblGuarantor.FirstOrDefault(x => x.Guarantor_Info_Id == infoId && x.GuarantorBlock == true);
            return result == null ? Task.Run(() => false) : Task.Run(() => true);

        }

        public static Task<List<tblGuarantor>> GetData(int loanId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblGuarantor.Where(x => x.Guarantor_Loan_Id == loanId).ToList());
        }

        public static Task<List<tblGuarantor>> GetInfoGuaData(int? infoId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblGuarantor.Where(x => x.Guarantor_Info_Id == infoId).ToList());
            //return Task.Run(() => dbLoanEntities.spSelectInfoGuarantor(infoId).ToList());
        }

        public static Task<int?> CheckInfoIsPer(int infoId)
        {
            var dbLoanEntities = new dbLoanEntities();

            var firstOrDefault = dbLoanEntities.tblPersonnel.FirstOrDefault(x => x.Personnel_Info_Id == infoId);
            if (firstOrDefault == null) return null;
            int? result = firstOrDefault.Id;
            return Task.Run(() => result);
            //return Task.Run(() => dbLoanEntities.spChekInfoIsPer(infoId).FirstOrDefault());
        }
        #endregion
    }
}