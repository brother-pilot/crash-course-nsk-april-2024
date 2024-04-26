namespace Market.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string? Pass { get; set; }
    public bool isSeller { get; set; }
}