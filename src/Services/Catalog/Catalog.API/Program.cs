using StackExchange.Redis;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        /// Add services to the container.
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
