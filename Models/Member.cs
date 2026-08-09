namespace Models;

public class Member
{
    public int id { get; set; }

    public string name { get; set; } = "";

    public string email { get; set; } = "";

    public List<Borrowing> Borrowings { get; set; } = new();
}