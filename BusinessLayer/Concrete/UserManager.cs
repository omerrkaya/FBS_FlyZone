using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using DataAccessLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public void RegisterUserAdd(User user)
        {
            _userDal.Insert(user);
        }
        
        public bool ChangePassword(string email, string currentPassword, string newPassword) // Şifre değiştirme işlemi için gerekli olan method. Bool tipinde döndürüyorum. Şifre doğru ise true, yanlış ise false döndürüyorum.
        {
            using var context = new Context();
            var user = context.Users.FirstOrDefault(x => x.Email == email && x.UserPassword == currentPassword); // Kullanıcının emaili ve şifresi doğru mu kontrol ediyorum.
            
            if (user == null)
                return false; // Kullanıcı bulunamadıysa(null) false döndürüyorum. 
                
                //Tüm hepsinde nasıl düşündüğümü yazdım, doğruysa kafana yattıysa bir test et, yorum satırlarını silersin. 
                
            user.UserPassword = newPassword; // Kullanıcının şifresini yeni şifre ile güncelliyorum.
            context.Update(user);
            context.SaveChanges();
            return true; // Kullanıcının şifresi güncellendiğinde true döndürüyorum.
        }
        
        public User? GetUserByEmail(string email) // Kullanıcının emaili doğru mu kontrol ediyorum.
        {
            using var context = new Context();
            return context.Users.FirstOrDefault(x => x.Email == email); // Kullanıcının emaili doğru ise kullanıcıyı döndürüyorum.
        }
    }
    
}
