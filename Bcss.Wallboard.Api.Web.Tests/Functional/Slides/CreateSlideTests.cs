using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Domain.Commands;
using Bcss.Wallboard.Api.Domain.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bcss.Wallboard.Api.Web.Tests.Functional.Slides
{
    [Category("Functional")]
    public class CreateSlideTests
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
        public async Task GivenValidCreateCommandCreateSlideCreatesSlideSuccessfully()
        {
            // Arrange
            const string slideName = "slideName";
            const string slideContent = "slideContent";
            const string endpoint = "api/v1/slides/";

            const int expectedId = 2;

            var command = new CreateSlideCommand
            {
                Name = slideName,
                Content = slideContent
            };

            var expectedResult = new SlideResponse
            {
                Id = expectedId,
                Name = slideName,
                Content = slideContent
            };

            var client = _factory.CreateClient();

            var expectedLocation = client.BaseAddress + endpoint + expectedId;

            var json = JsonConvert.SerializeObject(command);
            var requestContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act
            var response = await client.PostAsync(endpoint, requestContent);

            // Assert
            response.StatusCode.Should().Be((int) HttpStatusCode.Created);
            response.Headers.Location.AbsoluteUri.Should().BeEquivalentTo(expectedLocation);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().NotBeNullOrEmpty();

            var slideResponse = JsonConvert.DeserializeObject<SlideResponse>(responseBody);
            slideResponse.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GivenCreateSlideCommandWithMissingNameCreateSlideReturnsBadRequest()
        {
            // Arrange
            const string slideContent = "slideContent";
            const string endpoint = "api/v1/slides/";

            var command = new CreateSlideCommand
            {
                Content = slideContent
            };

            var client = _factory.CreateClient();

            var json = JsonConvert.SerializeObject(command);
            var requestContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act
            var response = await client.PostAsync(endpoint, requestContent);

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GivenCreateSlideCommandWithMissingContentCreateSlideReturnsBadRequest()
        {
            // Arrange
            const string slideName = "slideName";
            const string endpoint = "api/v1/slides/";

            var command = new CreateSlideCommand
            {
                Name = slideName
            };

            var client = _factory.CreateClient();

            var json = JsonConvert.SerializeObject(command);
            var requestContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act
            var response = await client.PostAsync(endpoint, requestContent);

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}