namespace Backend.Entity;

public class BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime updated_at { get; set; }
    public DateTime created_at { get; set; }
}
