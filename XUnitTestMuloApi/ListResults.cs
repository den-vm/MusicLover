using Microsoft.AspNetCore.Mvc;
using TagLib.Riff;

namespace XUnitTestMuloApi
{
    internal class ListResults : IQueryResults
    {
        private static ListResults _instance;

        public static ListResults GetListResults()
        {
            return _instance ??= new ListResults();
        }

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

        public JsonResult[] MethodGetSoundTracksUser()
        {
            var listResults = new[]
            {
                new JsonResult(new
                    {
                        error = "ERRORSERVER"
                    })
                    {StatusCode = 500},
                new JsonResult(new
                    {
                        tracks = "empty"
                    })
                    {StatusCode = 200}
            };
            return listResults;
        }

        public JsonResult[] MethodUploadSoundTrack()
        {
            var listResults = new[]
            {
                new JsonResult(new
                    {
                        error = "ERRORSERVER"
                    })
                    {StatusCode = 500}
            };
            return listResults;
        }
    }
}