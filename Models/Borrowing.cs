using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Borrowing
{
    public int id { get; set; }

    [Column("member_id")]
    public int MemberId { get; set; }

    [Column("book_id")]
    public int BookId { get; set; }

    public Member Member { get; set; } = null!;

    public Book Book { get; set; } = null!;
}