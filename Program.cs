using Microsoft.EntityFrameworkCore;
using Data;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Context>(options => {
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Supabase")
    );
});
builder.Services.AddScoped<BorrowingService>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

app.MapGet("/", () => """
GET http://localhost:5100/

GET http://localhost:5100/members/1

POST http://localhost:5100/members
Content-Type: application/json

{
    "name": "Anthony",
    "email": "hehe@sadsad"
}

##############################################

GET http://localhost:5100/books

GET http://localhost:5100/books/2

POST http://localhost:5100/books
Content-Type: application/json
{
    "title": "How to cook eggs",
    "author": "Jayvee Erandio"
}

GET http://localhost:5100/books/available

##############################################

POST http://localhost:5100/members/4/borrow/9

GET http://localhost:5100/members/5/borrowings

PATCH http://localhost:5100/members/1/return/4

""");

app.MapGet("/version", () => new
{
    version = "3.0",
    deployedBy = "GitHub Actions",
    issue = "Kapikon lang ang naka naka"
});

app.UseCors("Frontend");
app.MapControllers();
app.Run();
