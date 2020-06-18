using System.Threading;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data;
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
    public class DeleteSlideCommandHandlerTests
    {
        private ISlideWriter _mockSlideWriter;

        [SetUp]
        public void Setup()
        {
            _mockSlideWriter = Substitute.For<ISlideWriter>();
        }

        [Test]
        public void CanCreateDeleteSlideCommandHandler()
        {
            var handler = CreateHandler();
            handler.Should().BeOfType<DeleteSlideCommandHandler>();
        }

        [Test]
        public async Task GivenValidDeleteSlideRequestHandlerReportsSuccessfulDeletion()
        {
            const int slideId = 1;

            _mockSlideWriter.DeleteSlideAsync(Arg.Is(slideId))
                .Returns(true);

            var expectedResult = new SlideResponse
            {
                Id = slideId
            };

            var handler = CreateHandler();

            var command = new DeleteSlideCommand(slideId);

            var actualResult = await handler.Handle(command, new CancellationToken());

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GivenInvalidIdDeleteSlideRequestHandlerReportsNoRecordChanged()
        {
            const int slideId = 1;

            _mockSlideWriter.DeleteSlideAsync(Arg.Is(slideId))
                .Returns(false);

            var handler = CreateHandler();

            var command = new DeleteSlideCommand(slideId);

            var actualResult = await handler.Handle(command, new CancellationToken());

            actualResult.Should().BeNull();
        }

        private DeleteSlideCommandHandler CreateHandler()
        {
            return new DeleteSlideCommandHandler(_mockSlideWriter, new NullLogger<DeleteSlideCommandHandler>());
        }
    }
}