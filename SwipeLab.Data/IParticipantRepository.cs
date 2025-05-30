using SwipeLab.Domain.Participant;

namespace SwipeLab.Data
{
    public interface IParticipantRepository
    {
        Task<Guid> CreateParticipant(Participant participant);
        Task<Participant?> GetParticipant(Guid participantId);
    }
}
