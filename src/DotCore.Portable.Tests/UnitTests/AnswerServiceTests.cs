using System;
using System.Threading.Tasks;
using DotCore.Portable.BusinessLogic;
using DotCore.Portable.BusinessLogic.ApplicationServices;
using DotCore.Portable.BusinessLogic.Exceptions;
using DotCore.Portable.BusinessLogic.Models;
using DotCore.Portable.DataAccess;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.Repositories;
using Moq;
using NUnit.Framework;

namespace DotCore.Portable.Tests.UnitTests
{
    [TestFixture]
    public class AnswerServiceTests
    {
        [TestFixture]
        public class AnswerQuestionAsync : AnswerServiceTests
        {
            [Test]
            public void NullAnswer_ThrowArgumentNullException()
            {
                var answerService = new AnswerService(Mock.Of<IUserProvider>(), Mock.Of<IQuestionRepository>(), Mock.Of<IAnswerRepository>(),
                    Mock.Of<IUnitOfWork>(), MockedServices.Mapper);

                Assert.ThrowsAsync<ArgumentNullException>(()=> answerService.AnswerQuestionAsync(1, null));
            }

            [Test]
            public async Task AddValidObject()
            {
                const int questionId = 10;
                var newAnswer = new NewAnswer()
                {
                    Description = "Test"
                };

                var userProvider = MockedServices.UserProvider;

                var answerRepositoryMock = new Mock<IAnswerRepository>();
                var questionRepositoryMock = new Mock<IQuestionRepository>();
                var unitOfWorkMock = new Mock<IUnitOfWork>();

                Answer addedAnswer = null;
                Answer savedAnswer = null;

                answerRepositoryMock.Setup(x => x.AddAnswer(It.IsAny<Answer>()))
                    .Callback<Answer>(x => addedAnswer = x);
                questionRepositoryMock.Setup(x => x.GetQuestionAsync(It.IsAny<int>()))
                    .ReturnsAsync(new Question() { Id = questionId });

                unitOfWorkMock.Setup(x => x.CommitAsync())
                   .Callback(() => savedAnswer = addedAnswer)
                   .Returns(() => Task.FromResult((object)null));


                var answerService = new AnswerService(userProvider, questionRepositoryMock.Object, answerRepositoryMock.Object,
                    unitOfWorkMock.Object, MockedServices.Mapper);

                var result = await answerService.AnswerQuestionAsync(questionId, newAnswer);

                Assert.IsNotNull(savedAnswer);
                Assert.AreEqual(userProvider.CurrentUserId, savedAnswer.UserId);
                Assert.AreEqual(newAnswer.Description, savedAnswer.Description);
                Assert.AreEqual(questionId, savedAnswer.QuestionId);
                Assert.AreNotEqual(default(DateTime), savedAnswer.CreatedOn);

                Assert.AreEqual(false, result.IsAccepted);
            }
        }

        [Test]
        public async Task AnsweredQuestion_IsResultQuestionAnswered()
        {
            const int questionId = 10;
            var newAnswer = new NewAnswer()
            {
                Description = "Test"
            };

            var userProvider = MockedServices.UserProvider;

            var answerRepositoryMock = new Mock<IAnswerRepository>();
            var questionRepositoryMock = new Mock<IQuestionRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            Answer addedAnswer = null;
            Answer savedAnswer = null;

            answerRepositoryMock.Setup(x => x.AddAnswer(It.IsAny<Answer>()))
                .Callback<Answer>(x => addedAnswer = x);
            questionRepositoryMock.Setup(x => x.GetQuestionAsync(It.IsAny<int>()))
                .ReturnsAsync(new Question() { Id = questionId, AcceptedAnswerId = 2 });

            unitOfWorkMock.Setup(x => x.CommitAsync())
               .Callback(() => savedAnswer = addedAnswer)
               .Returns(() => Task.FromResult((object)null));


            var answerService = new AnswerService(userProvider, questionRepositoryMock.Object, answerRepositoryMock.Object,
                unitOfWorkMock.Object, MockedServices.Mapper);

            var result = await answerService.AnswerQuestionAsync(questionId, newAnswer);

            Assert.IsNotNull(savedAnswer);
            Assert.IsNotNull(result.Question);
            Assert.IsTrue(result.Question.IsAnswered);
        }

        [Test]
        public void MissingQuestion_Throw()
        {
            const int questionId = 10;
            var newAnswer = new NewAnswer()
            {
                Description = "Test"
            };

            var userProvider = MockedServices.UserProvider;

            var answerRepositoryMock = new Mock<IAnswerRepository>();
            var questionRepositoryMock = new Mock<IQuestionRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            Answer addedAnswer = null;
            Answer savedAnswer = null;

            answerRepositoryMock.Setup(x => x.AddAnswer(It.IsAny<Answer>()))
                .Callback<Answer>(x => addedAnswer = x);

            unitOfWorkMock.Setup(x => x.CommitAsync())
               .Callback(() => savedAnswer = addedAnswer)
               .Returns(() => Task.FromResult((object)null));


            var answerService = new AnswerService(userProvider, questionRepositoryMock.Object, answerRepositoryMock.Object,
                unitOfWorkMock.Object, MockedServices.Mapper);

            Assert.ThrowsAsync<BusinessLogicException>(() => answerService.AnswerQuestionAsync(questionId, newAnswer));
        }
    }

    [TestFixture]
    public class AcceptAnswerAsync : AnswerServiceTests
    {
        [Test]
        public async Task SetIsAccepted()
        {
            var userProvider = MockedServices.UserProvider;

            var answerRepositoryMock = new Mock<IAnswerRepository>();
            var questionRepositoryMock = new Mock<IQuestionRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var answer = new Answer()
            {
                Id = 1,
                Question = new Question()
                {
                    Id = 2,
                    User = new User() { Id = userProvider.CurrentUserId },
                    UserId = userProvider.CurrentUserId
                }
            };
            Answer savedAnswer = null;

            answerRepositoryMock.Setup(x => x.GetAnswerAsync(It.IsAny<int>()))
             .ReturnsAsync(answer);
            questionRepositoryMock.Setup(x => x.GetQuestionAsync(It.IsAny<int>()))
                .ReturnsAsync(answer.Question);

            unitOfWorkMock.Setup(x => x.CommitAsync())
               .Callback(() => savedAnswer = answer)
               .Returns(() => Task.FromResult((object)null));


            var answerService = new AnswerService(userProvider, questionRepositoryMock.Object, answerRepositoryMock.Object,
               unitOfWorkMock.Object, MockedServices.Mapper);

            var result = await answerService.AcceptAnswerAsync(answer.Id);

            Assert.IsTrue(result.IsAccepted);
            Assert.AreEqual(answer.Id, result.Id);
        }

        [Test]
        public void InvalidUser_ThrowAccessDenied()
        {
            var userProvider = MockedServices.UserProvider;

            var answerRepositoryMock = new Mock<IAnswerRepository>();
            var questionRepositoryMock = new Mock<IQuestionRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var answer = new Answer()
            {
                Id = 1,
                Question = new Question()
                {
                    Id = 2,
                    User = new User() { Id = "fake" },
                    UserId = "fake"
                }
            };
            Answer savedAnswer = null;

            answerRepositoryMock.Setup(x => x.GetAnswerAsync(It.IsAny<int>()))
             .ReturnsAsync(answer);
            questionRepositoryMock.Setup(x => x.GetQuestionAsync(It.IsAny<int>()))
               .ReturnsAsync(answer.Question);

            unitOfWorkMock.Setup(x => x.CommitAsync())
               .Callback(() => savedAnswer = answer)
               .Returns(() => Task.FromResult((object)null));


            var answerService = new AnswerService(userProvider, questionRepositoryMock.Object, answerRepositoryMock.Object,
               unitOfWorkMock.Object, MockedServices.Mapper);

            Assert.ThrowsAsync<AccessDeniedException>(() => answerService.AcceptAnswerAsync(answer.Id));
        }
    }
}
