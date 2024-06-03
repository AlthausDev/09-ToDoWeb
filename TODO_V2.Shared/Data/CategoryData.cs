using System.Net.Http.Json;
using TODO_V2.Shared.Models;

namespace TODO_V2.Shared.Data
{
    public class CategoryData
    {
        public static Category[] Categories { get; set; }

        public static async Task LoadTestCategories(HttpClient http)
        {
            Categories = [

                new("Informe"),
                new("Presentación"),
                new("Reunión"),
                new("Correo electrónico"),
                new("Llamadas de seguimiento"),
                new("Sitio web"),
                new("Pruebas de software"),
                new("Lista de verificación")
            ];

            foreach (Category category in Categories)
            {
                await http.PostAsJsonAsync("api/Category", category);
            }
        }
    }
}
