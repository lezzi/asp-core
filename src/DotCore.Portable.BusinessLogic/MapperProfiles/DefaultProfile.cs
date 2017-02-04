using System.Linq;
using AutoMapper;
using DotCore.Portable.BusinessLogic.Models;
using DotCore.Portable.DataAccess.Entities;

namespace DotCore.Portable.BusinessLogic.MapperProfiles
{
    /// <summary>
    /// Default mapper <see cref="Profile"/>.
    /// </summary>
    public class DefaultProfile : Profile
    {
        #region Constructors

        public DefaultProfile()
        {
            CreateMap<Question, QuestionModel>();
            CreateMap<User, UserModel>();

            CreateMap<Question, QuestionModel>()
                .ForMember(x => x.IsAnswered, x => x.MapFrom(src => src.AcceptedAnswerId.HasValue));

            CreateMap<Answer, AnswerModel>();
            CreateMap<Question, QuestionDetailsModel>()
                .PreserveReferences()
                .ForMember(x => x.IsAnswered, x => x.MapFrom(src => src.AcceptedAnswerId.HasValue))
                .AfterMap((source, target) =>
                {
                    if (source.AcceptedAnswerId.HasValue)
                    {
                        var answer = target.Answers.FirstOrDefault(x => x.Id == source.AcceptedAnswerId.Value);
                        if (answer != null)
                            answer.IsAccepted = true;
                    }
                });
        }

        #endregion
    }
}