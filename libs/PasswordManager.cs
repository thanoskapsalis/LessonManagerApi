using System.Security.Cryptography;
using System.Security;
using System.Text;

namespace lessonApi.libs
{
  public class PasswordManager
  {
    // Μετατροπή του κωδικού σε  md5 μορφή
    public string EncryptPassword(string password)
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      md5.ComputeHash(Encoding.ASCII.GetBytes(password));
      byte[] result = md5.Hash;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte t in result)
      {
        stringBuilder.Append(t.ToString("x2"));
      }

      return stringBuilder.ToString();
    }
  }
}