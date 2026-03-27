var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DATN_70.Data.SqlConnectionFactory>();
builder.Services.AddScoped<DATN_70.Services.IStoreRepository, DATN_70.Services.StoreRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
