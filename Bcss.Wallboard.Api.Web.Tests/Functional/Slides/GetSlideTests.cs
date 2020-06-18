using System.Net;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Domain.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bcss.Wallboard.Api.Web.Tests.Functional.Slides
{
    [Category("Functional")]
    public class GetSlideTests
    {
        private WebApplicationFactory<Startup> _factory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            _factory.Dispose();
        }

        [Test]
        public async Task GivenSlideWithIdExistsGetSlideReturnsExpectedSlide()
        {
            // Arrange
            var expectedResult = new SlideResponse
            {
                Id = 1,
                Name = "Sample",
                Content = "<h1>My Sample Slide</h1><p>This is a sample slide containing some simple html as an example of what type of content a slide could have.</p>"
            };

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/slides/1");

            // Assert
            response.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var responseBody = await response.Content.ReadAsStringAsync();

            var slideResponse = JsonConvert.DeserializeObject<SlideResponse>(responseBody);
            slideResponse.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GivenSlideIdThatDoesNotExistGetSlideReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/slides/2");

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}