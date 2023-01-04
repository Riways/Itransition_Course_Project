using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using totten_romatoes.Server.Services;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Controllers
{
    [Route("api/reviews")]
    [Authorize]
    [ApiController]
    public class ReviewModelsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewModelsController(IReviewService reviewService )
        {
            _reviewService = reviewService;
        }

        [AllowAnonymous]
        [HttpGet("search/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> FullTextSearch(string key)
        {
            if (key.IsNullOrEmpty())
                return BadRequest("No word to search was provided");
            var searchResult = await _reviewService.FullTextSearch(key);
            return searchResult;
        }

        [AllowAnonymous]
        [HttpGet("amount")]
        public async Task<int> GetAmountOfReviews()
        {
            return await _reviewService.GetAmountOfReviews();
        }

        [AllowAnonymous]
        [HttpGet("lightweight-chunk")]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetLightweightChunkOfReviews([FromQuery] string userId)
        {
            return Ok(await _reviewService.GetLightweightListOfReviews(userId));
        }

        [AllowAnonymous]
        [HttpGet("chunk")]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetChunkOfReviews([FromQuery] int number, [FromQuery] int sortType)
        {
            if (number < 0)
                return BadRequest("Page number can't be less then 0");
            return Ok(await _reviewService.GetChunkOfSortedReviews(number, (SortBy) sortType));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewModel>> GetReviewModel(long id)
        {
            var reviewModel = await _reviewService.GetReviewById(id);
            if (reviewModel == null)
            {
                return NotFound();
            }
            return Ok(reviewModel);
        }

        [HttpGet("add-fakes/{amount}")]
        public async Task<ActionResult> GenerateReviews(int amount)
        {
            if (amount <= 0)
                return BadRequest("Amount of reviews should be more then 0");
            await _reviewService.GenerateFakeReviews(amount);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ReviewModel>> PostReviewModel(ReviewModel reviewModel)
        {
            await _reviewService.AddReviewToDb(reviewModel);
            return CreatedAtAction("GetReviewModel", new { id = reviewModel.Id }, reviewModel);
        }

        [HttpPost("add-comment")]
        public async Task PostComment(CommentModel commentModel)
        {
            await _reviewService.AddCommentToDb(commentModel);
        }
        
        [HttpPost("like")]
        public void PostLike(LikeModel likeModel)
        {
             _reviewService.AddLikeToDb(likeModel);
        }

        [HttpDelete("like/{id}")]
        public void DeleteLike(long id)
        {
            _reviewService.DeleteLikeDromDb(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewModel(int id)
        {
            await _reviewService.DeleteReviewDromDb(id);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMultipleReviews([FromQuery] IEnumerable<long> ids)
        {
            await _reviewService.DeleteMuiltipleReviews(ids);
            return NoContent();
        }
    }
}
