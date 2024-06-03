using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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
                //_logger.Info($"Response: {context.Response.StatusCode}");
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
        switch (statusCode)
        {
            case 100: return "Continuar";
            case 101: return "Cambiando Protocolos";
            case 200: return "OK";
            case 201: return "Creado";
            case 202: return "Aceptado";
            case 203: return "Información No Autorizada";
            case 204: return "Sin Contenido";
            case 205: return "Contenido Reiniciado";
            case 206: return "Contenido Parcial";
            case 300: return "Múltiples Opciones";
            case 301: return "Movido Permanentemente";
            case 302: return "Encontrado";
            case 303: return "Ver Otro";
            case 304: return "No Modificado";
            case 305: return "Usar Proxy";
            case 307: return "Redirección Temporal";
            case 308: return "Redirección Permanente";
            case 400: return "Solicitud Incorrecta";
            case 401: return "No Autorizado";
            case 402: return "Pago Requerido";
            case 403: return "Prohibido";
            case 404: return "No Encontrado";
            case 405: return "Método No Permitido";
            case 406: return "No Aceptable";
            case 407: return "Autenticación de Proxy Requerida";
            case 408: return "Tiempo de Solicitud Agotado";
            case 409: return "Conflicto";
            case 410: return "Ya no está Disponible";
            case 411: return "Longitud Requerida";
            case 412: return "Falló la Precondición";
            case 413: return "Solicitud Demasiado Grande";
            case 414: return "URI Demasiado Largo";
            case 415: return "Tipo de Medios No Soportado";
            case 416: return "Rango No Satisfactorio";
            case 417: return "Expectativa Fallida";
            case 426: return "Requiere Actualización";
            case 428: return "Precondición Requerida";
            case 429: return "Demasiadas Solicitudes";
            case 431: return "Campos de Encabezado de Solicitud Demasiado Grandes";
            case 500: return "Error Interno del Servidor";
            case 501: return "No Implementado";
            case 502: return "Puerta de Enlace Incorrecta";
            case 503: return "Servicio No Disponible";
            case 504: return "Tiempo de Espera de la Puerta de Enlace Expirado";
            case 505: return "Versión de HTTP No Soportada";
            case 511: return "Autenticación de Red Requerida";
            default: return $"Código de Estado Desconocido: {statusCode}";
        }
    }
}
