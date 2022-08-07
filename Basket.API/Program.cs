using Infra.RabbitMQ;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IConnectionFactory>(p =>
{
    return new ConnectionFactory()
    {
        HostName = "localhost",
        UserName = "admin",
        Password = "123456"
        //HostName = rmqSettings.HostName,
        //Port = rmqSettings.Port,
        //UserName = rmqSettings.UserName,
        //Password = rmqSettings.Password,
        //VirtualHost = rmqSettings.VirtualHost,
        //DispatchConsumersAsync = true, // this is mandatory to have Async Subscribers
    };
});
builder.Services.AddSingleton(typeof(IQueuePublisher<>), typeof(RabbitMqBusQueue<>));
builder.Services.AddSingleton(typeof(IQueueSubscriber<>), typeof(RabbitMqBusQueue<>));
builder.Services.AddSingleton<IRabbitConnection, RabbitPersistentConnection>();


builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();
app.Run();
