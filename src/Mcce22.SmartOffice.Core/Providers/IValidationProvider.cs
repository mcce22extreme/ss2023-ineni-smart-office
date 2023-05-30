using FluentValidation;

namespace Mcce22.SmartOffice.Core.Providers
{
    public interface IValidationProvider
    {
        IValidator[] GetValidators();
    }
}
