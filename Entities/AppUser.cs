using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace API.Entities; //Logical Naming structure

public class AppUser
{
    public int Id { get; set; }
    public required string UserName {get; set;}
    public required byte[] PassHash { get; set; }
    public required byte[] PassSalt { get; set; }
}
