using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Text;
using TODO_V2.Server.Components;
using TODO_V2.Server.Repository.Impl;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Services.Impl;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Server.Utils;

//var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLog();

#region Configuración de JWT
var jwtKey = builder.Configuration["JWT:Key"];
var jwtIssuer = builder.Configuration["JWT:Issuer"];
var jwtAudience = builder.Configuration["JWT:Audience"];
var jwtExpirationHours = int.Parse(builder.Configuration["JWT:ExpirationHours"]);
#endregion

#region Añadir servicios
builder.Services.AddBlazorBootstrap();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
#endregion

#region Configuración de CORS
builder.Services.AddCors(options => options.AddPolicy("corsPolicy", builder =>
{
    _ = builder.AllowAnyMethod()
           .AllowAnyHeader()
           .SetIsOriginAllowed(origin => true)
           .AllowAnyOrigin();

}));
#endregion

#region Configura componentes de Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(client => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7216/"),
});
#endregion

#region Configura autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Account/Unauthorized/";
    options.AccessDeniedPath = "/Account/Forbidden/";
}).AddJwtBearer(options =>
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
#endregion

#region Configura repositorios y servicios
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ITaskItemService, TaskItemService>();
builder.Services.AddTransient<EncryptionUtil>();
#endregion

var app = builder.Build();

#region Configuración del entorno
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    _ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
    _ = app.UseHsts();
}
#endregion

#region Configuración de middleware
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("corsPolicy");
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    logger.Info($"Handling request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
    logger.Info($"Finished handling request: {context.Request.Method} {context.Request.Path}");
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseStaticFiles();
app.UseAntiforgery();
#endregion

#region Mapea páginas y controladores
app.MapRazorPages();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TODO_V2.Client._Imports).Assembly);
#endregion

app.Run();
