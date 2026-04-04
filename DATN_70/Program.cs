using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DATN_70.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DATN_70.Data.SqlConnectionFactory>();
builder.Services.AddScoped<DATN_70.Data.StorefrontDataSeeder>();
builder.Services.AddScoped<DATN_70.Services.IStoreRepository, DATN_70.Services.StoreRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DATN_70.Data.AppDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DATN_70.Data.StorefrontDataSeeder>();
    await seeder.SeedAsync();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();
