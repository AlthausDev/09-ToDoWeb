using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using TODO_V2.Client.Data;
using TODO_V2.Client.Modals;
using TODO_V2.Client.Pages;
using TODO_V2.Shared.Models;

namespace TODO_V2.Client.Components
{
    public partial class CategoryMannager
    {
        [Parameter]
        public string Id { get; set; }

        private User User { get; set; }

        private Modal ModalInstance = default!;
        private ConfirmDialog dialog = default!;

        [Inject] private ToastService ToastService { get; set; } = default!;
        [Inject] protected PreloadService PreloadService { get; set; }

        private List<ToastMessage> messages = new();
        private Grid<Category> DataGrid = default!;
        private ObservableCollection<Category> CategoryList { get; set; } = new ObservableCollection<Category>();

        private Category? selectedCategory { get; set; } = null;
        public Category? SelectedCategory
        {
            get
            {
                return selectedCategory;
            }
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                }
            }
        }

        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;
                PreloadService.Show(SpinnerColor.Light, "Cargando...");

                await CategoryDictionary.LoadCategoryDictionary(Http);
                await GetCategoryData();
            }
            finally
            {
                isLoading = false;
                await Task.Delay(1000);
                PreloadService.Hide();
            }
        }

        private RenderFragment RenderLoadingIndicator() => builder =>
        {
            if (isLoading)
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "loading-indicator");
                builder.AddContent(2, "Cargando...");
                builder.CloseElement();
            }
        };


        #region StartUp       

        private async Task<GridDataProviderResult<Category>> CategoryDataProvider(GridDataProviderRequest<Category> request)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            while (CategoryList.IsNullOrEmpty())
            {
                await Task.Delay(5);
            }

            stopwatch.Stop();
            Debug.WriteLine($"Tiempo total de espera: {stopwatch.ElapsedMilliseconds} ms");

            return await Task.FromResult(request.ApplyTo(CategoryList.OrderBy(Category => Category.Id)));
        }


        private async Task GetCategoryData()
        {
            CategoryList = new ObservableCollection<Category>(await Http.GetFromJsonAsync<List<Category>>($"api/Category"));
        }
        #endregion

        #region SelectRow    
        private async Task SelectCategory(GridRowEventArgs<Category> args)
        {
            SelectedCategory = args.Item;

            Debug.WriteLine(SelectedCategory.Id);
            Debug.WriteLine(SelectedCategory.Name);
        }

        #endregion SelectRow        

        #region Modal  
        #region Category Item Form    

        private async Task OnClickCategoryForm(Category? category)
        {
            SelectedCategory = category;
            var parameters = new Dictionary<string, object>
             {
                { "CategoryId", SelectedCategory?.Id },
                { "CategoryName", SelectedCategory?.Name},
                { "Aceptar", EventCallback.Factory.Create<MouseEventArgs>(this, CategoryFormResult) },
                { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, AdminPanel.HideModal) }
            };
            await AdminPanel.ModalInstance.ShowAsync<ModalCategory>(title: "Categorias", parameters: parameters);
        }


        private async Task CategoryFormResult()
        {
            ShowMessage(ToastType.Success, "Se ha creado exitosamente la categoría");
            await AdminPanel.HideModal();
            await GetCategoryData();
            await DataGrid.RefreshDataAsync();
        }
        #endregion

        #endregion Modal       

        #region Delete
        private async Task DeleteCategoryAsync(Category category)
        {
            SelectedCategory = category;

            var parameters = new Dictionary<string, object?>
    {
        { "Name", SelectedCategory.Name },
        { "Message", "esta categoria" }
    };

            var options = new ConfirmDialogOptions
            {
                YesButtonColor = ButtonColor.Danger,
                YesButtonText = "Eliminar",
                NoButtonText = "Cancelar",
                IsVerticallyCentered = true,
                Dismissable = true
            };

            var response = await dialog.ShowAsync<ModalDelete>(
                title: "Confirmar Eliminación",
                parameters,
                confirmDialogOptions: options);

            if (response)
            {
                await ConfirmAndDeleteCategoryAsync(SelectedCategory.Id);
            }
            else
            {
                ToastService.Notify(new ToastMessage(ToastType.Secondary, "Acción de eliminación cancelada."));
            }
        }

        private async Task ConfirmAndDeleteCategoryAsync(int categoryId)
        {
            try
            {
                if (await HasAssociatedTasksAsync(categoryId))
                {
                    ShowMessage(ToastType.Danger, "Error: No se puede eliminar una categoría que tenga tareas asociadas.");
                    return;
                }

                _ = await Http.DeleteAsync($"api/Category/{categoryId}");
                await GetCategoryData();
                await DataGrid.RefreshDataAsync();
                ShowMessage(ToastType.Success, "Categoría eliminada con éxito.");
            }
            catch (Exception ex)
            {
                ShowMessage(ToastType.Danger, $"Error al eliminar la categoría: {ex.Message}");
            }
            finally
            {
                SelectedCategory = null;
            }
        }

        private async Task<bool> HasAssociatedTasksAsync(int categoryId)
        {
            try
            {
                var tasksResponse = await Http.GetAsync($"api/TaskItem/category/{categoryId}/tasks");

                if (tasksResponse.IsSuccessStatusCode)
                {
                    var tasks = await tasksResponse.Content.ReadFromJsonAsync<List<TaskItem>>();
                    return tasks != null && tasks.Any();
                }
                else
                {
                    ShowMessage(ToastType.Danger, "Error al obtener las tareas asociadas.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ToastType.Danger, $"Error al verificar las tareas asociadas: {ex.Message}");
                return true;
            }
        }
        #endregion

        #region Toast
        private void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

        private ToastMessage CreateToastMessage(ToastType toastType, string message)
        {
            var toastMessage = new ToastMessage();
            toastMessage.Type = toastType;
            toastMessage.Message = message;

            return toastMessage;
        }
        #endregion
    }
}
