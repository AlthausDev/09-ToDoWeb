using BlazorBootstrap;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TODO_V2.Client.Layout;
using TODO_V2.Shared.Models;
using ToastType = BlazorBootstrap.ToastType;

namespace TODO_V2.Client.Pages
{
    public partial class Todo
    {
        [Parameter]
        public string Id { get; set; }
        public List<TaskItem> Chores { get; set; } = new List<TaskItem>();

        private TaskItem newChore { get; set; } = new TaskItem();
        private TaskItem NewChore
        {
            get
            {
                return newChore;
            }
            set
            {
                if (newChore != value)
                {
                    newChore = value;
                }
            }
        }

        private TaskItem? selectedChore { get; set; } = null;
        public TaskItem? SelectedChore
        {
            get
            {
                return selectedChore;
            }
            set
            {
                if (selectedChore != value)
                {
                    selectedChore = value;
                    //SelectedChangeHandler();
                }
            }
        }

        protected bool IsDisabled { get; set; } = true;
        protected bool IsDisabledEdit { get; set; } = true;

        //private Enum? accion = Accion.Espera;
        public Modal ModalInstance = default!;
        List<ToastMessage> messages = StartUp.messages;

        public static User user = new();


        private PieChart pieChart = default!;
        private PieChartOptions pieChartOptions = default!;
        private ChartData chartData = default!;
        private string[]? backgroundColors = ColorBuilder.CategoricalTwelveColors;

        private void ShowNewChoreModal()
        {
            //choreFormRef.SetChore(new TaskItem { CreationDate = DateTime.Now });
            //modalRef.Show();
        }

        private void ShowEditChoreModal(TaskItem TaskItem)
        {
            //choreFormRef.SetChore(TaskItem);
            //modalRef.Show();
        }

        private void SaveChore()
        {
            //var TaskItem = choreFormRef.GetChore();
            //if (!Chores.Contains(TaskItem))
            //{
            //    Chores.Add(TaskItem);
            //}
            //modalRef.Hide();
        }

        private void DeleteChore(TaskItem TaskItem)
        {
            Chores.Remove(TaskItem);
        }


        //        protected override async TaskItem OnInitializedAsync()
        //        {   
        //            //await getData();
        //            //await InitializeGraph();
        //        }

        #region OnClick
        private async Task OnClickLogOut()
        {
            var response = await Http.DeleteAsync("/user/logout");
            await storageService.RemoveItemAsync("token");
            await storageService.ClearAsync();
            NavManager.NavigateTo("/login");
            Http.DefaultRequestHeaders.Remove("Authorization");
        }
        #endregion

        //        #region ApiOperations    
        //        private async TaskItem getData()
        //        {
        //            TaskItem[]? choresArray = await Http.GetFromJsonAsync<TaskItem[]>("TaskItem");

        //            if (choresArray is not null)
        //            {
        //                Chores = [.. choresArray];
        //            }
        //        }


        //        private async TaskItem Post()
        //        {
        //            Chores.Add(newChore);
        //            await Http.PostAsJsonAsync("TaskItem", NewChore);
        //            await getData();
        //        }

        //        private async TaskItem Put()
        //        {
        //            await Http.PutAsJsonAsync("TaskItem", NewChore);

        //            Chores.Insert(Chores.IndexOf(selectedChore), newChore);
        //            Chores.Remove(selectedChore);
        //        }

        //        private async TaskItem Delete()
        //        {
        //            if (selectedChore != null)
        //            {
        //                Chores.Remove(selectedChore);
        //                HttpResponseMessage httpResponseMessage = await Http.DeleteAsync($"/DelChore/{selectedChore.Id}");

        //                await getData();
        //                SelectedChore = null;

        //                if (accion.Equals(Accion.Espera))
        //                {
        //                    ShowMessage(ToastType.Danger, "Registro eliminado con éxito");
        //                }
        //            }
        //        }
        //        #endregion ApiOperations

        //        #region Modal
        //        private async TaskItem execChore()
        //        {
        //            if (accion.Equals(Accion.Crear))
        //            {
        //                await Post();
        //                ShowMessage(ToastType.Success, "Registro agregado con éxito");
        //            }
        //            else
        //            {
        //                await Put();
        //                ShowMessage(ToastType.Warning, "Registro editado con éxito");
        //            }
        //            await HideModal();
        //        }


        //        private async TaskItem OnClickShowModal(Enum accion)
        //        {
        //            this.accion = accion;

        //            if (accion.Equals(Accion.Editar))
        //            {
        //                NewChore = selectedChore;
        //            }

        //            if (accion.Equals(Accion.Crear))
        //            {
        //                NewChore = new TaskItem();
        //            }

        //            await modal.ShowAsync();
        //        }

        //        private async TaskItem HideModal()
        //        {
        //            accion = Accion.Espera;
        //            SelectedChore = null;
        //            NewChore = new TaskItem();
        //            await modal.HideAsync();
        //        }
        //        #endregion Modal       

        //        #region SelectRow
        //        private void selectChore(TaskItem TaskItem)
        //        {
        //            SelectedChore = TaskItem;
        //            //Console.WriteLine(TaskItem.Name);
        //        }

        //        private string GetRowClass(TaskItem TaskItem)
        //        {
        //            return TaskItem == SelectedChore ? "selected-row" : "";
        //        }
        //        #endregion SelectRow

        //        #region AutoComplete
        //        private async TaskItem<AutoCompleteDataProviderResult<TaskItem>> ChoresDataProvider(AutoCompleteDataProviderRequest<TaskItem> request)
        //        {
        //            return await TaskItem.FromResult(request.ApplyTo(Chores.OrderBy(TaskItem => TaskItem.TaskItemName)));
        //        }

        //        #endregion AutoComplete

        //        #region Toast
        //        private void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

        //        private ToastMessage CreateToastMessage(ToastType toastType, string message)
        //        {
        //            var toastMessage = new ToastMessage();
        //            toastMessage.Type = toastType;
        //            toastMessage.Message = message;

        //            return toastMessage;
        //        }
        //        #endregion Toast

        //        #region Graph     
        //        //TODO no se cuentan correctamente el numero de lineas
        //        protected async TaskItem InitializeGraph()
        //        {
        //            chartData = new ChartData { Labels = GetDataLabels(), Datasets = GetDataSet() };

        //            pieChartOptions = new()
        //            {
        //                Responsive = true
        //            };
        //            pieChartOptions.Plugins.Title!.Text = "Chores Finalizadas";
        //            pieChartOptions.Plugins.Title.Display = true;
        //            pieChartOptions.Plugins.Title.Font.Size = 18;
        //            pieChartOptions.Plugins.Legend.Display = false;

        //            if (true)
        //            {
        //                await pieChart.InitializeAsync(chartData, pieChartOptions);
        //            }
        //            await base.OnAfterRenderAsync(true);
        //        }

        //        private List<IChartDataset> GetDataSet()
        //        {
        //            var datasets = new List<IChartDataset>
        //            {
        //                GetPieChartDataset()
        //            };

        //            return datasets;
        //        }

        //        private PieChartDataset GetPieChartDataset()
        //        {
        //            return new() { Label = " ", Data = GetChartData(), BackgroundColor = GetBackgroundColors() };
        //        }

        //        private List<double> GetChartData()
        //        {
        //            List<double> data = new();

        //            //double count = (from TaskItem TaskItem in Chores
        //            //                where TaskItem
        //            //                select TaskItem).Count();

        //            //data.Add(Chores.Count - count);
        //            //data.Add(count);

        //            return data;
        //        }

        //        //10 finalizado, 3 no finalizado
        //        private List<string> GetBackgroundColors()
        //        {
        //            List<string> colors = new()
        //            {
        //                backgroundColors[3],
        //                backgroundColors[10]
        //            };

        //            return colors;
        //        }

        //        private static List<string> GetDataLabels()
        //        {

        //            return new List<string>()
        //                {
        //                "No Finalizado",
        //                "Finalizado"
        //            };
        //        }
        //        #endregion Graph

        //        #region Handlers
        //        private void SelectedChangeHandler()
        //        {
        //            IsDisabledEdit = selectedChore == null;
        //        }

        //        private void ValueChangeHandler()
        //        {
        //            IsDisabled = (String.IsNullOrWhiteSpace(NewChore.TaskItemName) || String.IsNullOrWhiteSpace(NewChore.State));
        //        }

        //        private async TaskItem OnAutoCompleteChanged(TaskItem TaskItem)
        //        {
        //            SelectedChore = TaskItem;
        //            //await JS.InvokeVoidAsync($"searchFuction('{SelectedChore.TaskItemName}')");
        //            Console.WriteLine($"'{TaskItem?.TaskItemName}' selected.");
        //        }
        //        #endregion Handlers

        //        #region Enums
        //        public enum Accion
        //        {
        //            Espera,
        //            Crear,
        //            Editar
        //        }

        //        public enum IsFinalizado
        //        {
        //            No,
        //            Si
        //        }
        //        #endregion Enums
        private void ShowNewTaskModal(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}
