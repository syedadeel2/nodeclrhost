namespace BlazorApp
{
    using ElectronHostedBlazor.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            CreateHostBuilder(args).Build().Run();
        }

        public static IElectronHostBuilder CreateHostBuilder(string[] args) =>
            BlazorElectronHost.CreateDefaultBuilder()
            .UseBlazorStartup<Startup>();
    }
}