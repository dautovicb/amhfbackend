namespace MentalHealth.Exceptions
{
    public class DuplicateNameException : Exception
    {
        public DuplicateNameException(string name)
            : base($"An item with the name '{name}' already exists.")
        {
        }
    }
} 