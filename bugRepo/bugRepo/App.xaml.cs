using System;
using System.Diagnostics;
using System.Threading.Tasks;
using bugRepo.Services;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace bugRepo
{
    public partial class App : Application
    {
        private readonly WebView _webView = new WebView();

        public App()
        {
            InitializeComponent();

            MainPage = new ContentPage
            {
                Content = _webView,
            };
        }

        protected override async void OnStart()
        {
            var htmlService = DependencyService.Get<IHandleHtmlContentService>();
            await htmlService.InitializeHtmlContent();

            var filePath = htmlService.DirectoryPath;
            var url = "http://localhost:8787/";

            var server = new WebServer(url);

            server.RegisterModule(new LocalSessionModule());
            server.RegisterModule(new StaticFilesModule(filePath));
            server.Module<StaticFilesModule>().UseRamCache = true;
            server.Module<StaticFilesModule>().DefaultExtension = ".html";
            server.Module<StaticFilesModule>().DefaultDocument = "index.html";
            server.Module<StaticFilesModule>().UseGzip = false;

#pragma warning disable 4014
            Task.Factory.StartNew(async () =>
#pragma warning restore 4014
            {
                Debug.WriteLine("Starting Server");
                await server.RunAsync();
            });

            _webView.Source = url;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
