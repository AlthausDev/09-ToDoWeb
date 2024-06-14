using System;

namespace TODO_V2.Shared.Models

{
    public class Log
    {
        /// <summary>
        /// Identificador único del log.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Fecha y hora en que se registró el log.
        /// </summary>
        public DateTime Logged { get; set; }

        /// <summary>
        /// Nivel de log (e.g., Info, Debug, Error).
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Nombre del logger que generó el log. Puede ser nulo.
        /// </summary>
        public string? Logger { get; set; }

        /// <summary>
        /// Mensaje del log.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Nombre de la máquina donde se generó el log.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Método de la solicitud que generó el log. Puede ser nulo.
        /// </summary>
        public string? Request_method { get; set; }

        /// <summary>
        /// Traza de la pila en caso de una excepción. Puede ser nula.
        /// </summary>
        public string? Stacktrace { get; set; }

        /// <summary>
        /// Nombre del archivo asociado con el log. Puede ser nulo.
        /// </summary>
        public string? File_name { get; set; }

        /// <summary>
        /// Propiedades adicionales del evento en formato JSON. Puede ser nulo.
        /// </summary>
        public string? All_event_properties { get; set; }

        /// <summary>
        /// Código de estado HTTP asociado con la solicitud que generó el log. Puede ser nulo.
        /// </summary>
        public string? Status_code { get; set; }


        public string? Origin { get; set; }

        public Log() { }

        public Log(DateTime logged, string level, string? logger, string message, string machineName, string? request_method, string? stacktrace, string? file_name, string? all_event_properties, string? status_code, string? origin)
        {
            Logged = logged;
            Level = level;
            Logger = logger;
            Message = message;
            MachineName = machineName;
            Request_method = request_method;
            Stacktrace = stacktrace;
            File_name = file_name;
            All_event_properties = all_event_properties;
            Status_code = status_code;
            Origin = origin;

        }

        public Log(int id, DateTime logged, string level, string? logger, string message, string machineName, string? request_method, string? stacktrace, string? file_name, string? all_event_properties, string? status_code, string? origin)
        {
            Id = id;
            Logged = logged;
            Level = level;
            Logger = logger;
            Message = message;
            MachineName = machineName;
            Request_method = request_method;
            Stacktrace = stacktrace;
            File_name = file_name;
            All_event_properties = all_event_properties;
            Status_code = status_code;
            Origin = origin;
        }
    }
}
