using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace TasksManagerClient.Helpers
{
    public enum Ugrencys
    {
        Expiried,
        Coming,
        NotRush
    }
    class Utilits
    {
        public static string GetHashString(string s)
        {
            string res = string.Empty;
            byte[] hash;
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(s);
                MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
                hash = CSP.ComputeHash(bytes);
            }
            catch (Exception) { return ""; }
            foreach (byte b in hash)
                res += string.Format("{0:x2}", b);
            return res;
        }
        public static bool PasswordIsValid(string s)
        {
            return !string.IsNullOrEmpty(s) && !Regex.IsMatch(s, @"[а-яА-Я]"); //Regex.IsMatch(s, @"[a-z]") && Regex.IsMatch(s, @"[A-Z]") && Regex.IsMatch(s, @"[0-9]") &&
        }
        public static Ugrencys DateTimeToUgrency(DateTime date)
        {
            int days = (date - DateTime.Now).Days;
            if (days < 0)
                return Ugrencys.Expiried;
            if (days == 1)
                return Ugrencys.Coming;
            return Ugrencys.NotRush;
        }
    }
}
