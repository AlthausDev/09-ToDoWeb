//using TODO_V2.Client.Pages;
//using TODO_V2.Shared;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components;
//using System.Runtime.CompilerServices;

//namespace TODO_V2.Client.Shared.Modals
//{
//    partial class ModalEditUser
//    {
//        [Parameter]
//        public int Id { get; set; }
//        private string checkPassword { get; set; } = string.Empty;

//        [Parameter]
//        public string UserName { get; set; } = string.Empty;

//        [Parameter]
//        public string Password { get; set; } = string.Empty;

//        [Parameter]
//        public string TaskItemName { get; set; } = string.Empty;

//        [Parameter]
//        public string UserType { get; set; } = string.Empty;

//        private bool IsDisabled = true;
//        private string color = "black";

//        [Parameter] public EventCallback<MouseEventArgs> Editar { get; set; }
//        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }
//        [Parameter] public EventCallback<MouseEventArgs> Volver {  get; set; }


//        #region Handlers
//        protected void OnClickEdit()
//        {
//            //User user = new(Id, UserName, Password, TaskItemName, UserType, DateTimeOffset.Now.ToString("dd-MM-yyyy"));
//            //Sesion.user = user;
//            Editar.InvokeAsync();
//        }

//        protected void OnClickClose()
//        {           
//            Cerrar.InvokeAsync();
//        }

//        protected void OnClickVolver()
//        {
//            Volver.InvokeAsync();
//        }
//        #endregion Handlers
//    }
//}