using System.Threading.Tasks;
using DotCore.Portable.DataAccess.Entities;
using JetBrains.Annotations;

namespace DotCore.Portable.BusinessLogic
{
    /// <summary>
    /// Provides access to current logged-in user.
    /// </summary>
    public interface IUserProvider
    {
        /// <summary>
        /// Current user <see cref="User.Id"/>.
        /// </summary>
        [CanBeNull]
        string CurrentUserId { get; }

        /// <summary>
        /// Loads current <see cref="User"/> entity.
        /// </summary>
        /// <returns>Current <see cref="User"/> entity.</returns>
        [ItemCanBeNull]
        Task<User> GetCurrentUserAsync();
    }
}