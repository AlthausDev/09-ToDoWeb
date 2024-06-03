using Microsoft.AspNetCore.Components;

namespace TODO_V2.Client.Modals
{
    partial class ModalDelete
    {

        [Parameter] public string Name { get; set; }
        [Parameter] public string Message { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
