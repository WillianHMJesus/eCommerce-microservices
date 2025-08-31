using Carter;
using EM.Authentication.API.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencyInjection(builder.Configuration);
builder.Services.AddCarter(configurator: cfg =>
{
    cfg.WithValidatorLifetime(ServiceLifetime.Scoped);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCarter();
app.Run();

public partial class Program { }