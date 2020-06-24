using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Class
{
    public class DUser
    {
        private readonly dbLoanEntities _dbLoanEntities;

        #region Constructor

        public DUser()
        {
            _dbLoanEntities = new dbLoanEntities();
        }

        #endregion

        #region Properties

        public int DId { get; set; }

        public byte DPostTypeId { get; set; }

        public byte DSecurityQuestionId { get; set; }

        public string DFirstName { get; set; }

        public string DLastName { get; set; }

        public string DUserName { get; set; }

        public string DPassword { get; set; }

        public string DMobile { get; set; }

        public string DEmail { get; set; }

        public string DAnswer { get; set; }

        public string DRegistrationDate { get; set; }

        public string DImage { get; set; }

        public string DDescription { get; set; }


        #endregion

        #region Methods

        public void AddAdmin()
        {
            var tblUser = new tblUser
            {
                User_PostType_Id = DPostTypeId,
                User_SecurityQuestion_Id = DSecurityQuestionId,
                UserFirstName = DFirstName,
                UserLastName = DLastName,
                UserName = DUserName,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(DPassword),
                UserMobileNumber = DMobile,
                UserEmail = DEmail,
                UserAnswer = BCrypt.Net.BCrypt.HashPassword(DAnswer),
                UserRegistrationDate = DRegistrationDate,
                UserImage = DImage,
                UserDescription = DDescription
            };
            _dbLoanEntities.tblUser.Add(tblUser);
            _dbLoanEntities.SaveChanges();
            var tblSundry = new tblSundry
            {
                Id = 1,
                RegisteredAdminPassword = true
            };
            _dbLoanEntities.tblSundry.Add(tblSundry);
            _dbLoanEntities.SaveChanges();
        }

        public static Task<List<string>> GetQuestion()
        {
            var dbLoanEntities = new dbLoanEntities();
            return Task.Run(() => dbLoanEntities.tblSecurityQuestion.Select(x => x.SecurityQuestion).ToList());
        }

        public void ChangePassword()
        {
            var tblUser = new tblUser
            {
                UserName = DUserName,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(DPassword)
            };
            using (var dbLoanEntities = new dbLoanEntities())
            {
                dbLoanEntities.tblUser.Attach(tblUser);
                dbLoanEntities.Entry(tblUser).Property(x => x.UserPassword).IsModified = true;
                dbLoanEntities.SaveChanges();
            }
        }

        private static List<tblPostType> GetPostdata()
        {
            var dbLoanEntities = new dbLoanEntities();
            return dbLoanEntities.tblPostType.ToList();
        }

        #endregion
    }
}
