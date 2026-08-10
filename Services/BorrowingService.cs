using Microsoft.EntityFrameworkCore;
using Data;
using Models;
namespace Services;

public class BorrowingService(Context db) {
    public async Task<Result> BorrowBookAsync(int member_id, int book_id) {
        if (!await db.Members.AnyAsync(i => i.id == member_id)) return new Result(Status.NoMember, null);
        if (!await db.Books.AnyAsync(i => i.id == book_id)) return new Result(Status.NoBook, null);

        if(await db.Borrowings.AnyAsync(i => i.BookId == book_id && i.ReturnedAt == null))
            return new Result(Status.OtherBorrow, null);

        var borrowing = new Borrowing {
            MemberId = member_id,
            BookId = book_id,
            BorrowedAt = DateTime.UtcNow
        };

        db.Borrowings.Add(borrowing);

        await db.SaveChangesAsync();

        return new Result(Status.Success, borrowing);
    }

    public enum Status {
        NoMember,
        NoBook,
        OtherBorrow,
        Success
    }

    public record Result (
        Status Status,
        Borrowing? Borrowing
    );
}