using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace OwinServer.Test
{
    [TestClass]
    public class AuthorizationTests : TestBase
    {
        [TestMethod]
        public async Task AccessPublicController_Works()
        {
            using (var client = CreateTestClient())
            {
                var result = await client.GetAsync("restricted");
                result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
