using Evaluation_Backend_Amaya.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("backOffice", p => p.WithOrigins("*").WithMethods("GET", "POST", "PUT", "DELETE", "OPTION").AllowAnyHeader());
    }
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DatabaseHelper.sDbConnectionString = "Data Source=" + builder.Configuration.GetValue<string>("SVName") + ";initial catalog=evaluation_amaya_2024;" +
    "user id=fitAdmin;password=itChAT@#;Encrypt=False";

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("backOffice");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
