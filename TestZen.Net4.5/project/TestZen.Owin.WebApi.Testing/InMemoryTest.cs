using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Owin.Testing;
using Moq;
using NUnit.Framework;

namespace TestZen.Owin.WebApi.Testing
{
    [TestFixture]
    public abstract class InMemoryTest<TStartup> where TStartup : WebApiStartup, IWebApiStartup, new()
    {
        private TestServer _testServer;
        private IDictionary<Type, Mock> _mocks;

        public WebApiStartup Startup { get; private set; }
        public HttpClient HttpClient { get; private set; }

        [SetUp]
        public void TestSetUp()
        {
            _testServer = TestServer.Create(app =>
            {
                Startup = new TStartup();
                Startup.Configuration(app);
            });

            _mocks = new ConcurrentDictionary<Type, Mock>();
            HttpClient = _testServer.HttpClient;

            StartupTasks();
        }

        /// <summary>
        /// Override to apply addional startup tasks
        /// </summary>
        public virtual void StartupTasks()
        {

        }


        [TearDown]
        public void TestTearDown()
        {
            _testServer.Dispose();
        }

        public Mock<T> MockOut<T>() where T : class
        {
            if (_mocks.ContainsKey(typeof(T)))
            {
                RebindToConfiguredMocks();
                return (Mock<T>)_mocks[typeof(T)];
            }

            _mocks.Add(typeof(T), new Mock<T>());
            RebindToConfiguredMocks();

            return (Mock<T>)_mocks[typeof(T)];
        }

        private void RebindToConfiguredMocks()
        {
            foreach (var mock in _mocks)
            {
                Startup.Container.Unbind(mock.Key);
                var item1 = mock;
                Startup.Container.Bind(mock.Key).ToMethod(_ => _mocks[item1.Key].Object);
            }
        }
    }
}
