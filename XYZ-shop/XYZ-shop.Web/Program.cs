using Microsoft.EntityFrameworkCore;
using XYZ_shop.Application.Abstractions.Mapping;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Mapping;
using XYZ_shop.Application.Services;
using XYZ_shop.Infrastructure.Data;
using XYZ_shop.Infrastructure.Repositories;
using XYZ_shop.Web.Mapping;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Steam}/{action=Index}/{id?}");

app.Run();
