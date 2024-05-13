//using BlazorBootstrap;
//using System.Net.Http.Json;
//using TODO_V2.Shared.Models;
//using ToastType = BlazorBootstrap.ToastType;

//namespace TODO_V2.Client.Pages
//{
//    public partial class Todo
//    {
//        public List<Chore> Chores { get; set; } = new List<Chore>();

//        private Chore nuevaChore { get; set; } = new Chore();
//        private Chore NuevaChore
//        {
//            get
//            {
//                return nuevaChore;
//            }
//            set
//            {
//                if (nuevaChore != value)
//                {
//                    nuevaChore = value;
//                }
//            }
//        }


//        private Chore? selectedChore { get; set; } = null;
//        public Chore? SelectedChore
//        {
//            get
//            {
//                return selectedChore;
//            }
//            set
//            {
//                if (selectedChore != value)
//                {
//                    selectedChore = value;
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
//            Chore[]? choresArray = await Http.GetFromJsonAsync<Chore[]>("chore");

//            if (choresArray is not null)
//            {
//                Chores = [.. choresArray];
//            }
//        }


//        private async Task Post()
//        {
//            Chores.Add(nuevaChore);
//            await Http.PostAsJsonAsync("Chore", NuevaChore);
//            await getData();
//        }
      
//        private async Task Put()
//        {
//            await Http.PutAsJsonAsync("Chore", NuevaChore);

//            Chores.Insert(Chores.IndexOf(selectedChore), nuevaChore);
//            Chores.Remove(selectedChore);
//        }

//        private async Task Delete()
//        {
//            if (selectedChore != null)
//            {
//                Chores.Remove(selectedChore);
//                HttpResponseMessage httpResponseMessage = await Http.DeleteAsync($"/DelTask/{selectedChore.Id}");            
                
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
//        private async Task execChore()
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
//                NuevaChore = selectedChore;
//            }

//            if (accion.Equals(Accion.Crear))
//            {
//                NuevaChore = new Chore();
//            }

//            await modal.ShowAsync();
//        }

//        private async Task HideModal()
//        {
//            accion = Accion.Espera;
//            SelectedChore = null;
//            NuevaChore = new Chore();
//            await modal.HideAsync();
//        }
//        #endregion Modal       

//        #region SelectRow
//        private void selectChore(Chore chore)
//        {
//            SelectedChore = chore;
//            //Console.WriteLine(chore.Name);
//        }

//        private string GetRowClass(Chore chore)
//        {
//            return chore == SelectedChore ? "selected-row" : "";
//        }
//        #endregion SelectRow

//        #region AutoComplete
//        private async Task<AutoCompleteDataProviderResult<Chore>> ChoresDataProvider(AutoCompleteDataProviderRequest<Chore> request)
//        {
//            return await Task.FromResult(request.ApplyTo(Chores.OrderBy(chore => chore.TaskName)));
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

//            //double count = (from Chore chore in Chores
//            //                where chore
//            //                select chore).Count();

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
//            IsDisabled = (String.IsNullOrWhiteSpace(NuevaChore.TaskName) || String.IsNullOrWhiteSpace(NuevaChore.State));
//        }

//        private async Task OnAutoCompleteChanged(Chore chore)
//        {
//            SelectedChore = chore;
//            //await JS.InvokeVoidAsync($"searchFuction('{SelectedChore.TaskName}')");
//            Console.WriteLine($"'{chore?.TaskName}' selected.");
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
