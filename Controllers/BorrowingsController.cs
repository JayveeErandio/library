using Data;
using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Controllers;

[ApiController]
public class BorrowingsController(Context db, BorrowingService service) : ControllerBase {
    [HttpPost("/members/{member_id}/borrow/{book_id}")]
    public async Task<IActionResult> BorrowBook(int member_id, int book_id){
        var value = await service.BorrowBookAsync(member_id, book_id);
        return value.Status switch {
            BorrowingService.Status.NoMember => NotFound("No Member Found"),
            BorrowingService.Status.NoBook => NotFound("No Book found"),
            BorrowingService.Status.OtherBorrow =>  Conflict("The book is currently borrowed"),
            BorrowingService.Status.Success => Created("/members/" + member_id + "/borrowings", value.Borrowing),
            _ => Conflict("Unknown resolve")
        };
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