using AutoMapper;
using DatingApp.Application.Features.Auth.DTOs;
using DatingApp.Application.Features.Matches.DTOs;
using DatingApp.Application.Features.Messages.DTOs;
using DatingApp.Application.Features.Users.DTOs;
using DatingApp.Domain.Entities;

namespace DatingApp.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.Age, o => o.MapFrom(s => s.Age))
            .ForMember(d => d.MainPhotoUrl, o => o.MapFrom(s => s.MainPhoto != null ? s.MainPhoto.Url : null))
            .ForMember(d => d.Photos, o => o.MapFrom(s => s.Photos));

        CreateMap<User, UserProfileDto>()
            .ForMember(d => d.Age, o => o.MapFrom(s => s.Age))
            .ForMember(d => d.MainPhotoUrl, o => o.MapFrom(s => s.MainPhoto != null ? s.MainPhoto.Url : null))
            .ForMember(d => d.Photos, o => o.MapFrom(s => s.Photos));

        CreateMap<Photo, PhotoDto>();
        CreateMap<Match, MatchDto>()
            .ForMember(d => d.LastMessage, o => o.MapFrom(s => s.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()));
        CreateMap<Message, MessageDto>();
    }
}
