//using TODO_V2.Client.Pages;
//using TODO_V2.Shared;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;
//using System.Reflection.Metadata;

//namespace TODO_V2.Client.Shared.Modals
//{
//    partial class ModalLogin 
//    {
//        private string UserName { get; set; } = string.Empty;
//        private string Password { get; set; } = string.Empty;

//        private bool IsDisabled = true;
//        private string color = "black";


//        [Parameter] public EventCallback<MouseEventArgs> Login { get; set; }
//        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }   
      


//        #region Handlers
//        protected void OnClickLogin()
//        {
//            //User user = new(UserName, Password);
//            //Home.NewUser = user;
//            Login.InvokeAsync();
//        }

//        protected void OnClickClose()
//        {
//            UserName = string.Empty;
//            Password = string.Empty;
//            Cerrar.InvokeAsync();
//        }

//        private void ValueChangeHandler()
//        {
//            IsDisabled = string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password);
//        }

//        #endregion Handlers
//    }
//}

