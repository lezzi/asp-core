using System;
using System.Threading.Tasks;
using DotCore.Portable.BusinessLogic.ApplicationServices;
using DotCore.Portable.BusinessLogic.Models;
using DotCore.Portable.DataAccess;
using DotCore.Portable.DataAccess.Entities;
using DotCore.Portable.DataAccess.Repositories;
using Moq;
using NUnit.Framework;

namespace DotCore.Portable.Tests.UnitTests
{
    
    [TestFixture]
    public class AskServiceTests
    {
        public void Setup()
        {
            
        }

        [TestFixture]
        public class AskQuestionAsync : AskServiceTests
        {
            [Test]
            public async Task AddValidObject()
            {
                var askModel = new AskQuestionModel()
                {
                    Title = "Test title",
                    Description = "Test description"
                };

                Question addedQuestion = null;
                Question savedQuestion = null;

                var userProvider = MockedServices.UserProvider;

                var unitOfWorkMock = new Mock<IUnitOfWork>();
                var questionRepositoryMock = new Mock<IQuestionRepository>();

                questionRepositoryMock.Setup(x => x.AddQuestion(It.IsAny<Question>()))
                    .Callback<Question>(question => addedQuestion = question);
                unitOfWorkMock.Setup(x => x.CommitAsync())
                    .Callback(() => savedQuestion = addedQuestion)
                    .Returns(() => Task.FromResult((object) null));

                var askService = new AskService(userProvider, questionRepositoryMock.Object, unitOfWorkMock.Object,
                    MockedServices.Mapper);

                var returnedQuestion = await askService.AskQuestionAsync(askModel);

                Assert.IsNotNull(savedQuestion);
                Assert.AreEqual(userProvider.CurrentUserId, savedQuestion.UserId);
                Assert.AreEqual(askModel.Title, savedQuestion.Title);
                Assert.AreEqual(askModel.Description, savedQuestion.Description);
                Assert.IsNull(savedQuestion.AcceptedAnswer);
                Assert.IsNull(savedQuestion.AcceptedAnswerId);
                Assert.AreNotEqual(default(DateTime), savedQuestion.CreatedOn);

                Assert.AreEqual(false, returnedQuestion.IsAnswered);
            }
        }
    }
}