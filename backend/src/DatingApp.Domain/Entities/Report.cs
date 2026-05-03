using DatingApp.Domain.Common;
using DatingApp.Domain.Enums;

namespace DatingApp.Domain.Entities;

public class Report : BaseEntity
{
    private Report() { }

    public Guid ReporterId { get; private set; }
    public Guid ReportedUserId { get; private set; }
    public ReportReason Reason { get; private set; }
    public string? Description { get; private set; }
    public bool IsReviewed { get; private set; }

    public User Reporter { get; private set; } = null!;
    public User ReportedUser { get; private set; } = null!;

    public static Report Create(Guid reporterId, Guid reportedUserId, ReportReason reason, string? description)
        => new() { ReporterId = reporterId, ReportedUserId = reportedUserId, Reason = reason, Description = description };
}
