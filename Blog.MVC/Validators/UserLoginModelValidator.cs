using BlogMVC.Models;
using FluentValidation;

namespace BlogMVC.Validators
{
    public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginModelValidator()
        {
            this.RuleFor(x=>x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz");
            this.RuleFor(x=>x.Password).NotEmpty().WithMessage("Şfire adı boş olamaz");
        }
    }
}
