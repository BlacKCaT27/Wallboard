using System.Threading;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data;
using Bcss.Wallboard.Api.Data.Entities;
using Bcss.Wallboard.Api.Domain.Handlers;
using Bcss.Wallboard.Api.Domain.Queries;
using Bcss.Wallboard.Api.Domain.Responses;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace Bcss.Wallboard.Api.Domain.Test.Unit.Handlers
{
    [Category("Unit")]
    public class GetSlideByIdQueryHandlerTests
    {
        private ISlideReader _mockSlideReader;

        [SetUp]
        public void Setup()
        {
            _mockSlideReader = Substitute.For<ISlideReader>();
        }

        [Test]
        public void CanCreateGetSlideByIdQueryHandler()
        {
            var handler = CreateHandler();
            handler.Should().BeOfType<GetSlideByIdQueryHandler>();
        }

        [Test]
        public async Task GivenSlideExistsQueryHandlerReturnsSlide()
        {
            const int slideId = 1;
            const string slideName = "slideName";
            const string slideContent = "slideContent";

            var slide = new Slide
            {
                Id = slideId,
                Name = slideName,
                Content = slideContent
            };

            _mockSlideReader.GetSlideAsync(Arg.Is(slideId))
                .Returns(slide);

            var expectedResult = new SlideResponse
            {
                Id = slideId,
                Name = slideName,
                Content = slideContent
            };

            var request = new GetSlideByIdQuery(slideId);

            var handler = CreateHandler();

            var actualResult = await handler.Handle(request, new CancellationToken());

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GivenSlideDoesNotExistQueryHandlerReturnsNull()
        {
            const int slideId = 1;
            const string slideName = "slideName";
            const string slideContent = "slideContent";

            _mockSlideReader.GetSlideAsync(Arg.Is(slideId))
                .ReturnsNull();

            var request = new GetSlideByIdQuery(slideId);

            var handler = CreateHandler();

            var actualResult = await handler.Handle(request, new CancellationToken());

            actualResult.Should().BeNull();
        }

        private GetSlideByIdQueryHandler CreateHandler()
        {
            return new GetSlideByIdQueryHandler(_mockSlideReader, new NullLogger<GetSlideByIdQueryHandler>());
        }
    }
}