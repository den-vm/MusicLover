using System.Text.RegularExpressions;
using MuloApi.Interfaces;

namespace MuloApi.Classes
{
    public class CheckDataUser : ICheckData
    {
        public bool CheckLogin(string login)
        {
            var RegEmail = @"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$";
            return Regex.IsMatch(login, RegEmail, RegexOptions.IgnoreCase);
        }

        public bool CheckPassword(string pass)
        {
            var RegPassword = @"^[a-zA-Z][a-zA-Z]{5}$";
            return Regex.IsMatch(pass, RegPassword, RegexOptions.IgnoreCase);
        }
    }
}