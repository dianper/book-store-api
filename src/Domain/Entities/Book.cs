namespace Domain.Entities
{
    public record Book(string Name, decimal Price, string Author, string Category)
    {
        public int Id { get; set; }
    }
}