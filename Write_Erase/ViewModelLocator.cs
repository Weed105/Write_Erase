using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Erase.Models;
using Write_Erase.Services;
using Write_Erase.ViewModels;

namespace Write_Erase
{
    public class ViewModelLocator
    {
        public static ServiceProvider provider;

        public static void Init()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddTransient<mWindowViewModel>();
            services.AddTransient<mHomeViewModel>();
            services.AddTransient<mItemsViewModel>();
            services.AddTransient<mBasketViewModel>();
            services.AddTransient<mRegistrationViewModel>();
            services.AddTransient<mOrderViewModel>();

            services.AddSingleton<PageService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<OrderProductService>();

            services.AddDbContext<TradeContext>();
            provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                provider.GetRequiredService(service.ServiceType);
            }

        }
        public mWindowViewModel mWindowViewModel => provider.GetRequiredService<mWindowViewModel>();
        public mHomeViewModel mHomeViewModel => provider.GetRequiredService<mHomeViewModel>();
        public mItemsViewModel mItemsViewModel => provider.GetRequiredService<mItemsViewModel>();
        public mBasketViewModel mBasketViewModel => provider.GetRequiredService<mBasketViewModel>();
        public mRegistrationViewModel mRegistrationViewModel => provider.GetRequiredService<mRegistrationViewModel>();
        public mOrderViewModel mOrderViewModel => provider.GetRequiredService<mOrderViewModel>();
    }
}
