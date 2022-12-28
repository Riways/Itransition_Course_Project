using totten_romatoes.Shared.Models;

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
        public const int AMOUNT_OF_TAGS_IN_SEARCH_RESULT = 20;

        //urls
        public const string SUBJECT_GRADE_URL = "api/subjects/grade";
        public const string REVIEW_URL = "api/reviews";
        public const string REVIEW_SEARCH_URL = $"{REVIEW_URL}/search";
        public const string REVIEW_ADD_FAKE_URL = $"{REVIEW_URL}/add-fakes";
        public const string TAG_URL = "api/tags";
        public const string TAG_SEARCH_URL = $"{TAG_URL}/search";

        //search
        public const int AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT = 10;

        //faker
        public const string FAKER_USER_ID = "6c6e3c1c-606b-4939-8235-9e0711f0df1d";
        public const int FAKER_MAX_WORDS_IN_TITLE = 3;
        public const int FAKER_MIN_SENTENCES_IN_BODY = 30;
        public const int FAKER_MAX_SENTENCES_IN_BODY = 100;
        public const int FAKER_MIN_TAGS_AMOUNT = 0;
        public const int FAKER_MAX_TAGS_AMOUNT = 5;
        public const int FAKER_MIN_WORDS_IN_TAG_AMOUNT = 1;
        public const int FAKER_MAX_WORDS_IN_TAG_AMOUNT = 2;
        public const int FAKER_IMAGE_HEIGHT = 1080;
        public const int FAKER_IMAGE_WIDTH = 1920;
    }
}
