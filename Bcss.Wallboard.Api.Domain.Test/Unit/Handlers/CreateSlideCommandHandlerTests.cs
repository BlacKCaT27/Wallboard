using System.Threading;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data;
using Bcss.Wallboard.Api.Data.Entities;
using Bcss.Wallboard.Api.Domain.Commands;
using Bcss.Wallboard.Api.Domain.Handlers;
using Bcss.Wallboard.Api.Domain.Responses;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;

namespace Bcss.Wallboard.Api.Domain.Test.Unit.Handlers
{
    [Category("Unit")]
    public class CreateSlideCommandHandlerTests
    {
        private ISlideWriter _mockSlideWriter;

        [SetUp]
        public void Setup()
        {
            _mockSlideWriter = Substitute.For<ISlideWriter>();
        }

        [Test(Description = "It's helpful to have a simple test around constructors to ensure constructors don't do anything beyond property assignments (domain objects in a DDD architecture are an exception)")]
        public void CanCreateCreateSlideCommandHandler()
        {
            var handler = CreateHandler();
            handler.Should().BeOfType<CreateSlideCommandHandler>();
        }

        [Test]
        public async Task GivenProperCreateSlideCommandHandlerSuccessfullyCreatesSlide()
        {
            const int slideId = 1;
            const string slideName = "slideName";
            const string slideContent = "slideContent";

            var command = new CreateSlideCommand
            {
                Name = slideName,
                Content = slideContent
            };

            var expectedResult = new SlideResponse
            {
                Id = slideId
            };

            var createdSlide = new Slide
            {
                Id = slideId,
                Name = slideName,
                Content = slideContent
            };

            _mockSlideWriter.CreateSlideAsync(Arg.Is(slideName), Arg.Is(slideContent))
                .Returns(createdSlide);

            var handler = CreateHandler();

            var actualResult = await handler.Handle(command, new CancellationToken());

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private CreateSlideCommandHandler CreateHandler()
        {
            return new CreateSlideCommandHandler(_mockSlideWriter, new NullLogger<CreateSlideCommandHandler>());
        }
    }
}