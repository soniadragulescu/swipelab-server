using AutoMapper;
using SwipeLab.Model.Requests;
using Participant = SwipeLab.Domain.Participant.Participant;

namespace SwipeLab.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ExperimentCreateRequest, Participant>();
        }
    }
}