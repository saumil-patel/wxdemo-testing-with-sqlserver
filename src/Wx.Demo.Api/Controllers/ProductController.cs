using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Wx.Demo.Api.Commands;
using Wx.Demo.Api.Models;

namespace Wx.Demo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMediator _mediator;
        public ProductController(ILogger<ProductController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Product))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var productQuery = new GetProductQuery { ProductId = productId };
            var result = await _mediator.Send(productQuery);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductCommand createProductCommand)
        {
            if (createProductCommand == null)
            {
                return BadRequest("Request is null.");
            }

            var createdId = await _mediator.Send(createProductCommand);

            return new CreatedResult(Url.Action("GetProduct"), createdId);
        }

        [HttpPatch]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductCommand updateProductCommand)
        {
            if (updateProductCommand == null)
            {
                return BadRequest("Request is null.");
            }

            var updateResult = await _mediator.Send(updateProductCommand);

            if (updateResult)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
