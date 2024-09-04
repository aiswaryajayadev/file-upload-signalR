using Application.Command;
using Application.Command_Handler;
using Infrastructure.FileUploadHub;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddMediatR(typeof(ProcessFileCommand).Assembly);
builder.Services.AddTransient<IRequestHandler<ProcessFileCommand, Unit>, ProcessFileCommandHandler>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            // Allow port 4200 specifically
            if (origin == "http://localhost:4200")
            {
                return true;
            }

            // Allow any other port on localhost
            Uri uri = new Uri(origin);
            return uri.Host == "localhost";
        })
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials(); // Use this cautiously
    });
});



var app = builder.Build();
app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<FileProcessingHub>("/fileProcessingHub");
});

app.Run();
