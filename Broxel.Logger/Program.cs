using Serilog;
using Broxel.Logger.Middleware;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Broxel.Logger.Services;
using Broxel.Logger.Security;
using Microsoft.AspNetCore.Authentication;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddAuthentication("BasicAuthentification")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentification", null);


//Add support to logging with SERILOG
IConfigurationRoot configuration = new ConfigurationBuilder()
 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();



var _logger = new Serilog.LoggerConfiguration()
    .ReadFrom.Configuration(configuration).CreateLogger();

builder.Logging.AddSerilog(_logger);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//Agregar el Middleware
app.UseMiddleware<MiddlewareLogger>();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
