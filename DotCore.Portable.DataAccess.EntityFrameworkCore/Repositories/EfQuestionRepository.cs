using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DotCore.Portable.DataAccess.EntityFrameworkCore.Repositories
{
    /// <inheritdoc />
    public class EfQuestionRepository : IQuestionRepository
    {
        #region Fields

        private readonly DotCoreContext _context;

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/></exception>
        public EfQuestionRepository([NotNull] DotCoreContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        #endregion

        #region IQuestionRepository

        /// <inheritdoc />
        public async Task<IList<Question>> GetActiveQuestionsAsync()
        {
            return await _context.Questions
                .Include(x => x.User)
                .Where(x => x.AcceptedAnswer == null)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IList<Question>> GetAnsweredQuestionsAsync()
        {
            return await _context.Questions
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.AcceptedAnswer != null).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IList<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<Question> GetQuestionWithAnswersAsync(int questionId)
        {
            return _context.Questions
                .Include(x => x.User)
                .Include(x => x.Answers)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == questionId);
        }

        /// <inheritdoc />
        public Task<Question> GetQuestionAsync(int questionId)
        {
            return _context.Questions
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == questionId);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="question"/> is <see langword="null"/></exception>
        public void AddQuestion([NotNull] Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            _context.Add(question);
        }

        #endregion
    }
}