namespace SidraHub.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    public int UpdatedBy { get; set; }
    public DateTime UpdatedDateTime { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime? DeletedDateTime { get; set; }
}
