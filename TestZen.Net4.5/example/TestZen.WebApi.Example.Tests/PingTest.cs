using System.Net;
using NUnit.Framework;
using TestZen.Owin.WebApi;
using TestZen.Owin.WebApi.Testing;


namespace TestZen.WebApi.Example.Tests
{
    [TestFixture]
    public class PingTest : InMemoryTest<Startup>
    {
        [Test]
        public void Get_Returns200()
        {
            var response = HttpClient.GetAsync("ping").Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
