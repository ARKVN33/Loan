namespace Loan.Class
{
    public class CreateInstallment
    {
        public long DueAmount { get; set; }
        public string DueDate { get; set; }

        public CreateInstallment(long dueAmount, string dueDate)
        {
            DueAmount = dueAmount;
            DueDate = dueDate;
        }
    }
}
