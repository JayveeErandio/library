using Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Controllers;

[ApiController]
[Route("members")]
public class MembersController(Context db) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetMembers() {
        var members = await db.Members.ToListAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMember(int id) {
        var found = await db.Members.FindAsync(id);
        return found != null ? Ok(found) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> PostMember(ParamMember request) {
        if(await db.Members.AnyAsync(i => i.email == request.Email)) return Conflict("Email is already used by other member");
        
        var member = new Member {
            name = request.Name,
            email = request.Email,
        };
        db.Members.Add(member);
        await db.SaveChangesAsync();
        return Created("/members/" + member.id, member);
    }
}

public record ParamMember (
        string Name,
        string Email
);