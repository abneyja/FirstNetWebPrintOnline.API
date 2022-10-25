using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using FirstNetWebPrintOnline.Data;
using Microsoft.EntityFrameworkCore;
using FirstNetWebPrintOnline.Scope;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<FirstNetWebPrintOnlineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstNetWebPrintOnlineConnectionStringDev")));
}else
    builder.Services.AddDbContext<FirstNetWebPrintOnlineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstNetWebPrintOnlineConnectionString")));



string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:printrequests", policy => policy.Requirements.Add(new HasScopeRequirement("read:printrequests", $"https://{builder.Configuration["Auth0:Domain"]}/")));
    options.AddPolicy("write:printrequest", policy => policy.Requirements.Add(new HasScopeRequirement("write:printrequest", $"https://{builder.Configuration["Auth0:Domain"]}/")));
    options.AddPolicy("delete:printrequest", policy => policy.Requirements.Add(new HasScopeRequirement("delete:printrequest", $"https://{builder.Configuration["Auth0:Domain"]}/")));
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

builder.Services.AddControllers()
     .AddNewtonsoftJson(options =>
     {
         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDefaultFiles();

    app.UseStaticFiles();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
