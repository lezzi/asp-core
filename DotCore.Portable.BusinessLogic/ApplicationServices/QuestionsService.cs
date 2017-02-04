using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotCore.Portable.BusinessLogic.Models;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.Repositories;
using JetBrains.Annotations;

namespace DotCore.Portable.BusinessLogic.ApplicationServices
{
    /// <summary>
    /// Provides get questions functionality.
    /// </summary>
    public class QuestionsService : ApplicationService
    {
        #region Fields

        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException">One of parameters is <see langword="null" /></exception>
        public QuestionsService([NotNull] IUserProvider userProvider, [NotNull] IQuestionRepository questionRepository,
            [NotNull] IMapper mapper)
            : base(userProvider)
        {
            if (questionRepository == null)
                throw new ArgumentNullException(nameof(questionRepository));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads questions that are filtered by <see cref="QuestionStatus"/> type.
        /// </summary>
        /// <param name="questionStatus">Filter <see cref="QuestionStatus"/> value.</param>
        /// <returns>Questions that are filtered by <see cref="QuestionStatus"/> type.</returns>
        [ItemNotNull]
        public async Task<IList<QuestionModel>> GetQuestionsAsync(QuestionStatus questionStatus)
        {
            IList<Question> questions;

            switch (questionStatus)
            {
                case QuestionStatus.Active:
                    questions = await _questionRepository.GetActiveQuestionsAsync().ConfigureAwait(false);
                    break;
                case QuestionStatus.Answered:
                    questions = await _questionRepository.GetAnsweredQuestionsAsync().ConfigureAwait(false);
                    break;
                case QuestionStatus.All:
                    questions = await _questionRepository.GetAllQuestionsAsync().ConfigureAwait(false);
                    break;
                default:
                    questions = new List<Question>();
                    break;
            }

            return _mapper.Map<IList<QuestionModel>>(questions);
        }

        /// <summary>
        /// Loads question with all answers.
        /// </summary>
        /// <param name="questionId">Question id.</param>
        /// <returns>Specified <see cref="QuestionDetailsModel"/> object.</returns>
        [ItemCanBeNull]
        public async Task<QuestionDetailsModel> GetQuestionDetailsAsync(int questionId)
        {
            var question = await _questionRepository.GetQuestionWithAnswersAsync(questionId).ConfigureAwait(false);

            return question == null ? null : _mapper.Map<QuestionDetailsModel>(question);
        }

        #endregion
    }
}