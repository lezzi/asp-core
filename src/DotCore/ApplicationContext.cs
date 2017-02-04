using DotCore.Portable.DataAccess.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotCore
{
    public class ApplicationContext : DotCoreContext
    {
        #region Constructors

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        #endregion
    }
}