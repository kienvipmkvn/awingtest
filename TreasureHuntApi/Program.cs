using Microsoft.EntityFrameworkCore;
using TreasureHuntApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy for frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<TreasureHuntContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Use CORS
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// POST /api/treasurehunt/solve
app.MapPost("/api/treasurehunt/solve", async (TreasureHuntInput input, TreasureHuntContext db) =>
{
    // Validation
    if (input.N <= 0 || input.M <= 0 || input.P <= 0 || input.Matrix == null ||
        input.Matrix.Length != input.N || input.Matrix.Any(row => row.Length != input.M))
    {
        return Results.BadRequest("Invalid input dimensions.");
    }
    // Validate matrix values
    var validKeys = Enumerable.Range(0, input.P + 1).ToHashSet();
    foreach (var row in input.Matrix)
        foreach (var v in row)
            if (!validKeys.Contains(v))
                return Results.BadRequest($"Matrix contains invalid key value: {v}");

    // Store input
    db.Inputs.Add(input);
    await db.SaveChangesAsync();

    // Solve
    var (minFuel, path) = TreasureHuntSolver.Solve(input);
    var result = new TreasureHuntResult
    {
        InputId = input.Id,
        MinFuel = minFuel,
        Path = string.Join("->", path.Select(pos => $"({pos.x+1},{pos.y+1})"))
    };
    db.Results.Add(result);
    await db.SaveChangesAsync();

    return Results.Ok(new { input.Id, result.MinFuel, result.Path });
});

// GET /api/treasurehunt/submissions
app.MapGet("/api/treasurehunt/submissions", async (TreasureHuntContext db) =>
{
    var submissions = await db.Inputs.ToListAsync();
    var results = await db.Results.ToListAsync();
    var list = submissions.Select(i => new {
        i.Id, i.N, i.M, i.P, i.Matrix,
        Result = results.FirstOrDefault(r => r.InputId == i.Id)
    });
    return Results.Ok(list);
});

// GET /api/treasurehunt/submissions/{id}
app.MapGet("/api/treasurehunt/submissions/{id}", async (int id, TreasureHuntContext db) =>
{
    var input = await db.Inputs.FindAsync(id);
    if (input == null) return Results.NotFound();
    var result = await db.Results.FirstOrDefaultAsync(r => r.InputId == id);
    return Results.Ok(new { input, result });
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
