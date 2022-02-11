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
using API.Extensions;
using API.Helpers;

namespace API.Controllers
{
    [Authorize]
    [AllowAnonymous]


    public class UsersController : BasicApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _autoMapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository,IMapper autoMapper, IPhotoService  photoService)
        {
            _autoMapper = autoMapper;
            _photoService = photoService;
            _userRepository = userRepository;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = user.UserName;

            if (string.IsNullOrEmpty(userParams.Gender)) userParams.Gender = user.Gender == "male" ? "female" : "male";

            var users = await  _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,users.TotalCounts,users.TotalPages);

            return Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }   

        [HttpPut]

        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _autoMapper.Map(memberUpdateDto, user);
           
           _userRepository.UpdateUser(user);

           if (await _userRepository.SaveAllAsync()) return NoContent();

           return BadRequest("Failed to update User");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);
            
            if(result.Error != null) return BadRequest(result.Error.Message) ;

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicID = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
               
                return CreatedAtRoute("GetUser", new {username = user.UserName},_autoMapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem Adding the photo");
        }
        [HttpPut ("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId){

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This photo is already your main");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain= false;
            
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Fail to set Main Photo");

        }

        [HttpDelete ("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId){

        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("You cannot delete your main Photo");

        if (photo.PublicID != null) 
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicID);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if(await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Fail to delete the Photo");

        }

    }
}