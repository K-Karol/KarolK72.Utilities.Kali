using KarolK72.Core;
using KarolK72.Data.Common;
using KarolK72.Utilities.Kali.Server.Library.Models;
using KarolK72.Utilities.Kali.Server.Library.Services;

namespace KarolK72.Utilities.Kali.BlazorApp.ViewModels
{
    public class ApplicationViewModel : BasePropertyChangedClass
    {
        private readonly IUnitOfWorkFactory<ISqlProvider> _unitOfWorkFactory;

        private Application _application;
        public Application Application { get => _application; set { SetProperty(ref _application, value); OnPropertyChanged(nameof(IsApplicationSelected)); } }
        public bool IsApplicationSelected => _application != null;

        public ApplicationViewModel(IUnitOfWorkFactory<ISqlProvider> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task DeleteCurrentApplication()
        {
            using(var uow = _unitOfWorkFactory.CreateNew())
            {
                await Task.Delay(3000);
                await uow.Work.DeleteApplicationAsync(_application);
                await uow.SaveAsync();
            }
            Application = null;
        }
    }
}
