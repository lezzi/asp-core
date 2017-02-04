using System;
using System.Threading.Tasks;
using DotCore.Portable.BusinessLogic;
using DotCore.Portable.DataAccess.Entities;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DotCore.Services
{
    /// <inheritdoc />
    public class UserProvider : IUserProvider
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException"><paramref name="httpContextAccessor"/> or <paramref name="userManager"/> is <see langword="null"/></exception>
        public UserProvider([NotNull] IHttpContextAccessor httpContextAccessor, [NotNull] UserManager<User> userManager)
        {
            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        #endregion

        #region IUserProvider

        /// <inheritdoc />
        public string CurrentUserId => _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

        /// <inheritdoc />
        public Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }

        #endregion
    }
}