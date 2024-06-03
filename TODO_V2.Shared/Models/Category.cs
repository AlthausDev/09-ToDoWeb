namespace TODO_V2.Shared.Models
{
    public class Category : BaseModel
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El nombre de la categoría no puede estar vacío o contener solo espacios en blanco.");
                }
                _name = value;
            }
        }

        public Category()
        {
        }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Category(string name)
        {
            Name = name;
        }
    }
}
