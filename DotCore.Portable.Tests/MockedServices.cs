using AutoMapper;
using DotCore.Portable.BusinessLogic;
using DotCore.Portable.BusinessLogic.MapperProfiles;
using DotCore.Portable.DataAccess.Entities;
using Moq;

namespace DotCore.Portable.Tests
{
    public static class MockedServices
    {
        public static IUserProvider UserProvider 
        {
            get
            {
                const string userName = "test@test.com";
                var mock = new Mock<IUserProvider>();

                mock.Setup(x => x.CurrentUserId).Returns(userName);
                mock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(new User()
                {
                   Id = userName,
                   Email = userName
                });

                return mock.Object;
            }
        }

        public static IMapper Mapper
        {
            get
            {
                var mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new DefaultProfile())));

                return mapper;
            }
        }
    }
}