using Microsoft.EntityFrameworkCore;
using SGRentalsAPI.Data;
using SGRentalsAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddScoped<EmpresaService>();
builder.Services.AddScoped<UsuarioService>();

// --- CONFIGURAÇÃO DE CORS ---
// Adiciona um serviço de CORS que permite requisições de qualquer origem, método e cabeçalho.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Converte enums para strings (ex: PerfilUsuario.Administrador se torna "Administrador")
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // Ignora ciclos de referência para evitar erros de serialização (empresa -> sócio -> empresa)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Configuração do Swagger/OpenAPI para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Aplica migrações do banco de dados automaticamente durante o desenvolvimento.
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

// --- UTILIZAÇÃO DE CORS ---
// Habilita o uso da política de CORS que permite todas as origens.
app.UseCors("AllowAllOrigins");

app.UseAuthorization();
app.MapControllers();

app.Run();  