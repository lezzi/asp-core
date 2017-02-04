using System.Threading.Tasks;

namespace DotCore.Portable.DataAccess
{
    /// <summary>
    /// Unit of work instance.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Applies all changes that were made in repositories.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();
    }
}