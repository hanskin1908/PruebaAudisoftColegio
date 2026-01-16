using Aplicacion.Servicios;
using Infraestructura.Configuracion;

var constructor = WebApplication.CreateBuilder(args);

constructor.Services.AddControllers();
constructor.Services.AddEndpointsApiExplorer();
constructor.Services.AddSwaggerGen();
constructor.Services.AgregarAplicacion();
constructor.Services.AgregarInfraestructura(constructor.Configuration);

var aplicacion = constructor.Build();

if (aplicacion.Environment.IsDevelopment())
{
    aplicacion.UseSwagger();
    aplicacion.UseSwaggerUI();
}

aplicacion.UseHttpsRedirection();

aplicacion.UseAuthorization();

aplicacion.MapControllers();

aplicacion.Run();
