using AdventureWorks.API.Application.Common.Data;
using AdventureWorks.API.Application.Common.Extensions;
using AdventureWorks.API.Application.Common.Pipelines;
using AdventureWorks.API.Application.Common.Settings;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStrings = builder.Configuration.BindAndValidate<ConnectionStrings, ConnectionStringsValidator>();
builder.Services.AddSingleton(connectionStrings);

builder.Services.AddTransient<IDbConnectionFactory, SqlDbConnectionFactory>();

builder.Services
    .AddMediatR(options => { options.RegisterServicesFromAssemblyContaining(typeof(Program)); })
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(AuditLoggingBehaviour<,>));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();