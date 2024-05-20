using TODO_V2.Server.Components;
using Blazored.LocalStorage;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TODO_V2.Server.Repository.Impl;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Utils;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Server.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

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
#endregion

#region Configuración de CORS
builder.Services.AddCors(options => options.AddPolicy("corsPolicy", builder =>
{
    builder.AllowAnyMethod()
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
    BaseAddress = new Uri("https://localhost:7216/")
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
builder.Services.AddTransient<IUserService, UserService>();
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
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
#endregion

#region Configuración de middleware
app.UseCors("corsPolicy");
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
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
