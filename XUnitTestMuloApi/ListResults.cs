using Microsoft.AspNetCore.Mvc;

namespace XUnitTestMuloApi
{
    internal class ListResults : IQueryResults
    {
        public JsonResult[] MethodConnectUser()
        {
            var listResults = new[]
            {
                new JsonResult(new
                {
                    error = "ERRORSERVER"
                }) {StatusCode = 521},
                new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_PASSWORD_OR_LOGIN"
                    }
                }) {StatusCode = 401}
            };
            return listResults;
        }

        public JsonResult[] MethodCreateUser()
        {
            var listResults = new[]
            {
                new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_LOGIN"
                    }
                }) {StatusCode = 401},
                new JsonResult(new
                {
                    errors = new
                    {
                        message = "INCORRECT_PASSWORD"
                    }
                }) {StatusCode = 401},
                new JsonResult(new
                {
                    errors = new
                    {
                        message = "EXISTING_USER"
                    }
                }) {StatusCode = 401},
                new JsonResult(new
                {
                    error = "ERRORSERVER"
                }) {StatusCode = 521},
                new JsonResult(new
                {
                    error = "ERRORSERVER"
                }) {StatusCode = 500}
            };
            return listResults;
        }
    }
}