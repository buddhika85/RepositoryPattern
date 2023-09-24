using CustomerAPI_RepositoryPattern.Data;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("SQLDBConnection")
     ));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region routes

// get all
app.MapGet("api/v1/customers", async (AppDbContext context) =>
{
    var customers = await context.Customers.ToListAsync();
    return Results.Ok(customers);
});

// get by id
app.MapGet("api/v1/customers/{customerId}", async (AppDbContext context, string customerId) =>
{
    var customer = await context.Customers.FirstOrDefaultAsync(x => x.CustomerGuid == customerId);
    return customer != null ?
        Results.Ok(customer) :
        Results.NotFound($"Customer with ID {customerId} not found");
});

// create
app.MapPost("api/v1/customers", async (AppDbContext context, Customer customer) =>
{
    await context.Customers.AddAsync(customer);
    await context.SaveChangesAsync();

    return Results.Created($"api/v1/customers/{customer.CustomerGuid}", customer);
});

// update
app.MapPut("api/v1/customers/{customerId}", async (AppDbContext context, string customerId, Customer customer) =>
{
    var customerFromDb = await context.Customers.FirstOrDefaultAsync(x => x.CustomerGuid == customerId);

    if (customerFromDb is null)
        return Results.NotFound($"Customer with ID {customerId} not found");

    customerFromDb.FirstName = customer.FirstName;
    customerFromDb.LastName = customer.LastName;
    customerFromDb.IsPremium = customer.IsPremium;
    await context.SaveChangesAsync();

    return Results.Ok("Customer updated");
});

// delete
app.MapDelete("api/v1/customers/{customerId}", async (AppDbContext context, string customerId) =>
{
    var customerFromDb = await context.Customers.FirstOrDefaultAsync(x => x.CustomerGuid == customerId);

    if (customerFromDb is null)
        return Results.NotFound($"Customer with ID {customerId} not found");

    context.Customers.Remove(customerFromDb);
    await context.SaveChangesAsync();
    return Results.Ok("Customer deleted");
});

#endregion routes

app.Run();
