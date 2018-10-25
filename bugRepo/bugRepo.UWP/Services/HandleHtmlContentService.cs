using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using bugRepo.Services;
using bugRepo.UWP.Services;

[assembly: Xamarin.Forms.Dependency(typeof(HandleHtmlContentService))]
namespace bugRepo.UWP.Services
{
    public class HandleHtmlContentService : AbstractHandleHtmlContentService
    {
        public override string DirectoryPath => Path.Combine(ApplicationData.Current.LocalFolder.Path, "html");

        protected override bool DirectoryPopulated()
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "html");
            return System.IO.Directory.Exists(path);
        }

        protected override async Task LoadHtmlFromResource()
        {
            var contentFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Content");
            var destinationFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("html", CreationCollisionOption.ReplaceExisting);

            await CopyFolderAsync(contentFolder, destinationFolder);
        }

        public static async Task CopyFolderAsync(StorageFolder source, StorageFolder destinationFolder, string desiredName = null)
        {
            foreach (var file in await source.GetFilesAsync())
            {
                await file.CopyAsync(destinationFolder, file.Name, NameCollisionOption.ReplaceExisting);
            }
            foreach (var folder in await source.GetFoldersAsync())
            {
                await CopyFolderAsync(folder, await destinationFolder.CreateFolderAsync(folder.Name, CreationCollisionOption.ReplaceExisting));
            }
        }
    }
}