using System;
using System.Net.Http;
using System.Net.Http.Json;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;

namespace BlazorWebPage.Shared.Data
{
    public class TaskItemData
    {
        public static TaskItem[] Tasks { get; set; }

        public static async Task LoadTestTasks(HttpClient http)
        {
            Tasks = [
                new(1, 1, TaskStateEnum.Pendiente, "Completar informe", new DateOnly(2024, 5, 28)),
                new(2, 2, TaskStateEnum.Pendiente, "Revisar presentación", new DateOnly(2024, 5, 30)),
                new(3, 3, TaskStateEnum.Pendiente, "Preparar para la reunión", new DateOnly(2024, 6, 1)),
                new(4, 4, TaskStateEnum.Pendiente, "Enviar correo electrónico", new DateOnly(2024, 6, 5)),
                new(5, 5, TaskStateEnum.Pendiente, "Hacer llamadas de seguimiento", new DateOnly(2024, 6, 10)),
                new(6, 6, TaskStateEnum.Pendiente, "Actualizar sitio web", new DateOnly(2024, 6, 15)),
                new(7, 7, TaskStateEnum.Pendiente, "Realizar pruebas de software", new DateOnly(2024, 6, 20)),
                new(8, 8, TaskStateEnum.Pendiente, "Preparar lista de verificación", new DateOnly(2024, 6, 25)),
                new(9, 9, TaskStateEnum.Pendiente, "Investigar nuevos productos", new DateOnly(2024, 6, 28)),
                new(10, 10, TaskStateEnum.Pendiente, "Redactar propuesta de proyecto", new DateOnly(2024, 7, 1)),
                new(1, 11, TaskStateEnum.Pendiente, "Limpiar el área de trabajo", new DateOnly(2024, 7, 5)),
                new(2, 12, TaskStateEnum.Pendiente, "Organizar archivos", new DateOnly(2024, 7, 10)),
                new(3, 13, TaskStateEnum.Pendiente, "Resolver problemas de cliente", new DateOnly(2024, 7, 15)),
                new(4, 14, TaskStateEnum.Pendiente, "Planificar el próximo trimestre", new DateOnly(2024, 7, 20)),
                new(5, 15, TaskStateEnum.Pendiente, "Entrenar al nuevo empleado", new DateOnly(2024, 7, 25)),
                new(6, 16, TaskStateEnum.Pendiente, "Actualizar políticas de la empresa", new DateOnly(2024, 7, 28)),
                new(7, 17, TaskStateEnum.Pendiente, "Investigar tendencias del mercado", new DateOnly(2024, 8, 1)),
                new(8, 18, TaskStateEnum.Pendiente, "Realizar evaluaciones de desempeño", new DateOnly(2024, 8, 5)),
                new(9, 19, TaskStateEnum.Pendiente, "Crear informe de gastos", new DateOnly(2024, 8, 10)),
                new(10, 20, TaskStateEnum.Pendiente, "Revisar políticas de seguridad", new DateOnly(2024, 8, 15))
            ];

            foreach (TaskItem taskItem in Tasks)
            {
                await http.PostAsJsonAsync("tasks", taskItem);
            }
        }
    }
}
