using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wx.Demo.Api.Commands;
using Wx.Demo.Api.Data;
using Wx.Demo.Api.Models;

namespace Wx.Demo.Api.CommandHandlers
{
    public class GetProductQueryCommandHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetProduct(request.ProductId);
        }
    }
}