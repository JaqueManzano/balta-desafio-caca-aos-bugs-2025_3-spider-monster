using BugStore.Models;
using BugStore.Requests.Products;
using BugStore.Responses.Products;
using BugStore.Services.Interfaces;
using BugStore.Test.Repositories;

namespace BugStore.Test.Services
{
    public class FakeProductsService : IProductsService
    {
        private readonly FakeProductsRepository _repo;

        public FakeProductsService(FakeProductsRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetByIdProductsResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _repo.GetByIdAsync(id, cancellationToken);
            return new GetByIdProductsResponse
            {
                Product = product,
                Message = product == null ? "Product not found." : "Product found."
            };
        }

        public async Task<GetProductsResponse> GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await _repo.GetAllAsync(cancellationToken);
            return new GetProductsResponse
            {
                Products = products,
                Message = "Products retrieved."
            };
        }

        public async Task<CreateProductsResponse> CreateAsync(Product product, CancellationToken cancellationToken)
        {
            await _repo.AddAsync(product, cancellationToken);
            return new CreateProductsResponse
            {
                Product = product,
                Success = true,
                Message = "Product created."
            };
        }

        public async Task<UpdateProductsResponse> UpdateProductAsync(UpdateProductsRequest request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                return new UpdateProductsResponse
                {
                    Product = null,
                    Success = false,
                    Message = "Product not found."
                };
            }

            if (!string.IsNullOrEmpty(request.Slug)) product.Slug = request.Slug;
            if (request.Price.HasValue) product.Price = request.Price.Value;

            await _repo.UpdateAsync(product, cancellationToken);

            return new UpdateProductsResponse
            {
                Product = product,
                Success = true,
                Message = "Product updated."
            };
        }

        public async Task<DeleteProductsResponse> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _repo.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                return new DeleteProductsResponse
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            await _repo.DeleteAsync(product, cancellationToken);
            return new DeleteProductsResponse
            {
                Success = true,
                Message = "Product deleted."
            };
        }

        public async Task AddProductAsync(Product product)
        {
            await _repo.AddAsync(product, CancellationToken.None);
        }
    }
}
