using System.Security.Cryptography;
using System.Text;

namespace Market.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Pass { get; set; }
    public string Salt { get; set; } 
    public bool IsSeller { get; set; }
    
}