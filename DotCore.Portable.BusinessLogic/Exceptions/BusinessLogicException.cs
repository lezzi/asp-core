using System;

namespace DotCore.Portable.BusinessLogic.Exceptions
{
    /// <summary>
    /// Represents a basic class for all business logic exceptions.
    /// </summary>
    public class BusinessLogicException : Exception
    {
        #region Constructors

        /// <inheritdoc />
        public BusinessLogicException()
        {
        }

        /// <inheritdoc />
        public BusinessLogicException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public BusinessLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}