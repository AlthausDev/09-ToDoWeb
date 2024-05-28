using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using TODO_V2.Client.Data;
using TODO_V2.Client.Shared.Modals;
using TODO_V2.Shared.Models;
using Microsoft.IdentityModel.Tokens;

namespace TODO_V2.Client.Pages
{
    partial class CategoryMannager
    {
        [Parameter]
        public string Id { get; set; }

        private User User { get; set; }

        private Modal ModalInstance = default!;
        private ConfirmDialog dialog = default!;

        [Inject] ToastService ToastService { get; set; } = default!;
        [Inject] protected PreloadService PreloadService { get; set; }

        private List<ToastMessage> messages = new();

        Grid<Category> DataGrid = default!;
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
                { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, Admin.HideModal) }
            };
            await Admin.ModalInstance.ShowAsync<ModalCategory>(title: "Categorias", parameters: parameters);
        }


        private async Task CategoryFormResult()
        {
            ShowMessage(ToastType.Success, "Se ha creado exitosamente la categoría");
            await Admin.HideModal();
            await GetCategoryData();
            await DataGrid.RefreshDataAsync();
        }
        #endregion

        #endregion Modal       

        #region Delete
        private async Task DeleteCategoryAsync(Category category)
        {
            SelectedCategory = category;
            if (SelectedCategory == null)
            {
                ToastService.Notify(new ToastMessage(ToastType.Warning, "Ninguna categoria seleccionada."));
                return;
            }

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
                await DeleteCategory(SelectedCategory.Id);
            }
            else
            {
                ToastService.Notify(new ToastMessage(ToastType.Secondary, "Acción de eliminación cancelada."));
            }
        }

        //TODO Añadir mensaje de error al borrar si tiene tareas asociadas
        private async Task DeleteCategory(int Id)
        {
            SelectedCategory = null;

            await Http.DeleteAsync($"api/Category/{Id}");
            await GetCategoryData();
            await DataGrid.RefreshDataAsync();

            ShowMessage(ToastType.Success, "Categoria eliminada con éxito.");
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
