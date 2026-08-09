using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
namespace Controllers;

[ApiController]
[Route("books")]
public class BooksController(Context db) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetBooks() {
        return Ok(await db.Books.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id) {
        var found = await db.Books.FindAsync(id);
        return found != null ? Ok(found) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> PostBook(ParamBook request) {
        if(await db.Books.AnyAsync(i => i.title.ToLower() == request.Title.ToLower() && i.author.ToLower() == request.Author.ToLower()))
            return Conflict("There is already a book having this title and author.");

        var book = new Book {
            title = request.Title,
            author = request.Author
        };

        db.Books.Add(book);

        await db.SaveChangesAsync();

        return Created("/books/" + book.id, book);
    }
}

public record ParamBook (
    string Title,
    string Author
);