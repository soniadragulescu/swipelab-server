using SwipeLab.Domain.Experiment;

namespace SwipeLab.Feedback;

public class PredefinedUserPromptProvider : IUserPromptProvider
{
    public Task<List<string>> GetUserReflectiveQuestionsAsync(ExperimentInteractionData experimentInteractionData)
    {
        var totalCount = experimentInteractionData.TotalProfileCount;
        var swipedCount = experimentInteractionData.DatingProfileInteractions.Count;

        return Task.FromResult(GetUserPromptsPerProfileAsync(totalCount - swipedCount));
    }

    public List<string> GetUserPromptsPerProfileAsync(int promptedDatingProfileIndex)
    {
        var modulo = promptedDatingProfileIndex % 5;

        switch (modulo)
        {
            case 4:
                return
                [
                    "Do you notice any pattern in your swiping?"
                ];
            case 3:
                return
                [
                    "What are some things that you appreciate about this profile?"
                ];
            case 2:
                return
                [
                    "What are some things that you do not like about this profile?"
                ];
            case 1:
                return
                [
                    "Do you think this person would be enjoyable to be around? Why? Why not?"
                ];
            case 0:
            default:
                return
                [
                    "What made you swipe this way?"
                ];
        }
    }
}