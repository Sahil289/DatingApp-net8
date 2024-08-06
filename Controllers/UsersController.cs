using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
// [ApiController]
// [Route("api/[controller]")] //localhost:5001/api/Users
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
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

    // [AllowAnonymous]
    //The below code is called as asynchronous code
    [HttpGet]
    // public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
        //await keyword is used to check the code that might block the request
        // var users = await context.Users.ToListAsync();
        // var users = await userRepository.GetUsersAsync();
        var users = await userRepository.GetMembersAsync();

        // var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);
        // return Ok(users);
        // return Ok(usersToReturn);
        return Ok(users);
    }
    // [Authorize]
    // [HttpGet("{id:int}")]`
    [HttpGet("{username}")]
    // public async Task<ActionResult<AppUser>> GetUser(int id){
    public async Task<ActionResult<MemberDto>> GetUser(string username){
        // var user = await context.Users.FindAsync(id);
        // var user = await userRepository.GetUserByUsernameAsync(username);
        var user = await userRepository.GetMemberAsync(username);
        if(user == null) return NotFound();
        // return Ok(user);
        //Automapper
        // var userToReturn = mapper.Map<MemberDto>(user);
        // return Ok(userToReturn);
        return Ok(user);
        // return user;
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(username == null) return BadRequest("No username found in token!");
        var user = await userRepository.GetUserByUsernameAsync(username);
        if(user == null) return BadRequest("Could not find user!");
        mapper.Map(memberUpdateDto, user);
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to update the user!");
    }
}
