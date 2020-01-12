using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MuloApi.Interfaces;

namespace MuloApi.Classes
{
    public class CheckDataUser : ICheckData
    {
        public bool CheckLogin(string login)
        {
            var RegEmail =
                @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            return Regex.IsMatch(login, RegEmail, RegexOptions.IgnoreCase);
        }

        public bool CheckPassword(string pass)
        {
            var RegPassword = @"^[a-zA-Z][a-zA-Z]{5}$";
            return Regex.IsMatch(pass, RegPassword, RegexOptions.IgnoreCase);
        }
    }
}
