using System.Net;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace Bcss.Wallboard.Api.Web.Tests.Functional.Slides
{
    [Category("Functional")]
    public class DeleteSlideTests
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
        public async Task GivenExistingSlideIdDeleteCommandSuccessfullyDeletesSlide()
        {
            // Arrange
            const int slideId = 1;
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/v1/slides/{slideId}");

            // Assert
            response.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }

        [Test]
        public async Task GivenMultipleDeleteCommandsForTheSameIdDeleteSlidesReturnsSuccess()
        {
            // Arrange
            const int slideId = 1;
            var client = _factory.CreateClient();

            // Act
            var firstResponse = await client.DeleteAsync($"api/v1/slides/{slideId}");
            var secondResponse = await client.DeleteAsync($"api/v1/slides/{slideId}");
            var thirdResponse = await client.DeleteAsync($"api/v1/slides/{slideId}");

            // Assert
            firstResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
            secondResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
            thirdResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task GivenDeleteCommandWhenErrorOccursProcessingRequestDeleteSlideReturnsError()
        {
            // Arrange
            const int slideId = 1;
            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    // Use a mock to simulate an internal server issue occuring while deleting the record.
                    var mockSlideWriter = Substitute.For<ISlideWriter>();
                    mockSlideWriter.DeleteSlideAsync(Arg.Is(slideId))
                        .Returns(false);
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped(f => mockSlideWriter);
                    });
                })
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/v1/slides/{slideId}");

            // Assert
            response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}