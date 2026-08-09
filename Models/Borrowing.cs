using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("borrowings")]
public class Borrowing
{
    public int id { get; set; }

    [Column("member_id")]
    public int MemberId { get; set; }

    [Column("book_id")]
    public int BookId { get; set; }

    [Column("borrowed_at")]
    public DateTime BorrowedAt { get; set; }

    [Column("returned_at")]
    public DateTime? ReturnedAt { get; set; }

    public Member Member { get; set; } = null!;

    public Book Book { get; set; } = null!;
}