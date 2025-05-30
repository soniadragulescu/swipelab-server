using System.Text;
using SwipeLab.Domain.DatingProfile.Utils;
using SwipeLab.Domain.DatingProfileFeedback;
using SwipeLab.Domain.Experiment;

namespace SwipeLab.Feedback.LargeLanguageModels;

public class LlmPromptBuilder : ILlmPromptBuilder
{
    public string BuildReflectiveQuestionsPrompt(ExperimentInteractionData experimentInteractionData)
    {
        StringBuilder prompt = new StringBuilder();

        prompt.AppendLine(
            "A user is navigating through profiles on a dating app. Here’s the user's swipe history and reflections:\n");
        prompt.AppendLine(string.Empty);
        prompt.AppendLine("The interactions in order:");
        prompt.AppendLine(string.Empty);

        var interactions = experimentInteractionData.DatingProfileInteractions.Select((value, i) => (value, i));
        var totalInteractions = interactions.Count();

        foreach (var (interaction, index) in interactions)
        {
            if (index == totalInteractions - 1)
            {
                prompt.AppendLine("Current profile:");
            }
            else
            {
                prompt.AppendLine($"Profile {index + 1}.");
            }

            var datingProfileString = GetDatingProfileSection(interaction);

            prompt.AppendLine(datingProfileString);
            prompt.AppendLine(string.Empty);
        }

        prompt.Append(GetReflectiveQuestionsPromptEndPhrase());

        return prompt.ToString();
    }

    private string GetDatingProfileSection(DatingProfileInteractionData datingProfileInteractionData)
    {
        var datingProfileSection = new StringBuilder();

        datingProfileSection.AppendLine("Name: " + datingProfileInteractionData.Profile.Name);
        datingProfileSection.AppendLine("Ethnicity: " + datingProfileInteractionData.Profile.Ethnicity.ToString());
        datingProfileSection.AppendLine("Age: " + datingProfileInteractionData.Profile.Age);
        datingProfileSection.AppendLine("Height: " + datingProfileInteractionData.Profile.Height);
        datingProfileSection.AppendLine("Gender: " + datingProfileInteractionData.Profile.Gender.ToString());
        datingProfileSection.AppendLine("LookingFor: " + EnumUtils.FormatEnumValue(datingProfileInteractionData.Profile.LookingFor));
        datingProfileSection.AppendLine("Drinking: " + EnumUtils.FormatEnumValue(datingProfileInteractionData.Profile.Drinking));
        datingProfileSection.AppendLine("Smoking: " + EnumUtils.FormatEnumValue(datingProfileInteractionData.Profile.Smoking));
        datingProfileSection.AppendLine("Kids preference: " + EnumUtils.FormatEnumValue(datingProfileInteractionData.Profile.KidsPreference));
        datingProfileSection.AppendLine("Education: " + EnumUtils.FormatEnumValue(datingProfileInteractionData.Profile.Education));
        datingProfileSection.AppendLine("Job: " + datingProfileInteractionData.Profile.Job);

        var hobbiesString = string.Join(", ", datingProfileInteractionData.Profile.Hobbies);

        datingProfileSection.AppendLine("Hobbies: " + hobbiesString);

        datingProfileSection.AppendLine();

        datingProfileSection.AppendLine("Profile decision: " + datingProfileInteractionData.SwipeInformation.SwipeState.ToString());


        if (datingProfileInteractionData.UserReflection?.PromptAnswers == null)
        {
            return datingProfileSection.ToString();
        }

        datingProfileSection.AppendLine();
        datingProfileSection.AppendLine("User was asked the following questions after making a decision:");
        datingProfileSection.AppendLine();

        foreach (var entry in datingProfileInteractionData.UserReflection.PromptAnswers)
        {
            datingProfileSection.AppendLine($"Q: {entry.Key}");
            datingProfileSection.AppendLine($"A: {entry.Value}");
        }

        return datingProfileSection.ToString();
    }

    private StringBuilder GetReflectiveQuestionsPromptEndPhrase()
    {
        var sb = new StringBuilder();
        sb.Append("You are a researcher/therapist who explores how people use dating apps and how technology influences romantic decision-making. ");
        sb.Append("Given this information, generate 1 or 2 short self-reflective questions to be sent to the user related to the current profile. ");
        sb.Append("The questions should be short, thoughtful, reflective and should point out different unconscious biases or filter choices. ");
        sb.Append("Try to not duplicate previous questions and to point out specific biases the user might have.");

        return sb;
    }
}
