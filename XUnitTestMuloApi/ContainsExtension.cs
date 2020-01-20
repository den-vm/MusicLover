using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Sdk;

namespace XUnitTestMuloApi
{
    public static class ContainsExtension
    {
        public static void AssertContains(this JsonResult current, JsonResult[] other)
        {
            if (other.Any(jsonresult => current.Value.ToString().Equals(jsonresult.Value.ToString()) &&
                                        current.StatusCode.Equals(jsonresult.StatusCode)))
                return;

            Assert.False(true, new ContainsException(current, other).ToString());
        }
    }
}