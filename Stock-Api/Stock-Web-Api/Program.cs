using MassTransit;
using MongoDB.Driver;
using Shared;
using Stock_Web_Api.Consumers;
using Stock_Web_Api.Models.Entities;
using Stock_Web_Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



builder.Services.AddMassTransit(configurator =>
{
	configurator.AddConsumer<OrderCreatedEventConsumer>();
	configurator.UsingRabbitMq((context, _configurator) =>
	{
		_configurator.Host(builder.Configuration.GetConnectionString("RabbitMq"));

		_configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, 
			e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
	});
});

builder.Services.AddSingleton<MongoDbService>();

using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
MongoDbService mongoDbService = scope.ServiceProvider.GetService<MongoDbService>();
var collection = mongoDbService.GetCollection<Stock>();
if (!collection.FindSync(s => true).Any()) 
{
	await collection.InsertOneAsync(new() { ProductId = "Adwqcas12", Count = 2000 });
	await collection.InsertOneAsync(new() { ProductId = "asdlmkqw", Count = 1000 });
	await collection.InsertOneAsync(new() { ProductId = "lfdkvbmlfdþ", Count = 3000 });
	await collection.InsertOneAsync(new() { ProductId = "qwdqdwq", Count = 5000 });
	await collection.InsertOneAsync(new() { ProductId = "asdasdsad", Count = 500 });
}


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
