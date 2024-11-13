using ContactsAPI.Application.Features.Contacts.Handlers;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using ContactsAPI.Infrastructure;
using ContactsAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddRouting(options => options.LowercaseUrls = true);


builder.Services.AddScoped<ICrudRepository<Contact>, ContactRepository>();
builder.Services.AddScoped<ICrudRepository<Person>, PersonRepository>();

services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(typeof(CreateContactCommandHandler).Assembly);
});

services.AddDbContext<ContactsDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("ContactsDb"));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
