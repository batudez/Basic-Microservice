using MassTransit;
using Payment_Web_Api.Consumers;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransit(configurator =>
{
	configurator.AddConsumer<StockReservedEventConsumer>();
	configurator.UsingRabbitMq((context, _configurator) =>
	{
		_configurator.Host(builder.Configuration.GetConnectionString("RabbitMq"));

		_configurator.ReceiveEndpoint(RabbitMQSettings.Payment_StockReservedEventQueue,
			e => e.ConfigureConsumer<StockReservedEventConsumer>(context));
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
