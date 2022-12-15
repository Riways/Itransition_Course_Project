using AutoMapper.Internal;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using totten_romatoes.Server.Data;
using totten_romatoes.Server.Services;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReviewModelsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;

        public ReviewModelsController(IUserService userService, IReviewService reviewService)
        {
            _userService = userService;
            _reviewService = reviewService;
        }

        // GET: api/ReviewModels
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<ReviewModel> GetReviews()
        {
            return _reviewService.GetAllReviews();
        }

        // GET: api/ReviewModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewModel>> GetReviewModel(int id)
        {
            var reviewModel = _reviewService.GetReviewById(id);

             if (reviewModel == null)
            {
                return NotFound();
            }

            return Ok(reviewModel);
        }

        // PUT: api/ReviewModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /* [HttpPut("{id}")]
         public async Task<IActionResult> PutReviewModel(int id, ReviewModel reviewModel)
         {
             if (id != reviewModel.Id)
             {
                 return BadRequest();
             }

             _context.Entry(reviewModel).State = EntityState.Modified;

             try
             {
                 await _context.SaveChangesAsync();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!ReviewModelExists(id))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }

             return NoContent();
         }*/

        // POST: api/ReviewModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReviewModel>> PostReviewModel(ReviewModel reviewModel)
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            reviewModel.AuthorId = userId;
            _reviewService.AddReviewToDb(reviewModel);
            return CreatedAtAction("GetReviewModel", new { id = reviewModel.Id }, reviewModel);
        }

        // DELETE: api/ReviewModels/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewModel(int id)
        {
            var reviewModel = await _context.Reviews.FindAsync(id);
            if (reviewModel == null)
            {
                return NotFound();
            }
            _context.Reviews.Remove(reviewModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ReviewModelExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }*/
    }
}
