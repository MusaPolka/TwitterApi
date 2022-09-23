using AutoMapper;
using DomainLayer.DTOs;
using DomainLayer.Entities;

namespace TwitterAPI.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<UpdateUserDto, User>().ReverseMap();

            CreateMap<RequestToFollow, FollowerFollowing>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.SourceUserId, opt => opt.MapFrom(src => src.SourceUserId))
                .ForMember(dest => dest.SourceUser, opt => opt.MapFrom(src => src.SourceUser))
                .ForMember(dest => dest.FollowedUserId, opt => opt.MapFrom(src => src.FollowedUserId))
                .ForMember(dest => dest.FollowedUser, opt => opt.MapFrom(src => src.FollowedUser));

            CreateMap<RequestToFollow, SenderReciever>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SourceUserId))
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.SourceUser))
                .ForMember(dest => dest.RecieverId, opt => opt.MapFrom(src => src.FollowedUserId))
                .ForMember(dest => dest.Reciever, opt => opt.MapFrom(src => src.FollowedUser));

            CreateMap<Message, MessageDto>();
            CreateMap<Tweet, TweetDto>();
            CreateMap<TweetDto, Tweet>();
        }

    }
}
