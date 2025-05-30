using Microsoft.AspNetCore.Mvc;
using SwipeLab.Data;
using SwipeLab.Domain.DatingProfileFeedback;
using SwipeLab.Model.Requests;
using SwipeLab.Model.Responses.DatingProfile;
using SwipeLab.Services.Interfaces.Interfaces;

namespace SwipeLab.Controllers
{
    [ApiController]
    [Route("profiles")]
    public class ProfilesController : ControllerBase
    {
        private readonly ILogger<ProfilesController> _logger;
        private readonly IDatingProfileSetRepository _datingProfileSetRepository;
        private readonly IDatingProfileFeedbackService _datingProfileFeedbackService;

        public ProfilesController(ILogger<ProfilesController> logger, IDatingProfileSetRepository datingProfileSetRepository, IDatingProfileFeedbackService datingProfileFeedbackService)
        {
            _logger = logger;
            _datingProfileSetRepository = datingProfileSetRepository;
            _datingProfileFeedbackService = datingProfileFeedbackService;
        }

        [HttpGet("set/{datingProfileSetId}")]
        [ProducesResponseType(typeof(DatingProfile[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DatingProfileSet>> GetDatingProfileSet([FromRoute] Guid datingProfileSetId)
        {
            try
            {
                _logger.LogInformation("Getting DatingProfileSet with Id: {DatingProfileSetId}", datingProfileSetId);

                var datingProfileSet = await _datingProfileSetRepository.GetDatingProfileSetById(datingProfileSetId);

                if (datingProfileSet == null)
                {
                    return NotFound($"DatingProfileSet with Id: {datingProfileSetId} not found");
                }

                var response = datingProfileSet.DatingProfiles.Select(dp => new DatingProfile(dp)).ToList();

                _logger.LogInformation("DatingProfileSet with Id: {DatingProfileSetId}  and {Count} profiles found.", datingProfileSetId, datingProfileSet.DatingProfiles.Count);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving DatingProfileSet with ID: {DatingProfileSetId}", datingProfileSetId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the DatingProfileSet.");
            }
        }

        [HttpPost("{datingProfileId}/swipe")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> SwipeOnProfile([FromRoute] Guid datingProfileId, [FromBody] ProfileSwipeRequest request)
        {
            try
            {
                _logger.LogInformation("Registering profile {DatingProfileId} swipe: {SwipeState}, time spent (seconds): {timeSpentSeconds}", datingProfileId, request.SwipeState.ToString(), request.TimeSpentSeconds);

                await _datingProfileFeedbackService.RegisterSwipeAsync(new DatingProfileSwipe()
                {
                    DatingProfileId = datingProfileId,
                    SwipeState = request.SwipeState,
                    TimeSpentSeconds = request.TimeSpentSeconds
                });

                _logger.LogInformation("Succesfully registered profile {DatingProfileId} swipe: {SwipeState}, time spent (seconds): {timeSpentSeconds}", datingProfileId, request.SwipeState.ToString(), request.TimeSpentSeconds);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering profile swipe with data: {@ProfileSwipeRequest} for profile with ID {datingProfileId}", request, datingProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{datingProfileId}/reflect")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RegisterAnswersForProfile([FromRoute] Guid datingProfileId, [FromBody] ProfilePromptAnswersRequest request)
        {
            try
            {
                _logger.LogInformation("Registering answer for profile with Id {DatingProfileId}: {@Answers}", datingProfileId, request);

                var datingProfileReflection = new DatingProfileReflection()
                {
                    DatingProfileId = datingProfileId,
                    ChangedOpinion = request.ChangedOpinion,
                    PromptAnswers = request.PromptAnswers,
                    TimeSpentSeconds = request.TimeSpentSeconds,
                    ProfileReviewCount = request.ProfileReviewCount
                };

                await _datingProfileFeedbackService.RegisterReflectionAsync(datingProfileReflection);

                _logger.LogInformation("Succesfully registered answer for profile with Id {DatingProfileId}: {@Answers}", datingProfileId, request);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering profile reflection with data: {@ProfileSwipeRequest} for profile with ID {datingProfileId}", request, datingProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}