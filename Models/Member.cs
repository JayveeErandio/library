using System.ComponentModel.DataAnnotations.Schema;
namespace Models;

[Table("members")]
public class Member
{
    public int id { get; set; }

    public string name { get; set; } = "";

    public string email { get; set; } = "";

    public List<Borrowing> Borrowings { get; set; } = new();
}