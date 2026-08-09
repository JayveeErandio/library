using Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Controllers;

[ApiController]
public class BorrowingsController(Context db) : ControllerBase {
    [HttpPost("/members/{member_id}/borrow/{book_id}")]
    public async Task<IActionResult> BorrowBook(int member_id, int book_id){
        if (!await db.Members.AnyAsync(i => i.id == member_id)) return NotFound("No Member found");
        if (!await db.Books.AnyAsync(i => i.id == book_id)) return NotFound("No Book Found");

        if(await db.Borrowings.AnyAsync(i => i.BookId == book_id && i.ReturnedAt == null))
            return Conflict("The book is currently borrowed");

        var borrowing = new Borrowing {
            MemberId = member_id,
            BookId = book_id,
            BorrowedAt = DateTime.UtcNow
        };

        db.Borrowings.Add(borrowing);

        await db.SaveChangesAsync();

        return Created("/members/" + member_id + "/borrowings", borrowing);
    }

    [HttpGet("/members/{member_id}/borrowings")]
    public async Task<IActionResult> BorrowHistory(int member_id) {
        if (!await db.Members.AnyAsync(i => i.id == member_id)) return NotFound("No Member Found");

        return Ok(
            await db.Borrowings
                .Where(i => i.MemberId == member_id)
                .Select(i => new {
                    i.id, 
                    i.Book, 
                    BorrowedAt = ConvertDateTime(i.BorrowedAt),
                    ReturnedAt = i.ReturnedAt != null ? ConvertDateTime((DateTime) i.ReturnedAt) : null
                })
                .ToListAsync()
        );
    }

    [HttpPatch("/members/{member_id}/return/{book_id}")]
    public async Task<IActionResult> ReturnBook(int member_id, int book_id) {
        if (!await db.Members.AnyAsync(i => i.id == member_id)) return NotFound("No Member found");
        if (!await db.Books.AnyAsync(i => i.id == book_id)) return NotFound("No Book Found");

        var found = await db.Borrowings.FirstOrDefaultAsync(i => i.MemberId == member_id && i.BookId == book_id && i.ReturnedAt == null);
        if(found == null)
            return NotFound("The member has not borrowed the book at the first place");
        else
            found.ReturnedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return Ok("The book is successfully returned.");
    }

    public static object ConvertDateTime(DateTime basis) {
        TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");

        DateTime userLocalTime = TimeZoneInfo.ConvertTimeFromUtc(basis, timezone);
        
        return userLocalTime.ToString("MMMM dd, yyyy | h:mm tt");
    }
};