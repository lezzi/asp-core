using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DotCore.Portable.DataAccess.Entities
{
    public class User : IdentityUser
    {
        #region Constructors

        public User()
        {
        }

        public User(string userName) : base(userName)
        {
        }

        #endregion
    }
}