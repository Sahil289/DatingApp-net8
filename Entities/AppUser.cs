using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace API.Entities; //Logical Naming structure

public class AppUser
{
    public int Id { get; set; }
    public required string UserName {get; set;}
}
