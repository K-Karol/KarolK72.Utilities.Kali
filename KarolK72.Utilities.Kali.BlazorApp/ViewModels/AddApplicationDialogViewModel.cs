using KarolK72.Core;
using KarolK72.Data.Common;
using KarolK72.Utilities.Kali.Server.Library.Models;
using KarolK72.Utilities.Kali.Server.Library.Services;
using System.ComponentModel.DataAnnotations;

namespace KarolK72.Utilities.Kali.BlazorApp.ViewModels
{
    public class AddApplicationDialogViewModel : BasePropertyChangedClass
    {
        private readonly IUnitOfWorkFactory<ISqlProvider> _unitOfWorkFactory;

        private bool _showDialog = false;
        public bool ShowDialog { get => _showDialog; set => SetProperty(ref _showDialog, value); }

        private ApplicationModel _application;
        public ApplicationModel Application { get => _application; set => SetProperty(ref _application, value); }

        public AddApplicationDialogViewModel(IUnitOfWorkFactory<ISqlProvider> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task CreateApplication()
        {
            using(var uow = _unitOfWorkFactory.CreateNew())
            {
                await uow.Work.InsertApplicationAsync(new Application() { ApplicationName = _application.ApplicationName, ApplicationGUID = Guid.Parse(_application.ApplicationGUID) });
                await uow.SaveAsync();
            }
        }
    }

    public class ApplicationModel
    {
        [Required]
        public string ApplicationName { get; set; }
        [Required, ValidateGUID]
        public string ApplicationGUID { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ValidateGUID : ValidationAttribute {
        public ValidateGUID()
                : base("Cannot parse as a GUID")
        {
        }

        public override bool IsValid(object value)
        {
            if (!Guid.TryParse(value?.ToString() ?? String.Empty, out Guid res))
                return false;

            bool p = !Equals(res, Guid.Empty);

            return p;
        }
    }

}
