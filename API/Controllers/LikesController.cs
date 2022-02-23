using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]

    public class LikesController : BasicApiController
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public LikesController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        [HttpPost("{username}")]

        public async Task<ActionResult> AddLike(string username)
        {
            var SourceUserId = User.GetUserId();

            var LikedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            var SourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(SourceUserId);

            if (LikedUser == null) return NotFound();

            if (SourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var UserLike = await _unitOfWork.LikesRepository.GetUserLike(SourceUserId, LikedUser.Id);

            if( UserLike != null) return BadRequest("You 've already liked this user");

            UserLike = new  UserLike {

                SourceUserId = SourceUserId,
                LikedUserId = LikedUser.Id
            };

            SourceUser.LikedUsers.Add(UserLike);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams){

            likesParams.UserId = User.GetUserId();
            var users =  await _unitOfWork.LikesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCounts, users.TotalPages);

            return Ok(users);
        }
    }


}