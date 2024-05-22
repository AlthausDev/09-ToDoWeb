using System;
using System.Net.Http;
using System.Net.Http.Json;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;

namespace TODO_V2.Shared.Data
{
    public class TaskItemData
    {
        public static TaskItem[] Tasks { get; set; }

        public static async Task LoadTestTasks(HttpClient http)
        {
            Tasks = [
                new(1, 1, TaskStateEnum.Pendiente.ToString(), "Completar informe", new DateTime(2024, 5, 28)),
                new(2, 2, TaskStateEnum.Pendiente.ToString(), "Revisar presentación", new DateTime(2024, 5, 30)),
                new(3, 3, TaskStateEnum.Pendiente.ToString(), "Preparar para la reunión", new DateTime(2024, 6, 1)),
                new(4, 4, TaskStateEnum.Pendiente.ToString(), "Enviar correo electrónico", new DateTime(2024, 6, 5)),
                new(5, 5, TaskStateEnum.Pendiente.ToString(), "Hacer llamadas de seguimiento", new DateTime(2024, 6, 10)),
                new(6, 6, TaskStateEnum.Pendiente.ToString(), "Actualizar sitio web", new DateTime(2024, 6, 15)),
                new(7, 7, TaskStateEnum.Pendiente.ToString(), "Realizar pruebas de software", new DateTime(2024, 6, 20)),
                new(8, 8, TaskStateEnum.Pendiente.ToString(), "Preparar lista de verificación", new DateTime(2024, 6, 25)),
                new(9, 9, TaskStateEnum.Pendiente.ToString(), "Investigar nuevos productos", new DateTime(2024, 6, 28)),
                new(10, 10, TaskStateEnum.Pendiente.ToString(), "Redactar propuesta de proyecto", new DateTime(2024, 7, 1)),
                new(1, 11, TaskStateEnum.Pendiente.ToString(), "Limpiar el área de trabajo", new DateTime(2024, 7, 5)),
                new(2, 12, TaskStateEnum.Pendiente.ToString(), "Organizar archivos", new DateTime(2024, 7, 10)),
                new(3, 13, TaskStateEnum.Pendiente.ToString(), "Resolver problemas de cliente", new DateTime(2024, 7, 15)),
                new(4, 14, TaskStateEnum.Pendiente.ToString(), "Planificar el próximo trimestre", new DateTime(2024, 7, 20)),
                new(5, 15, TaskStateEnum.Pendiente.ToString(), "Entrenar al nuevo empleado", new DateTime(2024, 7, 25)),
                new(6, 16, TaskStateEnum.Pendiente.ToString(), "Actualizar políticas de la empresa", new DateTime(2024, 7, 28)),
                new(7, 17, TaskStateEnum.Pendiente.ToString(), "Investigar tendencias del mercado", new DateTime(2024, 8, 1)),
                new(8, 18, TaskStateEnum.Pendiente.ToString(), "Realizar evaluaciones de desempeño", new DateTime(2024, 8, 5)),
                new(9, 19, TaskStateEnum.Pendiente.ToString(), "Crear informe de gastos", new DateTime(2024, 8, 10)),
                new(10, 20, TaskStateEnum.Pendiente.ToString(), "Revisar políticas de seguridad", new DateTime(2024, 8, 15))
            ];

            foreach (TaskItem taskItem in Tasks)
            {
                await http.PostAsJsonAsync("tasks", taskItem);
            }
        }
    }
}
