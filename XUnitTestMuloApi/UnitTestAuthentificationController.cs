using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Controllers;
using MuloApi.Models;
using Xunit;

namespace XUnitTestMuloApi
{
    public class UnitTestAuthentificationController
    {
        private readonly ListResults _listResults = new ListResults();

        [Fact]
        public async void ConnectUserTest()
        {
            //Arrange
            var setTestDataUser = new[]
            {
                new ModelConnectingUser
                {
                    Login = "layon165@yandex.ru",
                    Password = "asdfqr"
                },
                new ModelConnectingUser
                {
                    Login = "layon16@yandex.ru",
                    Password = "asdfqr"
                },
                new ModelConnectingUser
                {
                    Login = "layon16@yandex.ru",
                    Password = "1234"
                },
                new ModelConnectingUser
                {
                    Login = "layon16@yandex,ru",
                    Password = "asdfqr"
                },
                new ModelConnectingUser
                {
                    Login = "layon16@yandex,ru",
                    Password = "1234"
                }
            };
            var resultsConnectUser = new List<JsonResult>();
            var authentificationController = new AuthentificationController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            //Act
            foreach (var dataUser in setTestDataUser)
            {
                var result = await authentificationController.ConnectUser(dataUser);
                resultsConnectUser.Add(result);
            }

            //Assert
            foreach (var result in resultsConnectUser)
            {
                Assert.NotNull(result);
                if (result.Value.ToString().Contains("user_id") && result.Value.ToString().Contains("login"))
                {
                    Assert.Matches(@"^\{\s[u,s,e,r]{4}_[a-zA-Z]{2}\s\=\s[0-9]+\,\s[l,o,g,i,n]{5}\s\=\s.*\s\}$",
                        result.Value.ToString());
                    continue;
                }

                result.AssertContains(_listResults.MethodConnectUser());
            }
        }

        [Fact]
        public async void CreateUserTest()
        {
            //Arrange
            var setTestDataUser = new[]
            {
                new ModelConnectingUser
                {
                    Login = "layon16@yandex.ru",
                    Password = "asdfqr"
                },
                new ModelConnectingUser
                {
                    Login = "layon16@yandex.ru",
                    Password = "1234"
                },
                new ModelConnectingUser
                {
                    Login = "layon16@yandex,ru",
                    Password = "asdfqr"
                },
                new ModelConnectingUser
                {
                    Login = "layon16@yandex,ru",
                    Password = "1234"
                }
            };
            var resultsCreateUser = new List<JsonResult>();
            var authentificationController = new AuthentificationController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            //Act
            foreach (var dataUser in setTestDataUser)
            {
                var result = await authentificationController.CreateUser(dataUser);
                resultsCreateUser.Add(result);
            }

            // Assert
            foreach (var result in resultsCreateUser)
            {
                Assert.NotNull(result);
                if (result.Value.ToString().Contains("user_id") && result.Value.ToString().Contains("login"))
                {
                    Assert.Matches(@"^\{\s[u,s,e,r]{4}_[a-zA-Z]{2}\s\=\s[0-9]+\,\s[l,o,g,i,n]{5}\s\=\s.*\s\}$",
                        result.Value.ToString());
                    continue;
                }

                result.AssertContains(_listResults.MethodCreateUser());
            }
        }
    }
}