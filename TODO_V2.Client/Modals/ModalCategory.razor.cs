using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Json;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;

namespace TODO_V2.Client.Modals
{
    public partial class ModalCategory
    {
        [Parameter]
        public int? CategoryId { get; set; }
        [Parameter]
        public string? CategoryName { get; set; } = string.Empty;


        private Category? NewCategory = new();

        private string DescripcionColor = "#03e9f4";

        private bool IsInputValid = false;
        public bool IsEditing { get; private set; } = false;

        private List<ToastMessage> messages = new();

        [Parameter] public EventCallback<MouseEventArgs> Aceptar { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            if (CategoryId.HasValue)
            {
                IsEditing = true;
                IsInputValid = true;
            }
            else
            {
                IsEditing = false;
                IsInputValid = false;
                ClearFields();
            }

            await base.OnParametersSetAsync();
        }


        #region OnClick     
        protected async Task OnClickAceptar()
        {
            if (IsInputValid)
            {
                if (IsEditing)
                {
                    NewCategory.Id = (int)CategoryId;
                    NewCategory.Name = CategoryName;

                    await EditItem();
                }
                else
                {
                    NewCategory = new(CategoryName);
                    await NewItem();
                }

                ClearFields();
                await Aceptar.InvokeAsync();
            }
            else
            {
                ShowMessage(ToastType.Danger, "Introduzca el nombre de la categoría");
                DescripcionColor = ColorsEnum.crimson.ToString();
                return;
            }
        }

        protected void OnClickClose()
        {
            ClearFields();
            _ = Cerrar.InvokeAsync();
        }

        #endregion OnClick

        #region Handlers
        private void ValueChangeHandler()
        {
            DescripcionColor = "#03e9f4";
            IsInputValid = !CategoryName.IsNullOrEmpty();
        }

        #endregion Handlers

        #region Api    
        private async Task NewItem()
        {
            _ = await Http.PostAsJsonAsync("api/Category", NewCategory);
        }

        private async Task EditItem()
        {
            _ = await Http.PutAsJsonAsync($"api/Category/{CategoryId}", NewCategory);
        }
        #endregion Api

        #region Toast
        private void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

        private ToastMessage CreateToastMessage(ToastType toastType, string message)
        {
            ToastMessage toastMessage = new();
            toastMessage.Type = toastType;
            toastMessage.Message = message;

            return toastMessage;
        }
        #endregion Toast


        #region Aux
        private void ClearFields()
        {
            CategoryId = null;
            CategoryName = string.Empty;
            IsInputValid = false;
            IsEditing = false;
            DescripcionColor = "#03e9f4";
        }
        #endregion Aux
    }
}
