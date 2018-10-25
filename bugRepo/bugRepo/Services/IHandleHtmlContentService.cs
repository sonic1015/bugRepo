using System.Threading.Tasks;
using Acr.UserDialogs;

namespace bugRepo.Services
{
    public interface IHandleHtmlContentService
    {
        Task InitializeHtmlContent();
        string DirectoryPath { get; }
    }

    public abstract class AbstractHandleHtmlContentService : IHandleHtmlContentService
    {

        public async Task InitializeHtmlContent()
        {
            if (!DirectoryPopulated())
            {
                using (var loading = UserDialogs.Instance.Loading("Setting up for first time use"))
                {
                    loading.Show();
                    await LoadHtmlFromResource();
                    await Task.Delay(3000);
                }
            }
        }
        
        public abstract string DirectoryPath { get; }

        protected virtual bool DirectoryPopulated()
        {
            return System.IO.Directory.Exists(DirectoryPath);
        }

        protected abstract Task LoadHtmlFromResource();
    }
}