using System;
using System.Threading.Tasks;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DotCore.Portable.DataAccess.EntityFrameworkCore.Repositories
{
    /// <inheritdoc />
    public class EfAnswerRepository : IAnswerRepository
    {
        #region Fields

        private readonly DotCoreContext _context;

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" /></exception>
        public EfAnswerRepository([NotNull] DotCoreContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        #endregion

        #region IAnswerRepository

        /// <inheritdoc />
        public Task<Answer> GetAnswerAsync(int answerId)
        {
            return _context.Answers
                .Include(x => x.User)
                .Include(x => x.Question.User)
                .FirstOrDefaultAsync(x => x.Id == answerId);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="answer"/> is <see langword="null"/></exception>
        public void AddAnswer([NotNull] Answer answer)
        {
            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            _context.Answers.Add(answer);
        }

        #endregion
    }
}