using Auth.Core.Models;
using Auth.Core.Repositories;
using Auth.Core.Security.Tokens;
using Auth.Core.Services;
using Auth.Mapping;
using Auth.Persistence;
using Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenHandler = Auth.Security.Tokens.TokenHandler;
using TokenOptions = Auth.Security.Tokens.TokenOptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DatabaseSeeder>();

builder.Services.AddScoped<ITokenHandler, TokenHandler>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("TokenOptions"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenOptions?.Issuer,
            ValidAudience = tokenOptions?.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions?.Secret))
        };
    });

Type[] profiles = new Type[] { typeof(ResourceToModelProfile), typeof(ModelToResourceProfile) };
builder.Services.AddAutoMapper(profiles);

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    //options.UseInMemoryDatabase("jwtapi");
    //options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
