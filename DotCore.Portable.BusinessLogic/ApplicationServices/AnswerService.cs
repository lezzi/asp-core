using System;
using System.Threading.Tasks;
using AutoMapper;
using DotCore.Portable.BusinessLogic.Exceptions;
using DotCore.Portable.BusinessLogic.Models;
using DotCore.Portable.DataAccess;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.Repositories;
using JetBrains.Annotations;

namespace DotCore.Portable.BusinessLogic.ApplicationServices
{
    /// <summary>
    /// Provides answer functionality.
    /// </summary>
    public class AnswerService : ApplicationService
    {
        #region Fields

        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException">One of parameters is <see langword="null" /></exception>
        public AnswerService([NotNull] IUserProvider userProvider, [NotNull] IQuestionRepository questionRepository,
            [NotNull] IAnswerRepository answerRepository, [NotNull] IUnitOfWork unitOfWork, [NotNull] IMapper mapper)
            : base(userProvider)
        {
            if (questionRepository == null)
                throw new ArgumentNullException(nameof(questionRepository));
            if (answerRepository == null)
                throw new ArgumentNullException(nameof(answerRepository));
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds new answer to the specified question.
        /// </summary>
        /// <param name="questionId">Question id.</param>
        /// <param name="newAnswer">Answer data.</param>
        /// <returns>Created <see cref="AnswerModel" /> object.</returns>
        /// <exception cref="BusinessLogicException">Question does not exist.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newAnswer" /> is <see langword="null" /></exception>
        /// <exception cref="InvalidOperationException">Logged in user is required to answer the question.</exception>
        [ItemNotNull]
        public async Task<AnswerModel> AnswerQuestionAsync(int questionId, [NotNull] NewAnswer newAnswer)
        {
            if (newAnswer == null)
                throw new ArgumentNullException(nameof(newAnswer));

            var question = await _questionRepository.GetQuestionAsync(questionId).ConfigureAwait(false);
            if (question == null)
                throw new BusinessLogicException($"Question {questionId} does not exist.");

            var currentUserId = await UserProvider.GetCurrentUserAsync().ConfigureAwait(false);
            if (currentUserId == null)
                throw new InvalidOperationException("Logged in user is required to answer the question.");

            var answer = new Answer(question, newAnswer.Description, currentUserId);

            _answerRepository.AddAnswer(answer);

            await _unitOfWork.CommitAsync().ConfigureAwait(false);

            return _mapper.Map<AnswerModel>(answer);
        }

        /// <summary>
        /// Accepts specified answer.
        /// </summary>
        /// <param name="answerId">Answer id.</param>
        /// <returns>Accepted <see cref="AnswerModel" /> object.</returns>
        /// <exception cref="AccessDeniedException">User is not allowed to perform this operation.</exception>
        [ItemNotNull]
        public async Task<AnswerModel> AcceptAnswerAsync(int answerId)
        {
            var answer = await _answerRepository.GetAnswerAsync(answerId).ConfigureAwait(false);
            if (answer.Question.UserId != UserProvider.CurrentUserId)
                throw new AccessDeniedException("User is not allowed to perform this operation.");

            answer.Question.AcceptAnswer(answer);

            await _unitOfWork.CommitAsync().ConfigureAwait(false);

            var model = _mapper.Map<AnswerModel>(answer);
            model.IsAccepted = true;

            return model;
        }

        #endregion
    }
}