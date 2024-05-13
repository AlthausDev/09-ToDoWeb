//using BlazorBootstrap;
//using System.Net.Http.Json;
//using TODO_V2.Shared.Models;
//using ToastType = BlazorBootstrap.ToastType;

//namespace TODO_V2.Client.Pages
//{
//    public partial class Todo
//    {
//        public List<Tasks> Taskss { get; set; } = new List<Tasks>();

//        private Tasks nuevaTasks { get; set; } = new Tasks();
//        private Tasks NuevaTasks
//        {
//            get
//            {
//                return nuevaTasks;
//            }
//            set
//            {
//                if (nuevaTasks != value)
//                {
//                    nuevaTasks = value;
//                }
//            }
//        }


//        private Tasks? selectedTasks { get; set; } = null;
//        public Tasks? SelectedTasks
//        {
//            get
//            {
//                return selectedTasks;
//            }
//            set
//            {
//                if (selectedTasks != value)
//                {
//                    selectedTasks = value;
//                    SelectedChangeHandler();
//                }
//            }
//        }

//        protected bool IsDisabled { get; set; } = true;
//        protected bool IsDisabledEdit { get; set; } = true;

//        private Enum? accion = Accion.Espera;
//        private Modal modal = default!;

//        List<ToastMessage> messages = new();

//        private PieChart pieChart = default!;
//        private PieChartOptions pieChartOptions = default!;
//        private ChartData chartData = default!;
//        private string[]? backgroundColors = ColorBuilder.CategoricalTwelveColors;


//        //protected override async Task OnInitializedAsync()
//        //{
//        //    await getData();
//        //    await InitializeGraph();
//        //}

//        #region ApiOperations    
//        private async Task getData()
//        {
//            Tasks[]? taskssArray = await Http.GetFromJsonAsync<Tasks[]>("tasks");

//            if (taskssArray is not null)
//            {
//                Taskss = [.. taskssArray];
//            }
//        }


//        private async Task Post()
//        {
//            Taskss.Add(nuevaTasks);
//            await Http.PostAsJsonAsync("Tasks", NuevaTasks);
//            await getData();
//        }
      
//        private async Task Put()
//        {
//            await Http.PutAsJsonAsync("Tasks", NuevaTasks);

//            Taskss.Insert(Taskss.IndexOf(selectedTasks), nuevaTasks);
//            Taskss.Remove(selectedTasks);
//        }

//        private async Task Delete()
//        {
//            if (selectedTasks != null)
//            {
//                Taskss.Remove(selectedTasks);
//                HttpResponseMessage httpResponseMessage = await Http.DeleteAsync($"/DelTask/{selectedTasks.Id}");            
                
//                await getData();
//                SelectedTasks = null;

//                if (accion.Equals(Accion.Espera))
//                {
//                    ShowMessage(ToastType.Danger, "Registro eliminado con éxito");
//                }
//            }
//        }
//        #endregion ApiOperations

//        #region Modal
//        private async Task execTasks()
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


//        private async Task OnClickShowModal(Enum accion)
//        {
//            this.accion = accion;

//            if (accion.Equals(Accion.Editar))
//            {
//                NuevaTasks = selectedTasks;
//            }

//            if (accion.Equals(Accion.Crear))
//            {
//                NuevaTasks = new Tasks();
//            }

//            await modal.ShowAsync();
//        }

//        private async Task HideModal()
//        {
//            accion = Accion.Espera;
//            SelectedTasks = null;
//            NuevaTasks = new Tasks();
//            await modal.HideAsync();
//        }
//        #endregion Modal       

//        #region SelectRow
//        private void selectTasks(Tasks tasks)
//        {
//            SelectedTasks = tasks;
//            //Console.WriteLine(tasks.Name);
//        }

//        private string GetRowClass(Tasks tasks)
//        {
//            return tasks == SelectedTasks ? "selected-row" : "";
//        }
//        #endregion SelectRow

//        #region AutoComplete
//        private async Task<AutoCompleteDataProviderResult<Tasks>> TaskssDataProvider(AutoCompleteDataProviderRequest<Tasks> request)
//        {
//            return await Task.FromResult(request.ApplyTo(Taskss.OrderBy(tasks => tasks.TaskName)));
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
//        protected async Task InitializeGraph()
//        {
//            chartData = new ChartData { Labels = GetDataLabels(), Datasets = GetDataSet() };

//            pieChartOptions = new()
//            {
//                Responsive = true
//            };
//            pieChartOptions.Plugins.Title!.Text = "Taskss Finalizadas";
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

//            //double count = (from Tasks tasks in Taskss
//            //                where tasks
//            //                select tasks).Count();

//            //data.Add(Taskss.Count - count);
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
//            IsDisabledEdit = selectedTasks == null;
//        }

//        private void ValueChangeHandler()
//        {
//            IsDisabled = (String.IsNullOrWhiteSpace(NuevaTasks.TaskName) || String.IsNullOrWhiteSpace(NuevaTasks.State));
//        }

//        private async Task OnAutoCompleteChanged(Tasks tasks)
//        {
//            SelectedTasks = tasks;
//            //await JS.InvokeVoidAsync($"searchFuction('{SelectedTasks.TaskName}')");
//            Console.WriteLine($"'{tasks?.TaskName}' selected.");
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
//    }

//}
