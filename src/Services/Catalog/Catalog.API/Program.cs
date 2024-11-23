using StackExchange.Redis;
using Serilog;
using Catalog.API.Data;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Logging
        builder.Host.UseSerilog((context, loggerConfig) => 
        {
            loggerConfig.WriteTo.Console();
        });

        builder.Services.AddControllers();

        builder.Services.AddScoped<DapperDbContext>(sp => 
        {
            var dbConnectionSttring = builder.Configuration.GetConnectionString("PlaygroundDB");
            return new DapperDbContext(dbConnectionSttring);
        });

        builder.Services.AddScoped<IProductRepository, ProductRepositoryDapper>();

        builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
        {
            var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.MapControllers();
        app.Run();
    }
}
