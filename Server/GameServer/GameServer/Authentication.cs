using System;
using System.Text;
using SJ.GameServer.DataAccess.Entity;
using SJ.GameServer.DataAccess.Redis.Repository;

namespace SJ.GameServer.Service
{
    public class Authentication
    {
        public static int GenerateSecureNumber()
        {
            var SecureNumberRandom = new Random((int)DateTime.Now.Ticks);
            var number = SecureNumberRandom.Next(15271493, 598410861);
            return number;
        }

        public static string GenerateSecureNumber2(int startNumber, int endNumber, int count)
        {
            var SecureNumberRandom = new Random((int)DateTime.Now.Ticks);
            StringBuilder secureString = new StringBuilder();

            for (int i = 0; i < count; ++i)
            {
                secureString.Append(SecureNumberRandom.Next(startNumber, endNumber).ToString());
            }

            return secureString.ToString();
        }

        // 출처: http://stackoverflow.com/questions/54991/generating-random-passwords
        public static string GenerateSecureString(int lowercase, int uppercase, int numerics)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);
        }

        public static bool IsValidToken(ref Session session, string username, string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            SessionRepository sessionRepository = new SessionRepository();
            session = sessionRepository.Find(username);

            if (session == null && session.authToken != token)
                return false;

            return true;
        }
    }
}
