using SQLite;

[Table("Products")]
public class DatabaseProduct
{
    [PrimaryKey]
    public int id { get; set; }

    public string productName { get; set; }

    public string description { get; set; }

    public string specialNote { get; set; }

    public string category { get; set; }

    public float price { get; set; }
}