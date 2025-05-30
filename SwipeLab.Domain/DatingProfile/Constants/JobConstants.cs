using SwipeLab.Domain.DatingProfile.Enums;

namespace SwipeLab.Domain.DatingProfile.Constants
{
    public static class JobConstants
    {
        public static List<string> GetAllowedJob(Education education)
        {
            return education switch
            {
                Education.MASTER or Education.PHD => CommonJobs.Concat(HigherEducationJobs).ToList(),
                _ => CommonJobs
            };
        }

        public static readonly List<string> CommonJobs = new()
        {
            "Student",
            "Freelancer",
            "Unemployed",
            "Barista",
            "Retail Associate",
            "Customer Service Representative",
            "Electrician",
            "Plumber",
            "Chef",
            "Photographer",
            "Personal Trainer",
            "Construction Worker",
            "Software Engineer",
            "Marketing Specialist",
            "Administrative Assistant",
            "Graphic Designer",
            "Business Analyst",
            "Accountant",
            "Human Resources Manager",
            "Sales Manager",
            "Data Scientist",
            "Project Manager"
        };

        public static readonly List<string> HigherEducationJobs = new()
        {
            "Architect",
            "Lawyer",
            "Kinetotherapist",
            "Dermatologist",
            "Dentist",
            "Research Scientist",
            "University Teacher",
            "Pharmacist",
            "Veterinarian",
            "Environmental Scientist",
            "Aerospace Engineer",
            "Biomedical Engineer",
            "Psychologist"
        };

    }
}
