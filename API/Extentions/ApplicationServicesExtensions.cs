using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
//using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extentions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = ActionContext => {
                    var errors = ActionContext.ModelState
                    .Where(e=>e.Value.Errors.Count > 0)
                    .SelectMany(x=>x.Value.Errors)
                    .Select(x=>x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse {Errors = errors}; 
                    return new BadRequestObjectResult(errorResponse);
                }; 
            });
            /*services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>(); */

            return services;
        }
    }
}