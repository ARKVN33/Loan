using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DInstallment
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DInstallment()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public int DLoanId { get; set; }

        public byte? DPaymentTypeId { get; set; }

        public long DDueAmount { get; set; }

        public long? DAmount { get; set; }

        public string DReceiptNumber { get; set; }

        public long? DTotalPaid { get; set; }

        public long? DRemaining { get; set; }

        public string DPaymentDate { get; set; }

        public string DDueDate { get; set; }

        public short? DDelayDay { get; set; }

        public string DDescription { get; set; }

        #endregion

        #region Methods

        public void Add()
        {
            tblInstallment tblInstallment = new tblInstallment
            {
                Installment_Loan_Id = DLoanId,
                Installment_PaymentType_Id = DPaymentTypeId,
                InstallmentDueAmount = DDueAmount,
                InstallmentAmount = DAmount,
                InstallmentReceiptNumber = DReceiptNumber,
                InstallmentTotalPaid = DTotalPaid,
                InstallmentRemaining = DRemaining,
                InstallmentPaymentDate = DPaymentDate,
                InstallmentDueDate = DDueDate,
                InstallmentDelayDay = DDelayDay,
                InstallmentDescription = DDescription
            };
            _dbLoanEntities.tblInstallment.Add(tblInstallment);
            _dbLoanEntities.SaveChanges();
        }

        public void AddPayment()
        {
            var tblInstallment = new tblInstallment
            {
                Id = DId,
                Installment_PaymentType_Id = DPaymentTypeId,
                InstallmentAmount = DAmount,
                InstallmentReceiptNumber = DReceiptNumber,
                InstallmentTotalPaid = DTotalPaid,
                InstallmentRemaining = DRemaining,
                InstallmentPaymentDate = DPaymentDate,
                InstallmentDelayDay = DDelayDay,
                InstallmentDescription = DDescription
            };
            using (var dbLoanEntities = new dbLoanEntities())
            {
                dbLoanEntities.tblInstallment.Attach(tblInstallment);
                dbLoanEntities.Entry(tblInstallment).Property(x => x.Installment_PaymentType_Id).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentAmount).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentReceiptNumber).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentTotalPaid).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentRemaining).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentPaymentDate).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentDelayDay).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentDescription).IsModified = true;
                dbLoanEntities.SaveChanges();
            }
        }

        public void Edit()
        {
            var tblInstallment = new tblInstallment
            {
                Id = DId,
                Installment_PaymentType_Id = DPaymentTypeId,
                InstallmentReceiptNumber = DReceiptNumber,
                InstallmentPaymentDate = DPaymentDate,
                InstallmentDelayDay = DDelayDay,
                InstallmentDescription = DDescription
            };
            using (var dbLoanEntities = new dbLoanEntities())
            {
                dbLoanEntities.tblInstallment.Attach(tblInstallment);
                dbLoanEntities.Entry(tblInstallment).Property(x => x.Installment_PaymentType_Id).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentReceiptNumber).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentPaymentDate).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentDelayDay).IsModified = true;
                dbLoanEntities.Entry(tblInstallment).Property(x => x.InstallmentDescription).IsModified = true;
                dbLoanEntities.SaveChanges();
            }
        }


        public void Delete()
        {
            _dbLoanEntities.tblInstallment.RemoveRange(_dbLoanEntities.tblInstallment.Where(x => x.Installment_Loan_Id == DLoanId));
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<tblInstallment>> GetData(int loanId)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblInstallment.Where(x => x.Installment_Loan_Id == loanId).ToList());
        }

        #endregion
    }
}

