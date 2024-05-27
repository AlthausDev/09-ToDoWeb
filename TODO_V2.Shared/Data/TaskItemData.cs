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
                new(1, 1, 1, "Completar informe", new DateTime(2024, 5, 28)),
                new(2, 2, 1, "Revisar presentación", new DateTime(2024, 5, 30)),
                new(3, 3, 1, "Preparar para la reunión", new DateTime(2024, 6, 1)),
                new(4, 4, 1, "Enviar correo electrónico", new DateTime(2024, 6, 5)),
                new(5, 5, 1, "Hacer llamadas de seguimiento", new DateTime(2024, 6, 10)),
                new(6, 6, 1, "Actualizar sitio web", new DateTime(2024, 6, 15)),
                new(7, 7, 1, "Realizar pruebas de software", new DateTime(2024, 6, 20)),
                new(8, 8, 1, "Preparar lista de verificación", new DateTime(2024, 6, 25)),
                new(3, 9, 1, "Investigar nuevos productos", new DateTime(2024, 6, 28)),
                new(5, 10, 1, "Redactar propuesta de proyecto", new DateTime(2024, 7, 1)),
                new(1, 11, 1, "Limpiar el área de trabajo", new DateTime(2024, 7, 5)),
                new(2, 12, 1, "Organizar archivos", new DateTime(2024, 7, 10)),
                new(3, 13, 1, "Resolver problemas de cliente", new DateTime(2024, 7, 15)),
                new(4, 14, 1, "Planificar el próximo trimestre", new DateTime(2024, 7, 20)),
                new(5, 15, 1, "Entrenar al nuevo empleado", new DateTime(2024, 7, 25)),
                new(6, 16, 1, "Actualizar políticas de la empresa", new DateTime(2024, 7, 28)),
                new(7, 17, 1, "Investigar tendencias del mercado", new DateTime(2024, 8, 1)),
                new(8, 18, 1, "Realizar evaluaciones de desempeño", new DateTime(2024, 8, 5)),
                new(9, 19, 1, "Crear informe de gastos", new DateTime(2024, 8, 10)),
                new(10, 20, 1, "Revisar políticas de seguridad", new DateTime(2024, 8, 15)),
                new(3, 3, 1, "Enviar correos electrónicos de seguimiento", new DateTime(2024, 6, 5)),
                new(4, 4, 1, "Planificar reuniones con clientes", new DateTime(2024, 6, 10)),
                new(5, 5, 1, "Actualizar la base de datos del cliente", new DateTime(2024, 6, 15)),
                new(6, 6, 1, "Preparar informe de ventas", new DateTime(2024, 6, 20)),
                new(7, 7, 1, "Revisar contratos de proveedores", new DateTime(2024, 6, 25)),
                new(8, 8, 1, "Crear presentación para el equipo de ventas", new DateTime(2024, 6, 28)),
                new(1, 9, 1, "Hacer investigación de mercado", new DateTime(2024, 7, 1)),
                new(2, 10, 1, "Actualizar software interno", new DateTime(2024, 7, 5)),
                new(3, 11, 1, "Realizar llamadas de ventas", new DateTime(2024, 7, 10)),
                new(4, 12, 1, "Preparar material promocional", new DateTime(2024, 7, 15)),
                new(5, 13, 1, "Entrenamiento del equipo de soporte", new DateTime(2024, 7, 20)),
                new(6, 14, 1, "Preparar informe de marketing", new DateTime(2024, 7, 25)),
                new(7, 15, 1, "Actualizar contenido del sitio web", new DateTime(2024, 7, 28)),
                new(8, 16, 1, "Investigar nuevas oportunidades de negocio", new DateTime(2024, 8, 1)),
                new(1, 17, 1, "Entrenamiento del personal de recursos humanos", new DateTime(2024, 8, 5)),
                new(2, 18, 1, "Preparar informe financiero trimestral", new DateTime(2024, 8, 10)),
                new(3, 19, 1, "Desarrollar estrategias de marketing digital", new DateTime(2024, 8, 15)),
                new(4, 20, 1, "Revisar políticas de seguridad informática", new DateTime(2024, 8, 20)),
                new(5, 21, 1, "Actualizar sistemas de gestión interna", new DateTime(2024, 8, 25)),
                new(6, 1, 1, "Organizar evento de lanzamiento de producto", new DateTime(2024, 8, 30)),
                new(7, 2, 1, "Realizar encuesta de satisfacción del cliente", new DateTime(2024, 9, 5)),
                new(8, 3, 1, "Preparar material para feria comercial", new DateTime(2024, 9, 10)),
                new(1, 4, 1, "Entrenar al equipo de desarrollo", new DateTime(2024, 9, 15)),
                new(2, 5, 1, "Revisar procesos de entrega de productos", new DateTime(2024, 9, 20)),
                new(3, 6, 1, "Actualizar políticas de calidad", new DateTime(2024, 9, 25)),
                new(4, 7, 1, "Realizar auditoría interna", new DateTime(2024, 9, 30)),
                new(5, 8, 1, "Planificar capacitación de liderazgo", new DateTime(2024, 10, 5)),
                new(6, 9, 1, "Investigar nuevas herramientas de productividad", new DateTime(2024, 10, 10)),
                new(7, 10, 1, "Desarrollar plan de comunicación interna", new DateTime(2024, 10, 15)),
                new(8, 11, 1, "Preparar informe de progreso del proyecto", new DateTime(2024, 10, 20)),
                new(1, 12, 1, "Realizar reunión de retroalimentación del equipo", new DateTime(2024, 10, 25)),
                new(2, 13, 1, "Actualizar políticas de teletrabajo", new DateTime(2024, 10, 30)),
                new(3, 14, 1, "Entrenar al equipo de ventas", new DateTime(2024, 11, 5)),
                new(4, 15, 1, "Preparar informe de tendencias del mercado", new DateTime(2024, 11, 10)),
                new(5, 16, 1, "Realizar pruebas de usabilidad del sitio web", new DateTime(2024, 11, 15)),
                new(6, 17, 1, "Entrevistar a candidatos para nuevas contrataciones", new DateTime(2024, 11, 20)),
                new(7, 18, 1, "Preparar material para evento de networking", new DateTime(2024, 11, 25)),
                new(8, 19, 1, "Actualizar políticas de protección de datos", new DateTime(2024, 11, 30)),
                new(1, 20, 1, "Revisar estrategias de retención de clientes", new DateTime(2024, 12, 5)),
                new(2, 21, 1, "Desarrollar programa de incentivos para empleados", new DateTime(2024, 12, 10)),
                new(3, 1, 1, "Realizar análisis de satisfacción del empleado", new DateTime(2024, 12, 15)),
                new(4, 2, 1, "Entrenar al equipo de atención al cliente", new DateTime(2024, 12, 20)),
                new(5, 3, 1, "Planificar eventos de integración del equipo", new DateTime(2024, 12, 25)),
                new(6, 4, 1, "Preparar material para conferencia sectorial", new DateTime(2024, 12, 30)),
                new(7, 5, 1, "Entrenamiento en técnicas de negociación", new DateTime(2025, 1, 5)),
                new(8, 6, 1, "Actualizar políticas de igualdad de género", new DateTime(2025, 1, 10)),
                new(1, 7, 1, "Realizar análisis de competencia", new DateTime(2025, 1, 15)),
                new(2, 8, 1, "Entrenar al equipo de recursos humanos", new DateTime(2025, 1, 20)),
                new(3, 9, 1, "Desarrollar estrategias de retención de talento", new DateTime(2025, 1, 25)),
                new(4, 10, 1, "Preparar material para feria de empleo", new DateTime(2025, 1, 30)),
                new(5, 11, 1, "Revisar políticas de reclutamiento", new DateTime(2025, 2, 5)),
                new(6, 12, 1, "Entrenamiento en habilidades de liderazgo", new DateTime(2025, 2, 10)),
                new(7, 13, 1, "Realizar análisis de riesgos empresariales", new DateTime(2025, 2, 15)),
                new(8, 14, 1, "Desarrollar programa de bienestar laboral", new DateTime(2025, 2, 20)),
                new(1, 15, 1, "Planificar actividades de team building", new DateTime(2025, 2, 25)),
                new(2, 16, 1, "Entrenar al equipo de marketing digital", new DateTime(2025, 2, 28)),
                new(3, 17, 1, "Preparar material para seminario web", new DateTime(2025, 3, 5)),
                new(4, 18, 1, "Revisar procesos de gestión de proyectos", new DateTime(2025, 3, 10)),
                new(5, 19, 1, "Actualizar políticas de formación continua", new DateTime(2025, 3, 15)),
                new(6, 20, 1, "Desarrollar estrategias de fidelización de clientes", new DateTime(2025, 3, 20)),
                new(7, 21, 1, "Entrenar al equipo de ventas en técnicas de cierre", new DateTime(2025, 3, 25)),
                new(8, 1, 1, "Planificar eventos corporativos", new DateTime(2025, 3, 30)),
                new(1, 2, 1, "Revisar políticas de reembolso de gastos", new DateTime(2025, 4, 5)),
                new(2, 3, 1, "Entrenamiento en resolución de conflictos", new DateTime(2025, 4, 10)),
                new(3, 4, 1, "Realizar análisis de costos operativos", new DateTime(2025, 4, 15)),
                new(4, 5, 1, "Preparar informe de proyecciones financieras", new DateTime(2025, 4, 20)),
                new(5, 6, 1, "Desarrollar estrategias de expansión de mercado", new DateTime(2025, 4, 25)),
                new(6, 7, 1, "Entrenamiento en gestión del tiempo", new DateTime(2025, 4, 30)),
                new(7, 8, 1, "Realizar análisis de satisfacción del cliente", new DateTime(2025, 5, 5)),
                new(8, 9, 1, "Planificar estrategias de marketing de contenidos", new DateTime(2025, 5, 10)),
                new(1, 10, 1, "Desarrollar programa de mentoring", new DateTime(2025, 5, 15)),
                new(2, 11, 1, "Preparar material para evento de lanzamiento", new DateTime(2025, 5, 20)),
                new(3, 12, 1, "Entrenamiento en habilidades de presentación", new DateTime(2025, 5, 25)),
                new(4, 13, 1, "Actualizar políticas de prevención de riesgos laborales", new DateTime(2025, 5, 30)),
                new(5, 14, 1, "Realizar análisis de satisfacción del empleado", new DateTime(2025, 6, 5)),
                new(6, 15, 1, "Preparar material para feria sectorial", new DateTime(2025, 6, 10)),
                new(7, 16, 1, "Entrenamiento en liderazgo situacional", new DateTime(2025, 6, 15)),
                new(8, 17, 1, "Planificar actividades de integración del equipo", new DateTime(2025, 6, 20)),
                new(1, 18, 1, "Revisar políticas de gestión de calidad", new DateTime(2025, 6, 25)),
                new(2, 19, 1, "Entrenamiento en gestión del cambio", new DateTime(2025, 6, 30)),
                new(3, 20, 1, "Realizar análisis de mercado", new DateTime(2025, 7, 5)),
                new(4, 21, 1, "Desarrollar estrategias de branding", new DateTime(2025, 7, 10))
            ];

            foreach (TaskItem taskItem in Tasks)
            {
                await http.PostAsJsonAsync("api/TaskItem", taskItem);
            }
        }
    }
}
