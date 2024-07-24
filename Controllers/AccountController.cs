using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] //account/register
    // public async Task<ActionResult<AppUser>> Register(string username, string password){
    // public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto){
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

        if(await UserExists(registerDto.Username)) return BadRequest("Username is taken!");
        return Ok();
        // using var hmac = new HMACSHA512();// we don't want to inject the hash class into the controller, we just want to use it, so basically with using statement it automatically disposes the variables from garbage collection
        // // using var hmac = new HMACSHA512(); // Control over garbage collection
        // // var hmac = new HMACSHA512(); // No control over garbase collection

        // var user = new AppUser
        // {
        //     UserName = registerDto.Username.ToLower(),
        //     PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        //     // UserName = username,
        //     // PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
        //     PassSalt = hmac.Key
        // };
        
        // context.Users.Add(user);
        
        // await context.SaveChangesAsync();

        // // return user;
        // return new UserDto{
        //     Username = user.UserName,
        //     Token = tokenService.CreateToken(user)
        // };
    }
    private async Task<bool> UserExists(string username){
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
    [HttpPost("login")]
    // public async Task<ActionResult<AppUser>> Login(LoginDto loginDto){
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
        //Checking whether any user exits with the entered username in the database
        var user = await context.Users.FirstOrDefaultAsync(
            x => x.UserName == loginDto.Username.ToLower());
        //Checking whether the firstordefaultasync returned any record or null. null because default value if no data is found is null
        if(user == null) return Unauthorized("Invalid username!");
        //using the hmac hashing to pass the PasswordSalt as it if been fetched from database
        using var hmac = new HMACSHA512(user.PassSalt);
        //Computing the entered password's hash value 
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        //Looping through the byte of array as the hashed password is stored in bytes
        for (int i = 0; i < computedHash.Length; i++)
        {
            //checking individual characters of the array whether they match or not. if they do then continue else return the below code
            if(computedHash[i] != user.PassHash[i]) return Unauthorized("Invalid password!");
        }
        // return user;
        return new UserDto{
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
}
