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

app.MapGet("/", () => "Hello World!");

app.UseCors("Frontend");
app.MapControllers();
app.Run();
