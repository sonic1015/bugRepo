using System;
using System.IO;
using System.Threading.Tasks;
using bugRepo.iOS.Services;
using bugRepo.Services;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(HandleHtmlContentService))]
namespace bugRepo.iOS.Services
{
    public class HandleHtmlContentService : AbstractHandleHtmlContentService
    {
        public override string DirectoryPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Resources), "html");

        protected override Task LoadHtmlFromResource()
        {
            var tcs = new TaskCompletionSource<object>();
            Task.Factory.StartNew(() =>
            {
                var source = new DirectoryInfo(Path.Combine(NSBundle.MainBundle.BundlePath, "html"));
                var destination = System.IO.Directory.CreateDirectory(DirectoryPath);

                try
                {
                    CopyFolderAndContents(source, destination);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                    return;
                }

                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        public static void CopyFolderAndContents(DirectoryInfo source, DirectoryInfo destination, string desiredName = null)
        {
            foreach (var file in source.EnumerateFiles())
            {
                System.IO.File.Copy(file.FullName, Path.Combine(destination.FullName, file.Name), true);
            }
            foreach (var folder in source.EnumerateDirectories())
            {
                CopyFolderAndContents(folder, System.IO.Directory.CreateDirectory(Path.Combine(destination.FullName, folder.Name)));
            }
        }
    }
}