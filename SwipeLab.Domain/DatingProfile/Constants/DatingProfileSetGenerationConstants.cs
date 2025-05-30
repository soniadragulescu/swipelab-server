namespace SwipeLab.Domain.DatingProfile.Constants
{
    public static class DatingProfileSetGenerationConstants
    {
        #region set characteristics
        public static int SET_SIZE = 20;
        public static double ETHNICITY_DISTRIBUTION = 0.25;
        public static double GENDER_DISTRIBUTION = 0.5;
        #endregion set characteristics

        #region profile attributes
        public static int HOBBIES_COUNT_LOWER_LIMIT = 3;
        public static int HOBBIES_COUNT_UPPER_LIMIT = 5;
        public static int PROMPTS_PER_PROFILE = 3;
        #endregion profile attributes

        #region heigh
        //from the global distribution based on gender
        public static double HEIGHT_STD_DEV = 7.6;
        public static double MALE_HEIGHT_MEAN = 175.3; // in cm
        public static double FEMALE_HEIGHT_MEAN = 162.6; // in cm
        #endregion height
    }
}
