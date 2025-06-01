using Microsoft.EntityFrameworkCore;

namespace server.Models;


[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Ip), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public string Ip { get; set; } = String.Empty;
    public int HighScore { get; set; }
    public int IconId { get; set; }
    public string Name { get; set; } = String.Empty;
}