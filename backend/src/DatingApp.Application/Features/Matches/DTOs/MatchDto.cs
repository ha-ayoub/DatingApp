using DatingApp.Application.Features.Messages.DTOs;
using DatingApp.Application.Features.Users.DTOs;

namespace DatingApp.Application.Features.Matches.DTOs;

public class MatchDto
{
    public Guid Id { get; set; }
    public UserDto OtherUser { get; set; } = null!;
    public MessageDto? LastMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}
