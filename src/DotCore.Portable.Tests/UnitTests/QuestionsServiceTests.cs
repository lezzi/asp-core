using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotCore.Portable.BusinessLogic.ApplicationServices;
using DotCore.Portable.BusinessLogic.Models;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.Repositories;
using Moq;
using NUnit.Framework;

namespace DotCore.Portable.Tests.UnitTests
{
    [TestFixture]
    public class QuestionsServiceTests
    {
        [TestFixture]
        public class GetQuestionsAsync : QuestionsServiceTests
        {
            private readonly List<Question> _questions;
            private readonly Question _answeredQuestion;
            private readonly Question _notAnsweredQuestion;

            public GetQuestionsAsync()
            {
                var questions = new List<Question>();

                questions.Add(_answeredQuestion = new Question("Answered", "Description", "test@test.com")
                {
                    Id = 1,
                    AcceptedAnswerId = 2
                });

                questions.Add(_notAnsweredQuestion = new Question("Not Answered", "Description", "test@test.com")
                {
                    Id = 3,
                });

                _questions = questions;
            }

            [Test]
            public async Task Answered_ReturnOnlyAnswered()
            {
                var userProvider = MockedServices.UserProvider;
                
                var questionRepositoryMock = new Mock<IQuestionRepository>();

                questionRepositoryMock.Setup(x => x.GetAnsweredQuestionsAsync())
                    .ReturnsAsync(_questions.Where(x => x.AcceptedAnswerId.HasValue).ToList());
                   
                var questionsService = new QuestionsService(userProvider, questionRepositoryMock.Object,
                    MockedServices.Mapper);

                var models = await questionsService.GetQuestionsAsync(QuestionStatus.Answered);
                
                Assert.IsNotNull(models);
                Assert.AreEqual(1, models.Count);

                Assert.AreEqual(_answeredQuestion.Id, models[0].Id);
            }

            [Test]
            public async Task Active_ReturnOnlyActive()
            {
                var userProvider = MockedServices.UserProvider;

                var questionRepositoryMock = new Mock<IQuestionRepository>();

                questionRepositoryMock.Setup(x => x.GetActiveQuestionsAsync())
                    .ReturnsAsync(_questions.Where(x => !x.AcceptedAnswerId.HasValue).ToList());

                var questionsService = new QuestionsService(userProvider, questionRepositoryMock.Object,
                    MockedServices.Mapper);

                var models = await questionsService.GetQuestionsAsync(QuestionStatus.Active);

                Assert.IsNotNull(models);
                Assert.AreEqual(1, models.Count);

                Assert.AreEqual(_notAnsweredQuestion.Id, models[0].Id);
            }

            [Test]
            public async Task All_ReturnAll()
            {
                var userProvider = MockedServices.UserProvider;

                var questionRepositoryMock = new Mock<IQuestionRepository>();

                questionRepositoryMock.Setup(x => x.GetAllQuestionsAsync())
                    .ReturnsAsync(_questions.ToList());

                var questionsService = new QuestionsService(userProvider, questionRepositoryMock.Object,
                    MockedServices.Mapper);

                var models = await questionsService.GetQuestionsAsync(QuestionStatus.All);

                Assert.IsNotNull(models);
                Assert.AreEqual(2, models.Count);
            }
        }

        [TestFixture]
        public class GetQuestionDetailsAsync : QuestionsServiceTests
        {
            [Test]
            public async Task QuestionDoesNotExist_ReturnNull()
            {
                var userProvider = MockedServices.UserProvider;

                var questionRepositoryMock = new Mock<IQuestionRepository>();

                questionRepositoryMock.Setup(x => x.GetQuestionWithAnswersAsync(It.IsAny<int>()))
                    .ReturnsAsync((Question)null);

                var questionsService = new QuestionsService(userProvider, questionRepositoryMock.Object,
                    MockedServices.Mapper);

                var result = await questionsService.GetQuestionDetailsAsync(1);

                Assert.IsNull(result);
            }

            [Test]
            public async Task AcceptedAnswerExists_SetIsAccepted()
            {
                var userProvider = MockedServices.UserProvider;

                var questionRepositoryMock = new Mock<IQuestionRepository>();

                var answer = new Answer()
                {
                    Id = 1
                };
                var question = new Question()
                {
                    Id = 2,
                    User = new User(),
                    AcceptedAnswer = answer,
                    AcceptedAnswerId = answer.Id,
                    Answers = new List<Answer>() { answer }
                };

                questionRepositoryMock.Setup(x => x.GetQuestionWithAnswersAsync(It.IsAny<int>()))
                    .ReturnsAsync(question);

                var questionsService = new QuestionsService(userProvider, questionRepositoryMock.Object,
                    MockedServices.Mapper);

                var result = await questionsService.GetQuestionDetailsAsync(question.Id);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Answers.Count);
                Assert.IsTrue(result.Answers[0].IsAccepted);
            }
        }
    }
}