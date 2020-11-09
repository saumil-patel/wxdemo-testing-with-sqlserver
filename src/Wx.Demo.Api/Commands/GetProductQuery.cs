using MediatR;
using Wx.Demo.Api.Models;

namespace Wx.Demo.Api.Commands
{
    public class GetProductQuery : IRequest<Product>
    {
        public int ProductId { get; set; }
    }
}