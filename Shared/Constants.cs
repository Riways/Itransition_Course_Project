namespace totten_romatoes.Shared
{
    public static class Constants
    {
        //image
        public const int MAX_IMAGE_SIZE = 2 * 1024 * 1024;
        public const string IMAGE_FORMAT = "image/jpeg";
        //tags
        public const int MAX_AMOUNT_OF_TAGS_IN_REVIEW = 15;
        public const int MAX_TAG_LENGTH = 40;
        public const int MIN_TAG_LENGTH = 3;
        public const int AMOUNT_OF_TAGS_IN_CLOUD = 20;

        //urls
        public const string SUBJECT_GRADE_URL = "api/subjects/grade";
        public const string REVIEW_URL = "api/reviews";
    }
}
