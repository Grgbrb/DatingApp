using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [AllowAnonymous]


    public class UsersController : BasicApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _autoMapper;
        public UsersController(IUserRepository userRepository,IMapper autoMapper )
        {
            _autoMapper = autoMapper;
            _userRepository = userRepository;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await  _userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }   

        [HttpPut]

        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _autoMapper.Map(memberUpdateDto, user);
           
           _userRepository.UpdateUser(user);

           if (await _userRepository.SaveAllAsync()) return NoContent();

           return BadRequest("Failed to update User");

        }
    }
}