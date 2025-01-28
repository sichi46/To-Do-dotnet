using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


// Create the routes 

app.MapGet("/todoitems", async(TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async(TodoDb db)=>
    await db.Todos.Where( t => t.IsComplete).ToListAsync());    

app.MapGet("/todoitems/{id}", async(TodoDb db, int id) => 
    await db.Todos.FindAsync(id)
        is Todo todo 
        ? Results.Ok(todo) 
        : Results.NotFound());
    
app.MapPost("/todoitems", async(Todo todo, TodoDb db) =>{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});
    