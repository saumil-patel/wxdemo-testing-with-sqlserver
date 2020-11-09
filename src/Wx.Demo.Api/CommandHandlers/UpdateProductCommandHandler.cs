using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wx.Demo.Api.Commands;
using Wx.Demo.Api.Data;
using Wx.Demo.Api.Models;
using Wx.Demo.Api.Services;

namespace Wx.Demo.Api.CommandHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Descrition = request.Description,
                Id = request.Id
            };

            return await _productRepository.Update(product);
        }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return await _productRepository.Create(request.Name, request.Description);
        }
    }
}
