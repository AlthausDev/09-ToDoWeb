using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace TODO_V2.Client.Pages
{
    public partial class UserPanel
    {
        public static Modal ModalInstance = default!;

        [Parameter]
        public string Id { get; set; }

        public static async Task HideModal()
        {
            await ModalInstance.HideAsync();
        }
    }
}
