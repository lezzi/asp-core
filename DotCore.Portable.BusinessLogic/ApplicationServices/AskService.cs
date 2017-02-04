using System;
using System.Threading.Tasks;
using AutoMapper;
using DotCore.Portable.BusinessLogic.Models;
using DotCore.Portable.DataAccess;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.Repositories;
using JetBrains.Annotations;

namespace DotCore.Portable.BusinessLogic.ApplicationServices
{
    /// <summary>
    /// Provides ask question functionality.
    /// </summary>
    public class AskService : ApplicationService
    {
        #region Fields

        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException">One of parameters is <see langword="null" /></exception>
        public AskService([NotNull] IUserProvider userProvider, [NotNull] IQuestionRepository questionRepository,
            [NotNull] IUnitOfWork unitOfWork, [NotNull] IMapper mapper) : base(userProvider)
        {
            if (questionRepository == null)
                throw new ArgumentNullException(nameof(questionRepository));
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds new question.
        /// </summary>
        /// <param name="askQuestionModel">Question data.</param>
        /// <returns>Created <see cref="QuestionModel" /> object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="askQuestionModel" /> is <see langword="null" /></exception>
        [ItemNotNull]
        public async Task<QuestionModel> AskQuestionAsync([NotNull] AskQuestionModel askQuestionModel)
        {
            if (askQuestionModel == null)
                throw new ArgumentNullException(nameof(askQuestionModel));

            var question = new Question(askQuestionModel.Title, askQuestionModel.Description, UserProvider.CurrentUserId);

            _questionRepository.AddQuestion(question);

            await _unitOfWork.CommitAsync().ConfigureAwait(false);

            return _mapper.Map<QuestionModel>(question);
        }

        #endregion
    }
}