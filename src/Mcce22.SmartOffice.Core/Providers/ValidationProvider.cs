using FluentValidation;

namespace Mcce22.SmartOffice.Core.Providers
{
    public interface IValidationProvider
    {
        IValidator[] GetValidators();
    }

    public class ValidationProvider : IValidationProvider
    {
        private readonly IValidator[] _validators;

        public ValidationProvider(IValidator[] validators = null)
        {
            _validators = validators ?? Array.Empty<IValidator>();
        }

        public IValidator[] GetValidators()
        {
            return _validators;
        }
    }
}
