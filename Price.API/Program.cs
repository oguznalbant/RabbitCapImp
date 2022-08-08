using Infra.RabbitMQ;
using Infra.RabbitMQ.CAP;
using Price.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRabbitConnection();
builder.Services.AddCap();
builder.Services.AddSingleton(typeof(IQueuePublisher<>), typeof(RabbitMqBusQueue<>));
builder.Services.AddSingleton(typeof(IQueueSubscriber<>), typeof(RabbitMqBusQueue<>));
builder.Services.AddSingleton<IRabbitConnection, RabbitPersistentConnection>();
builder.Services.AddHostedService<EventService>();
builder.Services.AddTransient<PriceCalculateService>();
// Add services to the container.
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
