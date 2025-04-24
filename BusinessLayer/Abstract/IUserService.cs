using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IUserService
    {
        void RegisterUserAdd(User user);
        bool ChangePassword(string email, string currentPassword, string newPassword); // Şifre değiştirme işlemi için gerekli olan method. Bool tipinde döndürüyorum. Şifre doğru ise true, yanlış ise false döndürüyorum.
        User? GetUserByEmail(string email); // usermanager da kullanılacak.




    }
}
