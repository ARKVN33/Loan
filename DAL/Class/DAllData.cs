using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DAllData
    {
        public static Task<List<spAllData_Result>> GetAllData()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spAllData().ToList());
        }

        public static Task<List<spPeriodAllData_Result>> GetPeriodAllData(string startDate, string endDate)
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.spPeriodAllData(startDate, endDate).ToList());
        }
    }
}
