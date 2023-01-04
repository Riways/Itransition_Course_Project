using totten_romatoes.Shared.Models;

namespace totten_romatoes.Shared
{
    public static class Constants
    {
        public const string APP_NAME = "Romatoes";

        //pagination
        public const int REVIEWS_ON_HOME_PAGE = 10;

        //dropbox
        public const string DROPBOX_PATH = "/romatoes/";

        //image
        public const int MAX_IMAGE_SIZE = 2 * 1024 * 1024;
        public const string IMAGE_FORMAT = "image/jpeg";

        //tags
        public const int MAX_AMOUNT_OF_TAGS_IN_REVIEW = 15;
        public const int MAX_TAG_LENGTH = 40;
        public const int MIN_TAG_LENGTH = 3;
        public const int AMOUNT_OF_TAGS_IN_CLOUD = 20;
        public const int AMOUNT_OF_TAGS_IN_SEARCH_RESULT = 20;

        //api urls
        public const string SUBJECT_GRADE_URL = "api/subjects/grade";
        public const string REVIEWS_URL = "api/reviews";
        public const string REVIEWS_LIGHTWEIGHT_CHUNK_URL = $"{REVIEWS_URL}/lightweight-chunk";
        public const string REVIEWS_CHUNK_URL = $"{REVIEWS_URL}/chunk";
        public const string REVIEWS_SEARCH_URL = $"{REVIEWS_URL}/search";
        public const string REVIEWS_AMOUNT_URL = $"{REVIEWS_URL}/amount";
        public const string REVIEWS_ADD_FAKE_URL = $"{REVIEWS_URL}/add-fakes";
        public const string REVIEWS_ADD_COMMENT_URL = $"{REVIEWS_URL}/add-comment";
        public const string REVIEWS_LIKE_URL = $"{REVIEWS_URL}/like";
        public const string TAG_URL = "api/tags";
        public const string TAG_SEARCH_URL = $"{TAG_URL}/search";
        public const string TAG_GET_DEFAULT_AMOUNT_URL = $"{TAG_URL}/take";

        //search
        public const int AMOUNT_OF_REVIEWS_IN_SEARCH_RESULT = 10;

        //faker
        public const string FAKER_USER_ID = "6c6e3c1c-606b-4939-8235-9e0711f0df1d";
        public const int FAKER_MAX_WORDS_IN_TITLE = 3;
        public const int FAKER_MIN_SENTENCES_IN_BODY = 30;
        public const int FAKER_MAX_SENTENCES_IN_BODY = 100;
        public const int FAKER_MIN_COMMENTS_AMOUNT = 10;
        public const int FAKER_MAX_COMMENTS_AMOUNT = 100;
        public const int FAKER_MIN_SENTENCES_IN_COMMENT_AMOUNT = 1;
        public const int FAKER_MAX_SENTENCES_IN_COMMENT_AMOUNT = 4;
        public const int FAKER_MIN_TAGS_AMOUNT = 0;
        public const int FAKER_MAX_TAGS_AMOUNT = 5;
        public const int FAKER_MIN_WORDS_IN_TAG_AMOUNT = 1;
        public const int FAKER_MAX_WORDS_IN_TAG_AMOUNT = 2;
        public const int FAKER_IMAGE_HEIGHT = 720;
        public const int FAKER_IMAGE_WIDTH = 1280;
    }
}
