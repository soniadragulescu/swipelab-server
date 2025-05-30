using Microsoft.Extensions.Logging;
using SwipeLab.Data;
using SwipeLab.Domain.DatingProfileFeedback;
using SwipeLab.Services.Interfaces.Interfaces;

namespace SwipeLab.Services.Services
{
    public class DatingProfileFeedbackService : IDatingProfileFeedbackService
    {
        private readonly ILogger<DatingProfileFeedbackService> _logger;

        private readonly IDatingProfileFeedbackRepository _datingProfileFeedbackRepository;
        private readonly IExperimentService _experimentService;

        public DatingProfileFeedbackService(ILogger<DatingProfileFeedbackService> logger,
            IDatingProfileFeedbackRepository datingProfileFeedbackRepository,
            IExperimentService experimentService)
        {
            _logger = logger;
            _datingProfileFeedbackRepository = datingProfileFeedbackRepository;
            _experimentService = experimentService;
        }

        public async Task RegisterSwipeAsync(DatingProfileSwipe datingProfileSwipe)
        {
            if (datingProfileSwipe.DatingProfileId == null || datingProfileSwipe.DatingProfileId == Guid.Empty)
            {
                throw new ArgumentException("DatingProfileId cannot be null");
            }

            var existingSwipe =
                await _datingProfileFeedbackRepository.GetDatingProfileSwipeByDatingProfileId(
                    (Guid)datingProfileSwipe.DatingProfileId);

            if (existingSwipe != null)
            {
                throw new ArgumentException($"Profile {datingProfileSwipe.DatingProfileId} has already been swiped");
            }

            await _datingProfileFeedbackRepository.SaveDatingProfileSwipe(datingProfileSwipe);
        }

        public async Task RegisterReflectionAsync(DatingProfileReflection datingProfileReflection)
        {
            if (datingProfileReflection.DatingProfileId == null ||
                datingProfileReflection.DatingProfileId == Guid.Empty)
            {
                throw new ArgumentException("DatingProfileId cannot be null");
            }

            var existingReflection =
                await _datingProfileFeedbackRepository.GetDatingProfileReflectionByDatingProfileId(
                    (Guid)datingProfileReflection.DatingProfileId);

            if (existingReflection != null)
            {
                throw new ArgumentException($"Profile {datingProfileReflection.DatingProfileId} has already been reflected");
            }

            await _datingProfileFeedbackRepository.SaveDatingProfileReflection(datingProfileReflection);


            var hasBeenCompleted = await _experimentService.UpdateExperimentStateIfNeeded((Guid)datingProfileReflection.DatingProfileId);
            if (hasBeenCompleted)
            {
                _logger.LogInformation("Experiment associated to dating profile {DatingProfileId} has been completed", datingProfileReflection.DatingProfileId);
            }
            else
            {
                _logger.LogInformation("Experiment associated to dating profile {DatingProfileId} has not been completed", datingProfileReflection.DatingProfileId);
            }
        }
    }
}