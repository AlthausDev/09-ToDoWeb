namespace TODO_V2.Shared.Models.Enum
{
    public enum TaskStateEnum
    {
        Pendiente = 1,
        EnProceso = 2,
        Completada = 3,
        Cancelada = 4
    }

    public static class TaskStateEnumExtensions
    {
        private static readonly Dictionary<int, string> TaskStateNames = new()
        {
            { (int)TaskStateEnum.Pendiente, "Pendiente" },
            { (int)TaskStateEnum.EnProceso, "En proceso" },
            { (int)TaskStateEnum.Completada, "Completada" },
            { (int)TaskStateEnum.Cancelada, "Cancelada" }
        };

        public static string GetName(this TaskStateEnum taskState)
        {
            return TaskStateNames[(int)taskState];
        }
    }
}
