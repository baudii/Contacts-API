using ContactsAPI.Application.Features.Contacts.Handlers;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using ContactsAPI.Infrastructure;
using ContactsAPI.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddRouting(options => options.LowercaseUrls = true);


builder.Services.AddScoped<ICrudRepository<Contact>, ContactRepository>();
builder.Services.AddScoped<ICrudRepository<Person>, PersonRepository>();
builder.Services.AddScoped<IConfiguration>(x => builder.Configuration);

services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(typeof(CreateContactCommandHandler).Assembly);
});

services.AddDbContext<ContactsDbContext>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();
