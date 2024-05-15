using TODO_V2.Client.Pages;
using TODO_V2.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TODO_V2.Server.Repository.Impl;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Utils;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Server.Services.Impl;
using TODO_V2.Shared.Models;
using BlazorBootstrap;
using TODO_V2.Server.Controllers.Impl;
using TODO_V2.Server.Controllers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuración de JWT desde appsettings.json
var jwtKey = builder.Configuration["JWT:Key"];
var jwtIssuer = builder.Configuration["JWT:Issuer"];
var jwtAudience = builder.Configuration["JWT:Audience"];
var jwtExpirationHours = int.Parse(builder.Configuration["JWT:ExpirationHours"]);

// Añadir servicios.
//builder.Services.AddScoped<AuthenticationService>();
//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddMvc();
//builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddAuthorizationCore();
builder.Services.AddBlazorBootstrap();
builder.Services.AddHttpClient();

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

//builder.Services.AddHttpClient<UserService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7216/api/");
//});

builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7216/");
});


builder.Services.AddAuthentication()
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Unauthorized/";
            options.AccessDeniedPath = "/Account/Forbidden/";
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

// Configurar Dependencias
//builder.Services.AddDbContext<UserContext>();
//builder.Services.AddDbContext<ChoreContext>();

builder.Services.AddTransient<IChoreRepository, ChoreRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IChoreService, ChoreService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<EncryptionUtil> ();


// Configuración de CORS para admitir cualquier origen
builder.Services.AddCors(options => options.AddPolicy("corsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseCors("corsPolicy");
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TODO_V2.Client._Imports).Assembly);

app.Run();
