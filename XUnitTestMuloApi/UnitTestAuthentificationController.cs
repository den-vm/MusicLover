using System.Collections.Generic;
using System.Data;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuloApi.Controllers;
using MuloApi.Models;
using Xunit;

namespace XUnitTestMuloApi
{
    public class UnitTestAuthentificationController
    {
        private readonly ListResults _resultConnectUser = new ListResults();

        [Fact]
        public async void ConnectUserTest()
        {
            //Arrange
            var autControl = new AuthentificationController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            //Act
            var dataUserArray = new[]
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
            var showsMethod = new List<string>();
            foreach (var _dataUser in dataUserArray)
            {
                var result = await autControl.ConnectUser(_dataUser);
                showsMethod.Add(result.Value.ToString());
            }



            // Assert
            foreach (var resMethod in showsMethod)
            {
                Assert.NotNull(resMethod);
                if (resMethod.Contains("user_id") && resMethod.Contains("login"))
                {
                    Assert.Matches(@"^\{\s[u,s,e,r]{4}_[a-zA-Z]{2}\s\=\s[0-9]+\,\s[l,o,g,i,n]{5}\s\=\s.*\s\}$",
                        resMethod);
                    continue;
                }

                Assert.Contains(resMethod, _resultConnectUser.MethodConnectUser());
            }
        }

        [Fact]
        public async void CreateUserTest()
        {
            //Arrange
            var autControl = new AuthentificationController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            //Act
            var dataUserArray = new[]
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
            var showsMethod = new List<string>();
            
            foreach (var _dataUser in dataUserArray)
            {
                var result = await autControl.CreateUser(_dataUser);
                showsMethod.Add(result.Value.ToString());
            }


            // Assert
            foreach (var resMethod in showsMethod)
            {
                Assert.NotNull(resMethod);
                if (resMethod.Contains("user_id") && resMethod.Contains("login"))
                {
                    Assert.Matches(@"^\{\s[u,s,e,r]{4}_[a-zA-Z]{2}\s\=\s[0-9]+\,\s[l,o,g,i,n]{5}\s\=\s.*\s\}$",
                        resMethod);
                    continue;
                }

                Assert.Contains(resMethod, _resultConnectUser.MethodCreateUser());
            }
        }
    }
}