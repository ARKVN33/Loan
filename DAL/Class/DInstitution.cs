using System.Threading.Tasks;

namespace DAL.Class
{
    public class DInstitution
    {
        private readonly dbLoanEntities _dbLoanEntities;

        public DInstitution()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        public string DInstName { get; set; }

        public Task<int> Add()
        {
            var tblInstitution = new tblInstitution
            {
                Institution = DInstName
            };
            
            _dbLoanEntities.tblInstitution.Add(tblInstitution);
            _dbLoanEntities.SaveChanges();
            return Task.Run(() => tblInstitution.id);
        }
    }
}
