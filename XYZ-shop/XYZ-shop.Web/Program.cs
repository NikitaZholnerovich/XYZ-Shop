using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using XYZ_shop.Application.Abstractions.Mapping;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Mapping;
using XYZ_shop.Application.Services;
using XYZ_shop.Infrastructure.Data;
using XYZ_shop.Infrastructure.Repositories;
using XYZ_shop.Infrastructure.Security;
using XYZ_shop.Web.Auth;
using XYZ_shop.Web.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Jwt settings are not configured");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[JwtTokenService.ACCESS_TOKEN_COOKIE];
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.Redirect("/Auth/Login");
                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddDbContext<XyzDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDbConnection"),
        b => b.MigrationsAssembly("Xyz-shop.Infrastructure")
    ));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IGameGenreRepository, GameGenreRepository>();
builder.Services.AddScoped<IGameReviewRepository, GameReviewRepository>();
builder.Services.AddScoped<ICommunityChatMessageRepository, CommunityChatMessageRepository>();
builder.Services.AddScoped<IGameMapper, GameMapper>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICatalogViewModelMapper, CatalogViewModelMapper>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Steam}/{action=Index}/{id?}");

app.Run();
