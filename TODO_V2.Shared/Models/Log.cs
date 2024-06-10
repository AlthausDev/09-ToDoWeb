using System;

namespace TODO_V2.Shared.Models

{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string? Logger { get; set; }
        public string Message { get; set; }
        public string MachineName { get; set; }
        public string? Request_method { get; set; }
        public string? Stacktrace { get; set; }
        public string? File_name { get; set; }
        public string? All_event_properties { get; set; }
        public string? Log_exception { get; set; }

        public Log() { }

        public Log(DateTime logged, string level, string? logger, string message, string machineName, string? request_method, string? stacktrace, string? file_name, string? all_event_properties, string? log_exception)
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
            Log_exception = log_exception;
        }

        public Log(int id, DateTime logged, string level, string? logger, string message, string machineName, string? request_method, string? stacktrace, string? file_name, string? all_event_properties, string? log_exception)
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
            Log_exception = log_exception;
        }
    }
}
