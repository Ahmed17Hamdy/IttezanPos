using Microsoft.Extensions.DependencyInjection;

using Shiny;
namespace IttezanPos.Helpers
{
    public class ShinyAppStartup : Shiny.ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // for general client functionality
            services.UseBleCentral();

            // for client functionality in the background
         //   services.UseBleCentral<YourBleDelegate>();

            // for GATT server
            services.UseBlePeripherals();
        }
    }
}
