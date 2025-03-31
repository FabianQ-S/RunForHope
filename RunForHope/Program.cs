using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunForHope_️.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos para Identity (SQL Server)
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

// Configuración de la base de datos para Participantes (SQLite)
var participantesConnectionString = builder.Configuration.GetConnectionString("ParticipantesContext")
    ?? throw new InvalidOperationException("Connection string 'ParticipantesContext' not found.");
builder.Services.AddDbContext<ParticipantesContext>(options =>
    options.UseSqlite(participantesConnectionString));

// Configuración de Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Requiere confirmación de cuenta
    options.Password.RequireDigit = true; // La contraseña requiere un dígito
    options.Password.RequireLowercase = true; // La contraseña requiere una minúscula
    options.Password.RequireUppercase = true; // La contraseña requiere una mayúscula
    options.Password.RequiredLength = 8; // Longitud mínima de la contraseña
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configuración de servicios adicionales
builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Filtro de excepciones para desarrollo

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint(); // Habilita el endpoint de migraciones en desarrollo
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Manejo de errores en producción
    app.UseHsts(); // Habilita HSTS (HTTP Strict Transport Security)
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Habilita archivos estáticos (CSS, JS, imágenes)
app.UseRouting();

app.UseAuthentication(); // Habilita la autenticación
app.UseAuthorization(); // Habilita la autorización

// Configuración de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Habilita las páginas Razor (Identity)

// Aplicar migraciones pendientes al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Aplicar migraciones para ApplicationDbContext (Identity)
        var identityContext = services.GetRequiredService<ApplicationDbContext>();
        identityContext.Database.Migrate();

        // Aplicar migraciones para ParticipantesContext
        var participantesContext = services.GetRequiredService<ParticipantesContext>();
        participantesContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar las migraciones.");
    }
}

app.Run();