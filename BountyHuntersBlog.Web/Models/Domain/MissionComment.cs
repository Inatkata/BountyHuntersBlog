using System.ComponentModel.DataAnnotations;
using static BountyHuntersBlog.Constants.EntityConstants.MissionComment;
using BountyHuntersBlog.Models.Domain;

public class MissionComment
{
    public Guid Id { get; set; }

    public Guid MissionPostId { get; set; }

    public MissionPost MissionPost { get; set; } = null!;

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;
    
    public string UserId { get; set; } = null!;

    public Hunter Hunter { get; set; } = null!;

    public DateTime DateAdded { get; set; }
}