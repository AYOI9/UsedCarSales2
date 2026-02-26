using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UsedCarsAPI.Models;
using UsedCarsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Used Cars API", Version = "v1" });
    options.AddSecurityDefinition("JWT_OR_COOKIE", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Введите валидный JWT токен",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id=JwtBearerDefaults.AuthenticationScheme
                    }
                },
                new string[]{}
            }
        });
});

// строка подключения к нашей базе kursdb16
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<UsedCarsDb16Context>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddScoped<ClientsService, ClientsService>();
builder.Services.AddScoped<ContractsService, ContractsService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // издатель
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            // потребитель
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            // время жизни
            ValidateLifetime = true,
            // ключ
            IssuerSigningKey = AuthOptions.GetSimmetricSecurutyKey(),
            ValidateIssuerSigningKey = true
        };
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

// Авторизация
app.MapPost("/login", async (Person user, UsedCarsDb16Context db) =>
{
    Person? person = await db.Persons!.FirstOrDefaultAsync(p => p.Email == user.Email);
    string passwordHash = AuthOptions.GetHash(user.Password);
    if (person is null) return Results.Unauthorized();
    if (person.Password != passwordHash) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };
    var jwt = new JwtSecurityToken
    (
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSimmetricSecurutyKey(), SecurityAlgorithms.HmacSha256)
    );
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    var response = new
    {
        access_token = encodedJwt,
        username = person.Email
    };
    return Results.Json(response);
});

// Регистрация
app.MapPost("/register", async (Person user, UsedCarsDb16Context db) =>
{
    user.Password = AuthOptions.GetHash(user.Password);
    db.Persons.Add(user);
    await db.SaveChangesAsync();
    Person createdUser = db.Persons.First(p => p.Email == user.Email);
    return Results.Ok(createdUser);
});

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UsedCarsDb16Context>();
    SeedData.SeedDatabase(context);
}

app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer";
    public const string AUDIENCE = "MyAuthClient";
    const string KEY = "mysupersecret_secretsecretkey!123";
    public static SymmetricSecurityKey GetSimmetricSecurutyKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

    public static string GetHash(string plaintext)
    {
        var sha = new SHA1Managed();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
        return Convert.ToBase64String(hash);
    }
}
