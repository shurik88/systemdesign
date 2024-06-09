using FluentValidation;

namespace SystemDesign.TinyUrlService.Validators
{
    public class TinyUrlDtoValidator : AbstractValidator<TinyUrlDto>
    {
        public TinyUrlDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Url)
                .NotEmpty()
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));
        }
    }
}
