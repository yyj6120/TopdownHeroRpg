using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace SJ.GameServer.Utilities
{
    // 출처: http://programmers.high-way.info/cs/aes.html
    // AES 128비트 암호화. 한글 지원

    // 사용예
    //string JSonData = "fdfdfd";
    //string EncryptString = AESEncrypt.Encrypt(JSonData);
    //string DeEncryptString = AESEncrypt.Decrypt(EncryptString);



    class AESEncrypt
    {
        // 128bit(16byte)의 IV（초기화 벡터）와 Key(암호키）
        static private string AesIV = @"!QAZ2WSX#EDC4RFV";
        static private string AesKey = @"5TGB&YHN7UJM(IK<";

        /// <summary>
        /// 문자열을 AES로 암호화 한다
        /// </summary>
        static public string Encrypt(string text)
        {
            // AES 암호화 서비스 프로바이더 
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.IV = Encoding.UTF8.GetBytes(AesIV);
            aes.Key = Encoding.UTF8.GetBytes(AesKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // 문자열을 바이트형 배열로 변환
            byte[] src = Encoding.Unicode.GetBytes(text);

            // 암호화 한다
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                // 바이트형 배열에서 Base64형식의 문자열로 변환
                return Convert.ToBase64String(dest);
            }
        }

        /// <summary>
        /// 문자열을 AES로 복호화 한다
        /// </summary>
        static public string Decrypt(string text)
        {
            // AES 암호화 서비스 프로바이더
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.IV = Encoding.UTF8.GetBytes(AesIV);
            aes.Key = Encoding.UTF8.GetBytes(AesKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Base64 형식의  문자열에서 바이트형 배열로 변환
            byte[] src = System.Convert.FromBase64String(text);

            // 복호화 한다
            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                return Encoding.Unicode.GetString(dest);
            }
        }
    }
}
