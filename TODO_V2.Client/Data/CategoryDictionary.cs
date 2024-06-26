﻿using System.Net.Http.Json;
using TODO_V2.Shared.Models;

namespace TODO_V2.Client.Data
{
    public static class CategoryDictionary
    {
        public static Dictionary<int, string> categoryDictionary = new();

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
            }
            catch (Exception)
            {
                // Debug.WriteLine($"Error loading category dictionary: {ex.Message}");
            }
        }


        public static string GetCategoryName(int categoryId)
        {
            return categoryDictionary.ContainsKey(categoryId) ? categoryDictionary[categoryId] : "Categoría Desconocida";
        }      
    }
}
