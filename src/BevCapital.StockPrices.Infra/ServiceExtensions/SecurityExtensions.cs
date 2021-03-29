using BevCapital.StockPrices.Application.Gateways.Security;
using BevCapital.StockPrices.Infra.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.StockPrices.Infra.ServiceExtensions
{
    public static class SecurityExtensions
    {
        public static IServiceCollection AddAppSecurity(this IServiceCollection services)
        {
            services.AddSingleton<ITokenSecret, TokenSecret>();

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder builder)
        {
            builder.UseXContentTypeOptions();
            builder.UseReferrerPolicy(opt => opt.NoReferrer());
            builder.UseXXssProtection(opt => opt.EnabledWithBlockMode());
            builder.UseXfo(opt => opt.Deny());

            return builder;
        }

    }
}
