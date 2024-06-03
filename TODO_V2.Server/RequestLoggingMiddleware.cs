using NLog;
using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            var sw = Stopwatch.StartNew();

            try
            {
                _logger.Info($"Request: {context.Request.Method} {context.Request.Path}");
                await _next(context);
                _logger.Info($"Response: {context.Response.StatusCode} - {GetStatusCodeDescription(context.Response.StatusCode)}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception occurred while processing the request.");
                throw;
            }
            finally
            {
                sw.Stop();
                _logger.Info($"Request duration: {sw.Elapsed.TotalMilliseconds} ms\n");
            }
        }
        else
        {
            await _next(context);
        }
    }

    private string GetStatusCodeDescription(int statusCode)
    {
        return statusCode switch
        {
            100 => "Continuar",
            101 => "Cambiando Protocolos",
            200 => "OK",
            201 => "Creado",
            202 => "Aceptado",
            203 => "Información No Autorizada",
            204 => "Sin Contenido",
            205 => "Contenido Reiniciado",
            206 => "Contenido Parcial",
            300 => "Múltiples Opciones",
            301 => "Movido Permanentemente",
            302 => "Encontrado",
            303 => "Ver Otro",
            304 => "No Modificado",
            305 => "Usar Proxy",
            307 => "Redirección Temporal",
            308 => "Redirección Permanente",
            400 => "Solicitud Incorrecta",
            401 => "No Autorizado",
            402 => "Pago Requerido",
            403 => "Prohibido",
            404 => "No Encontrado",
            405 => "Método No Permitido",
            406 => "No Aceptable",
            407 => "Autenticación de Proxy Requerida",
            408 => "Tiempo de Solicitud Agotado",
            409 => "Conflicto",
            410 => "Ya no está Disponible",
            411 => "Longitud Requerida",
            412 => "Falló la Precondición",
            413 => "Solicitud Demasiado Grande",
            414 => "URI Demasiado Largo",
            415 => "Tipo de Medios No Soportado",
            416 => "Rango No Satisfactorio",
            417 => "Expectativa Fallida",
            426 => "Requiere Actualización",
            428 => "Precondición Requerida",
            429 => "Demasiadas Solicitudes",
            431 => "Campos de Encabezado de Solicitud Demasiado Grandes",
            500 => "Error Interno del Servidor",
            501 => "No Implementado",
            502 => "Puerta de Enlace Incorrecta",
            503 => "Servicio No Disponible",
            504 => "Tiempo de Espera de la Puerta de Enlace Expirado",
            505 => "Versión de HTTP No Soportada",
            511 => "Autenticación de Red Requerida",
            _ => $"Código de Estado Desconocido: {statusCode}",
        };
    }
}
