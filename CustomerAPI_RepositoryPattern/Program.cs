using CustomerAPI_RepositoryPattern.Data;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("SQLDBConnection")
     ));

// IMPORTANT - Telling built in DI to give an instance of SqlCustomerRepository if code asks for an object ICustomerRepository
builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region routes

// get all
app.MapGet("api/v1/customers", async (ICustomerRepository repository) =>
{
    var customers = await repository.GetAllCustomersAsync();
    return Results.Ok(customers);
});

// get by id
app.MapGet("api/v1/customers/{customerId}", async (ICustomerRepository repository, string customerId) =>
{
    var customer = await repository.GetCustomerByIdAsync(customerId);
    return customer != null ?
        Results.Ok(customer) :
        Results.NotFound($"Customer with ID {customerId} not found");
});

// create
app.MapPost("api/v1/customers", async (ICustomerRepository repository, Customer customer) =>
{
    await repository.CreateCustomerAsync(customer);
    await repository.SaveChangesAsync();

    return Results.Created($"api/v1/customers/{customer.CustomerGuid}", customer);
});

// update
app.MapPut("api/v1/customers/{customerId}", async (ICustomerRepository repository, string customerId, Customer customer) =>
{
    var customerFromDb = await repository.GetCustomerByIdAsync(customerId);

    if (customerFromDb is null)
        return Results.NotFound($"Customer with ID {customerId} not found");

    customerFromDb.FirstName = customer.FirstName;
    customerFromDb.LastName = customer.LastName;
    customerFromDb.IsPremium = customer.IsPremium;
    await repository.SaveChangesAsync();

    return Results.Ok("Customer updated");
});

// delete
app.MapDelete("api/v1/customers/{customerId}", async (ICustomerRepository repository, string customerId) =>
{
    var customerFromDb = await repository.GetCustomerByIdAsync(customerId);

    if (customerFromDb is null)
        return Results.NotFound($"Customer with ID {customerId} not found");

    repository.DeleteCustomer(customerFromDb);
    await repository.SaveChangesAsync();
    return Results.Ok("Customer deleted");
});

#endregion routes

app.Run();
