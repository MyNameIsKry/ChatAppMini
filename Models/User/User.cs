using System.ComponentModel.DataAnnotations;

namespace ChatAppMini.Models;
public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}
