using System.ComponentModel.DataAnnotations;
using UUIDNext;

namespace CardsApp.Domain.Entities;

public class BaseEntity
{
    [Key] 
    public string Id { get; set; } = Uuid.NewDatabaseFriendly(Database.PostgreSql).ToString();
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}