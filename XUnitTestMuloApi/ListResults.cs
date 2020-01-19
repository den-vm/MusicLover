namespace XUnitTestMuloApi
{
    internal class ListResults : IQueryResults
    {
        public string[] MethodConnectUser()
        {
            string[] listResults =
            {
                new
                {
                    errors = new
                    {
                        message = "INCORRECT_PASSWORD_OR_LOGIN"
                    }
                }.ToString()
            };
            return listResults;
        }

        public string[] MethodCreateUser()
        {
            string[] listResults =
            {
                new
                {
                    errors = new
                    {
                        message = "INCORRECT_LOGIN"
                    }
                }.ToString(),
                new
                {
                    errors = new
                    {
                        message = "INCORRECT_PASSWORD"
                    }
                }.ToString(),
                new
                {
                    errors = new
                    {
                        message = "EXISTING_USER"
                    }
                }.ToString(),
                new
                {
                    error = "ERRORSERVER"
                }.ToString()
            };
            return listResults;
        }
    }
}