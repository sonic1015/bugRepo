using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using bugRepo.Droid.Services;
using bugRepo.Services;

[assembly: Xamarin.Forms.Dependency(typeof(HandleHtmlContentService))]
namespace bugRepo.Droid.Services
{
    public class HandleHtmlContentService : AbstractHandleHtmlContentService
    {
        private static Context _context;

        public static void Init(Context context)
        {
            _context = context;
        }

        public override string DirectoryPath => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "html");

        protected override Task LoadHtmlFromResource()
        {
            var tcs = new TaskCompletionSource<object>();

            Task.Factory.StartNew(() => 
            {
                //delete old fodlers if the exist
                if (System.IO.Directory.Exists(DirectoryPath))
                {
                    System.IO.Directory.Delete(DirectoryPath, true);
                }

                //load all files in Assets to an HTML folder
                try
                {
                    SyncAssets("html", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/");
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

        private static void SyncAssets(string assetFolder, string targetDir)
        {
            if(_context == null) throw new NullReferenceException("Class has not been initialized");

            var slash = (assetFolder == "" ? "" : "/");

            string[] assets = _context.Assets.List(assetFolder);

            foreach (string asset in assets)
            {
                string[] subAssets = _context.Assets.List(assetFolder + slash + asset);

                // if it has a length, it's a folder
                if (subAssets.Length > 0)
                {
                    SyncAssets(assetFolder + slash + asset, targetDir);
                }
                else
                {
                    // it's a file
                    using (var source = _context.Assets.Open(assetFolder + slash + asset))
                    {
                        if (!System.IO.Directory.Exists(targetDir + assetFolder))
                        {
                            System.IO.Directory.CreateDirectory(targetDir + assetFolder);
                        }

                        using (var dest = System.IO.File.Create(targetDir + assetFolder + slash + asset))
                        {
                            Console.WriteLine("Copying '" + assetFolder + slash + asset + "' to '" + targetDir + assetFolder + slash + asset + "'");
                            source.CopyTo(dest);
                        }
                    }
                }

            }
        }
    }
}