using Microsoft.AspNetCore.Mvc;
using SwipeLab.Model.Responses.Feedback;
using SwipeLab.Services.Interfaces.Interfaces;

namespace SwipeLab.Controllers
{
    [Route("feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger<FeedbackController> _logger;
        private readonly IPromptService _promptService;

        public FeedbackController(ILogger<FeedbackController> logger, IPromptService promptService)
        {
            _logger = logger;
            _promptService = promptService;
        }

        [HttpGet("prompts/{experimentId}")]
        [ProducesResponseType(typeof(SwipingFeedbackPrompts), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SwipingFeedbackPrompts>> GetPromptsForExperiment([FromRoute] Guid experimentId)
        {
            try
            {
                _logger.LogInformation("Getting prompts for experiment with ID: {ExperimentId}", experimentId);

                var response = await _promptService.GetPromptsForExperimentAsync(experimentId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving experiment with ID: {ExperimentId}", experimentId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the experiment.");
            }
        }
    }
}
