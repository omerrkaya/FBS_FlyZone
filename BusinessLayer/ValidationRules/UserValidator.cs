using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{

    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.User_Name_Surname).NotEmpty().WithMessage("Ad Soyad boş geçilemez");
            RuleFor(x => x.User_Name_Surname).MinimumLength(3).WithMessage("Ad Soyad en az 3 karakter olmalıdır");
            RuleFor(x => x.User_Name_Surname).MaximumLength(50).WithMessage("Ad Soyad en fazla 50 karakter olmalıdır");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta boş geçilemez");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçerli bir E-Posta adresi giriniz");
            RuleFor(x => x.UserPassword).NotEmpty().WithMessage("Şifre boş geçilemez");
            RuleFor(x => x.UserPassword).MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");
            RuleFor(x => x.UserPassword).MaximumLength(16).WithMessage("Şifre en fazla 16 karakter olmalıdır");


        }
    }
}
