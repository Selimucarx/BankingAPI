using System.Text;
using BankingAPI.Application.Mappings;
using BankingAPI.Application.Validators;
using BankingAPI.Domain.Interfaces;
using BankingAPI.Domain.Manager;
using BankingAPI.Exception;
using BankingAPI.Infrastructure.Persistence;
using BankingAPI.Infrastructure.Persistence.Repositories;
using BankingAPI.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerValidator>());


// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AccountValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TransactionValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PurchaseRequestValidator>();


builder.Services.AddValidatorsFromAssemblyContaining<CustomerUpdateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerPasswordChangeValidator>();

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ICardRepository, CardRepository>();

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();


builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<AccountManager>();
builder.Services.AddScoped<CardManager>();
builder.Services.AddScoped<TransactionManager>();


builder.Services.AddScoped<PasswordHasher>();


// Configure JwtSettings and Add JwtService
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IJwtService, JwtService>();

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(CustomerMapper));
builder.Services.AddAutoMapper(typeof(AccountMapper));
builder.Services.AddAutoMapper(typeof(CardMapper));
builder.Services.AddAutoMapper(typeof(TransactionMapper));


// Swagger Configuration
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Banking API", Version = "v1" }); });

// JWT Authentication Configuration
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = jwtSection["Secret"];
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var app = builder.Build();


app.UseMiddleware<TokenValidationMiddleware>();


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking API v1");
    c.RoutePrefix = string.Empty;
});

// Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();