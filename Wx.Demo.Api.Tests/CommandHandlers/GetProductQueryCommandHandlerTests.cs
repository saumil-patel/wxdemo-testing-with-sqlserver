using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Wx.Demo.Api.CommandHandlers;
using Wx.Demo.Api.Commands;
using Wx.Demo.Api.Models;
using Wx.Demo.Api.Services;
using Xunit;

namespace Wx.Demo.Api.Tests.CommandHandlers
{
    public class GetProductQueryCommandHandlerTests
    {
        private readonly GetProductQueryCommandHandler _sut;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        public GetProductQueryCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _sut = new GetProductQueryCommandHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProductDescriptionIsReturned()
        {
            const string testDescription = "test description";

            _productRepositoryMock.Setup(_ => _.GetProduct(5))
                .ReturnsAsync(() => new Product {Id = 5, Descrition = testDescription});

            var result = await _sut.Handle(new GetProductQuery {ProductId = 5}, CancellationToken.None);
            result.Should().NotBeNull();
            result.Descrition.Should().Be(testDescription);
        }
    }
}
