using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DataService.Shared.Helpers
{
    /// <summary>
    /// 加密/解密
    /// </summary>
    public class AESCryptoHelper
    {
        private static string key = "LPSYS-2022-WEBERP";
        //默认密钥向量
        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AESEncrypt(string input)
        {
            return AESEncrypt(input, key);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static string AESEncrypt(string encryptString, string encryptKey)
        {
            encryptKey = GetSubString(encryptKey, 0, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = Keys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AESDecrypt(string input)
        {
            return AESDecrypt(input, key);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="decryptKey"></param>
        /// <returns></returns>
        public static string AESDecrypt(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = GetSubString(decryptKey, 0, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// GetSubString
        /// </summary>
        /// <param name="p_SrcString"></param>
        /// <param name="p_StartIndex"></param>
        /// <param name="p_Length"></param>
        /// <param name="p_TailString"></param>
        /// <returns></returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AESEncrypt(string toEncrypt, string key, string iv)
        {

            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            byte[] keyArray = Encoding.UTF8.GetBytes(key);//注意编码格式(utf8编码 UTF8Encoding)
            byte[] ivArray = Encoding.UTF8.GetBytes(iv);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            //rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();//加密
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AESDecrypt(string toDecrypt, string key, string iv)
        {
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            byte[] keyArray = Encoding.UTF8.GetBytes(key);//注意编码格式(utf8编码 UTF8Encoding)
            byte[] ivArray = Encoding.UTF8.GetBytes(iv);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();//解密
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    public class MD5Helper
    {

        /// <summary>
        /// HmacMD5加密
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HmacMD5(string source, string key)
        {
            HMACMD5 hmacmd = new HMACMD5(Encoding.Default.GetBytes(key));
            byte[] inArray = hmacmd.ComputeHash(Encoding.Default.GetBytes(source));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < inArray.Length; i++)
            {
                sb.Append(inArray[i].ToString("X2"));
            }

            hmacmd.Clear();

            return sb.ToString();
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            //创建MD5对像  MD5是抽象的方法 不能实例化
            MD5 md5 = MD5.Create();//这里要引用using System.Security.Cryptography;
            //需要将字符串转成字节数组
            byte[] buffer = Encoding.Default.GetBytes(str);
            //字符转成的字节用MD5加密
            byte[] MD5buffer = md5.ComputeHash(buffer);
            string strNew = "";
            for (int i = 0; i < MD5buffer.Length; i++)
            {
                strNew += MD5buffer[i].ToString("x2");//x2是输出字符的格式  16进制  
                //如果只输x也是16进制但有的0b会省掉0 输的是b
            }
            return strNew;
        }
    }
}
