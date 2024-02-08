using AutoMapper;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Discount.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateDiscountCommandHandler>(
));

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<DiscountService>();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client");
});

app.MapRazorPages();

app.Run();
