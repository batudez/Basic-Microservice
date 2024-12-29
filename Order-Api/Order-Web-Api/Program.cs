using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order_Web_Api.Consumers;
using Order_Web_Api.Context;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<OrderApiDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
});

builder.Services.AddMassTransit(configurator =>
{
	configurator.AddConsumer<PaymentCompletedEventConsumer>();
	configurator.AddConsumer<StockNotReservedEventConsumer>();
	configurator.AddConsumer<PaymentFailedEventConsumer>();
	configurator.UsingRabbitMq((context, _configurator) =>
	{
		_configurator.Host(builder.Configuration.GetConnectionString("RabbitMq"));

		_configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentCompletedEventQueue, 
			e => e.ConfigureConsumer<PaymentCompletedEventConsumer>(context));

		_configurator.ReceiveEndpoint(RabbitMQSettings.Order_StockNotReservedEventQueue,
			e => e.ConfigureConsumer<StockNotReservedEventConsumer>(context));

		_configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue,
			e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
	});
});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
