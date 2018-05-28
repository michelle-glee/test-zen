# TestZen.Owin.WebApi
Set up your WebApi with an OWIN pipeline and easily unit test by mocking out your dependancies

Use
===
To use TestZen.Owin.WebApi:
* Add to your WebApi projects
* Inherit your Owin WebApi Startup class from WebApiStartup
* Configure your WebApi Startup class to use Ninject and OWIN (see below) 


```C#

[assembly: OwinStartup(typeof(Startup))]

namespace MyProject.WebApi.Gateway
{
	public class Startup : WebApiStartup
	{
	  public override void Configuration(IAppBuilder app)
	  {
			...
			//Load your Ninject Modules for IoC
			Container.Load(new MyNinjectModule());
			
			//Do your normal Startup stuff 
			var config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			
			//Initialise the Ninject middleware
			app.UseNinjectMiddleware(() => Container);
			
			//Add your middleware to the pipeline
			app.UseMyCustomOwinMiddleware();
			
			//Register the WebApi through Ninject
			app.UseNinjectWebApi(config);
			
			...
	  }
	}
}
```

Test
===

To unit test your Owin pipeline, reference TestZen.Owin.WebApi.Testing your test projects.


To create a unit test base for your service, you can inherit from InMemoryTest<TStartup>.
This can be handy to mock out interfaces you will encounter in ever test but you dont need verify, like logging or metrics

```C#

[TestFixture]
public abstract class MyParentTestClass<TStartup> : InMemoryTest<TStartup> where TStartup : WebApiStartup, IWebApiStartup, new()
{
	public override void StartupTasks()
	{
		MockOut<IMetrics>();
		MockOut<ILogger>();
	}
}

```

In your unit tests you can call endpoints and test their behaviours, for example:

```C#

[TestFixture]
public class MyTests : MyParentTestClass<Startup>
{
	[Test]
	public void Post_EmptyRequestBody_ReturnsBadRequest()
	{
		var emptyBody = new BodyContent("{ }");

		using (var response = HttpClient.PostAsync("my-endpoint-uri", emptyBody).Result)
		{
			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}
	
```

You can mock out interface dependancies declared in your service, like:

```C#
[Test]
public void Get_UnknownId_ReturnsNotFound()
{
	MockOut<IUserRepository>();

	using (var response = HttpClient.GetAsync("users/123456", body).Result)
	{
		Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
	}
}

[Test]
public void Post_NewUser_ReturnsCreatedAndSavesUser()
{
	var userRepository = MockOut<IUserRepository>();

	userRepository.Setup(repo => repo.Insert(It.IsAny<User>()).Returns(new User());

	var body = new BodyContent("{ name:'Test User', emailAddress:'test@domain.com' }");

	using (var response = HttpClient.PostAsync("user", body).Result)
	{
		Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

		userRepository.Verify(u => u.Insert(It.IsAny<User>()), Times.Once);
	}
}
``` 

You can also test to make sure your response objects contain the expected values

```C#
[Test]
public void Get_ValidUserId_ReturnsCorrectDetails()
{
	var repo = MockOut<IUserRepository>();

	repo.Setup(r => r.Get(333)).Returns(new UserModel
			{
				Id = 333,
				FirstName = "Grace",
				Surname = "Hopper"
			}
		);

	using (var response = HttpClient.GetAsync("users/333").Result)
	{
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

		var message = response.Content.ReadAsStringAsync().Result;
		var transaction = JsonConvert.DeserializeObject<UserContract>(message);
	
		Assert.AreEqual(333, user.UserNumber, "user id mapped to contract");
		Assert.AreEqual("Grace Hopper", user.FullName, "names mapped to contract");
	
	}
}

```

Creating extension methods can be handy for setup code you write often. 
You can also access the Ninject Container and the HttpClient on the InMemoryTest

```C#
public static class InMemoryTestExtention
{
	public static void SetupActiveSubscription<T>(this InMemoryTest<T> test)
	 where T : WebApiStartup, IWebApiStartup, new()
	{
		var subscription = new SubscriptionModel
		{
			SubscriptionStatus = SubscriptionStatus.Active
		};

		test.MockOut<ISubscriptionRepository>().Setup(s => s.Get(It.IsAny<Guid>())).Returns(subscription);
	}
	
	public static void AddAccessTokenToHeader<T>(this InMemoryTest<T> test)
	where T : WebApiStartup, IWebApiStartup, new()
	{
		var tokenService = test.Startup.Container.Get<IJwtTokenService>();
		var accessToken = tokenService.GenerateAccessToken();

		test.HttpClient.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization, accessToken);
	}
}		
```

Enjoy the freedom of testing at your boundaries! Refactor away at the internal implementation and you wont need to modify your unit tests :)


