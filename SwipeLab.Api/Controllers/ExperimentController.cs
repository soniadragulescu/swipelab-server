using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SwipeLab.Domain.Experiment;
using SwipeLab.Domain.Participant;
using SwipeLab.Domain.Question;
using SwipeLab.Model.Requests;
using SwipeLab.Services.Interfaces.Interfaces;

namespace SwipeLab.Controllers;

[ApiController]
[Route("experiment")]
public class ExperimentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<ExperimentController> _logger;
    private readonly IExperimentService _experimentService;

    public ExperimentController(IMapper mapper, ILogger<ExperimentController> logger, IExperimentService experimentService)
    {
        _mapper = mapper;
        _logger = logger;
        _experimentService = experimentService;
    }

    [HttpGet("{experimentId}")]
    [ProducesResponseType(typeof(Experiment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Experiment>> GetExperiment([FromRoute] Guid experimentId)
    {
        try
        {
            _logger.LogInformation("Getting experiment with ID: {ExperimentId}", experimentId);

            var experiment = await _experimentService.GetExperiment(experimentId);

            if (experiment == null)
            {
                _logger.LogWarning("Experiment with ID: {ExperimentId} not found", experimentId);
                return NotFound();
            }

            _logger.LogInformation("Experiment with ID: {ExperimentId} found", experimentId);
            return Ok(experiment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving experiment with ID: {ExperimentId}", experimentId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving the experiment.");
        }
    }

    [HttpPost("new")]
    [ProducesResponseType(typeof(Experiment), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Experiment>> StartExperiment([FromBody] ExperimentCreateRequest request)
    {
        try
        {
            _logger.LogInformation("Creating experiment...");

            var participant = _mapper.Map<Participant>(request);
            var experiment = await _experimentService.CreateExperiment(participant, request.OnboardingConfidence, request.OnboardingComfortable);

            _logger.LogInformation("Experiment succesfully created with ID {ExperimentId} and type {Type}.", experiment.ExperimentId, experiment.Type.ToString());
            return CreatedAtAction(
               nameof(GetExperiment), new { experimentId = experiment.ExperimentId }, experiment
           );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating experiment with data: {@Experiment}", request);
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while creating the experiment.");
        }
    }

    [HttpPost("{experimentId}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Experiment>> CompleteExperiment([FromRoute] Guid experimentId, [FromBody] List<QuestionAnswerItemRequest> request)
    {
        try
        {
            _logger.LogInformation("Getting questions for experiment with ID: {ExperimentId}", experimentId);

            var answers = request.Select(a => new QuestionAnswer
            {
                QuestionNumber = a.QuestionNumber,
                ExperimentId = experimentId,
                Text = a.Text,
                Answer = a.Answer
            }).ToList();

            var result = await _experimentService.CompleteExperimentAsync(experimentId, answers);

            if (!result)
            {
                _logger.LogError("Answers for experiment with ID: {ExperimentId} could not be saved", experimentId);
                return NotFound();
            }

            _logger.LogInformation("Answers for experiment with ID: {ExperimentId} saved succesfully1", experimentId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving answers to final questions for experiment with ID: {ExperimentId}", experimentId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving questions for the experiment.");
        }
    }
}