using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TODO_V2.Shared.Models;

namespace TODO_V2.Client.Data
{
    public static class CategoryDictionary
    {
        public static Dictionary<int, string> categoryDictionary = new Dictionary<int, string>();

        public static async Task LoadCategoryDictionary(HttpClient http)
        {
            try
            {
                var categories = await http.GetFromJsonAsync<List<Category>>("api/Category");
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        if (!categoryDictionary.ContainsKey(category.Id))
                        {
                            categoryDictionary.Add(category.Id, category.Name);
                        }                       
                    }
                }
                PrintDictionary();
            }
            catch (Exception ex)
            {
                // Debug.WriteLine($"Error loading category dictionary: {ex.Message}");
            }
        }


        public static string GetCategoryName(int categoryId)
        {
            if (categoryDictionary.ContainsKey(categoryId))
            {
                return categoryDictionary[categoryId];
            }
            else
            {
                return "Categoría Desconocida";
            }
        }

        public static void PrintDictionary()
        {
            // Debug.WriteLine("Category Dictionary Contents:");
            foreach (var kvp in categoryDictionary)
            {
                // Debug.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            }
        }
    }
}
