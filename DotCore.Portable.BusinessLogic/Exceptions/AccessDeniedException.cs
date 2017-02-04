using System;

namespace DotCore.Portable.BusinessLogic.Exceptions
{
    /// <summary>
    /// Represents resource access denied exception.
    /// </summary>
    public class AccessDeniedException : BusinessLogicException
    {
        #region Constructors

        /// <inheritdoc />
        public AccessDeniedException()
        {
        }

        /// <inheritdoc />
        public AccessDeniedException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public AccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}