using System.ComponentModel.DataAnnotations.Schema;
namespace Models;

[Table("books")]
public class Book
{
    public int id { get; set; }

    public string title { get; set; } = "";

    public string author { get; set; } = "";

    public List<Borrowing> Borrowings { get; set; } = new();
}