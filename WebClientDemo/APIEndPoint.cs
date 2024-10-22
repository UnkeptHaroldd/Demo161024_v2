namespace WebClientDemo
{
    public class APIEndPoint
    {
        public const string BaseURL = "http://localhost:5237";

        public const string SignIn = BaseURL + "/api/Accounts/SignIn";

        public const string AddBook = BaseURL + "/api/Books";

        public const string EditBook = BaseURL + "/api/Books/";

        public const string GetBookList = BaseURL + "/api/Books";

        public const string GetBookByID = BaseURL + "/api/Books/";

        public const string DeleteBook = BaseURL + "/api/Books/";
    }
}
