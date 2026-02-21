using ContactManagementSystem.API.Validators;
using ContactManagementSystem.Repositories;
using ContactManagementSystem.Repositories.Interface;
using ContactManagementSystem.Repositories.Repository;
using ContactManagementSystem.Services.Automapper;
using ContactManagementSystem.Services.Interface;
using ContactManagementSystem.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CONTROLLERS
builder.Services.AddControllers();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Contact Management API",
        Version = "v1"
    });

    // JWT AUTH CONFIG FOR SWAGGER
    options.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter Bearer token. Example: Bearer eyJhbGciOiJIUzI1NiIs..."
        });

    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


// IDENTITY
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// REPOSITORY DI
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddHostedService<DbInitializer>();

// SERVICE DI
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddAutoMapper(typeof(ContactMapping));

builder.Services.AddScoped<ContactValidator>();
builder.Services.AddScoped<RegisterValidator>();
builder.Services.AddScoped<LoginValidator>();
builder.Services.AddScoped<UpdateValidator>();

builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddHttpContextAccessor();

// JWT AUTHENTICATION
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// MIDDLEWARE PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();