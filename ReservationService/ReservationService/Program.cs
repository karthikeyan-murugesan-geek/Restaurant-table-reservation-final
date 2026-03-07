using AutoMapper;

using ReservationService.DAL;
using ReservationService.DAL.Repositories;
using ReservationService.DAL.Repositories.Interfaces;
using ReservationService.Mappings;
using ReservationService.Models;
using ReservationService.Services;
using ReservationService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using ReservationService.Middlewares;
using System.Text.Json.Serialization;
using ReservationService.Filters;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
var apiSettings = builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();
var key = Encoding.UTF8.GetBytes(appSettings.Jwt.Key);
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token like: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"]
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenForwardingHandler>();
builder.Services.AddSingleton(typeof(AppSettings), appSettings);
builder.Services.AddSingleton(typeof(ApiSettings), apiSettings);
builder.Services.AddDbContext<ReservationContext>(options =>
    options.UseInMemoryDatabase("ReservationDB"));

builder.Services.AddHttpClient<CustomerApiService>().AddHttpMessageHandler<TokenForwardingHandler>();
builder.Services.AddScoped<IReservationService, ReservationService.Services.ReservationService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddAutoMapper(typeof(ReservationProfile));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ReservationContext>();
    DataSeeder.Seed(context);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exceptionFeature?.Error.Message, "Unhandled exception occurred");
        if (exceptionFeature != null)
        {
            var error = new
            {
                Message = "An unexpected error occurred.",
                Detail = exceptionFeature.Error.Message
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    });
});
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthorizationMiddleware>();

app.MapControllers();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();