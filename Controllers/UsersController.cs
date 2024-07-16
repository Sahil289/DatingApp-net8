using System.Net.Mime;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// [ApiController]
// [Route("api/[controller]")] //localhost:5001/api/Users
public class UsersController(DataContext context) : BaseApiController
{
    // private readonly DataContext _context = context; // lot of developers like to name the private fields as _

    //Endpoints

    //The below code is called as synchronous code
    // ****** the best practice is to user asynchronous code if we are working with database because the code might block the request
    // [HttpGet]
    // public ActionResult<IEnumerable<AppUser>> GetUsers(){
    //     var users = context.Users.ToList();
    //     return Ok(users);
    //     // Types of Http Responses:
    //     // return Ok(users); or return users; // It returns ok request and both do the same thing
    //     // return BadRequest(); // This will return a code 400 as the response
    //     // return NotFound();// If there is no record it returns the notfound response
    // }
    // [HttpGet("{id:int}")] // api/user/1
    // public ActionResult<AppUser> GetUser(int id){
    //     var user = context.Users.Find(id);
    //     if(user == null) return NotFound();
    //     return Ok(user);
    // }

    [AllowAnonymous]
    //The below code is called as asynchronous code
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
        //await keyword is used to check the code that might block the request
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id){
        var user = await context.Users.FindAsync(id);
        if(user == null) return NotFound();
        return Ok(user);
    }

}
