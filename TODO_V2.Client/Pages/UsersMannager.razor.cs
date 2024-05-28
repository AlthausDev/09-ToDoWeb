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
    partial class UsersMannager
    {
        [Parameter]
        public string Id { get; set; }

        private User User { get; set; }

        private Modal ModalInstance = default!;
        private ConfirmDialog dialog = default!;

        [Inject] ToastService ToastService { get; set; } = default!;
        [Inject] protected PreloadService PreloadService { get; set; }

        private List<ToastMessage> messages = new();

        Grid<User> DataGrid = default!;
        private ObservableCollection<User> UserList { get; set; } = new ObservableCollection<User>();

        private User? selectedUser { get; set; } = null;
        public User? SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
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
               
                await GetUserData();
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

        private async Task<GridDataProviderResult<User>> UserDataProvider(GridDataProviderRequest<User> request)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            while (UserList.IsNullOrEmpty())
            {
                await Task.Delay(5);
            }

            stopwatch.Stop();
            Debug.WriteLine($"Tiempo total de espera: {stopwatch.ElapsedMilliseconds} ms");

            return await Task.FromResult(request.ApplyTo(UserList.OrderBy(User => User.Id)));
        }


        private async Task GetUserData()
        {
            UserList = new ObservableCollection<User>(await Http.GetFromJsonAsync<List<User>>($"api/User"));
        }
        #endregion

        #region SelectRow    
        private async Task SelectUser(GridRowEventArgs<User> args)
        {
            SelectedUser = args.Item;
        }

        #endregion SelectRow        

        #region Modal  
        private async Task HideModal()
        {
            SelectedUser = null;
            await ModalInstance.HideAsync();
        }

        #region User Item Form    

        private async Task OnClickUserForm(User? user)
        {
            SelectedUser = user;
            var parameters = new Dictionary<string, object>
             {
                { "UserName", SelectedUser?.Name},
                { "UserId", SelectedUser?.Id },
                { "Aceptar", EventCallback.Factory.Create<MouseEventArgs>(this, UserFormResult) },
                { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, HideModal) }
            };
            await ModalInstance.ShowAsync<ModalRegistro>(title: "Categorias", parameters: parameters);
        }


        private async Task UserFormResult()
        {
            ShowMessage(ToastType.Success, "Se ha creado exitosamente la categoría");
            await HideModal();
            await GetUserData();
            await DataGrid.RefreshDataAsync();
        }
        #endregion

        #endregion Modal       

        #region Delete

        private async Task DeleteUserAsync(User user)
        {
            SelectedUser = user;
            if (SelectedUser == null)
            {
                ToastService.Notify(new ToastMessage(ToastType.Warning, "Ninguna categoria seleccionada."));
                return;
            }

            var parameters = new Dictionary<string, object?>
            {
                { "Name", SelectedUser.Name },
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
                await DeleteUser(SelectedUser.Id);
            }
            else
            {
                ToastService.Notify(new ToastMessage(ToastType.Secondary, "Acción de eliminación cancelada."));
            }
        }

        private async Task DeleteUser(int Id)
        {
            SelectedUser = null;

            await Http.DeleteAsync($"api/User/{Id}");
            await GetUserData();
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
