using System;
using JetBrains.Annotations;

namespace DotCore.Portable.BusinessLogic.ApplicationServices
{
    /// <summary>
    /// Basic class for all application services.
    /// </summary>
    public abstract class ApplicationService
    {
        #region Properties, Indexers

        /// <summary>
        /// <see cref="UserProvider" /> instance.
        /// </summary>
        protected IUserProvider UserProvider { get; }

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException"><paramref name="userProvider" /> is <see langword="null" /></exception>
        protected ApplicationService([NotNull] IUserProvider userProvider)
        {
            if (userProvider == null)
                throw new ArgumentNullException(nameof(userProvider));

            UserProvider = userProvider;
        }

        #endregion
    }
}