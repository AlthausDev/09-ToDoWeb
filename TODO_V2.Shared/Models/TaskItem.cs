namespace TODO_V2.Shared.Models
{
    public class TaskItem : BaseModel
    {
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int StateId { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public TaskItem()
        {
        }

        public TaskItem(int categoryId, int userId, int stateId, string name, DateTime expirationDate)
        {
            CategoryId = categoryId;
            UserId = userId;
            StateId = stateId;
            Name = name;
            ExpirationDate = expirationDate;
        }
    }
}
