using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace xcat
{
    /// <summary>
    /// 读取和保存密码管理文件
    /// </summary>
    public class PasswordRecord
    {
        //kouling,32位md5,实用你的密码md5加密后的
        //recordcount:2位,记录条数
        //struct:
        //name ? ,使用0x00结尾
        //link  ?
        //user  ?
        //password ?
        //remark    ?

        public struct PasswordInfo
        {
            public int Id;
            public string Name;
            public string Link;
            public string User;
            public string Password;
            public string Memo;
        }

        public string File { get; set; }

        public PasswordRecord(string file)
        {
            this.File = file;
        }

        public string Password { get; set; }

        private SortedDictionary<int, PasswordInfo> list = new SortedDictionary<int, PasswordInfo>();

        public SortedDictionary<int, PasswordInfo> Records
        {
            get { return list; }
        }

        private int index = 1;

        private string currentfile = string.Empty;

        public bool Read(string password)
        {
            this.list.Clear();
            index = 1;

            using (FileStream fs = new FileStream(this.File, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8,false))
                {

                    byte[] pwd = br.ReadBytes(16);

                    if (pwd.Length<16)
                    {
                        return false;
                    }

                    if (!CompareArrays(MD5(password, Encoding.UTF8), pwd))
                    {
                        return false;
                    }

                    this.Password = password;

                    string iv = GetIV(this.Password);
                    string key = GetKey(this.Password);

                    int count = br.ReadInt16();


                    for (int i = 0; i < count; i++)
                    {
                        int id = index++;
                        string name = DESDecrypst(br.ReadString(),key, iv);
                        string link = DESDecrypst(br.ReadString(),key, iv);
                        string user = DESDecrypst(br.ReadString(),key, iv);
                        string pass = DESDecrypst(br.ReadString(),key, iv);
                        string memo = DESDecrypst(br.ReadString(),key, iv);

                        this.list.Add(index, new PasswordInfo
                        {
                            Id = id,
                            Name = name,
                            Link = link,
                            User = user,
                            Password = pass,
                            Memo = memo
                        });
                    }

                }
            }

            return true;
        }

        private string GetKey(string password)
        {
            byte[] result = MD5(password, Encoding.UTF8);
            StringBuilder sb = new StringBuilder();

            for (var i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            return sb.ToString().Substring(8);
        }

        private string GetIV(string password)
        {
            byte[] result = MD5(password, Encoding.UTF8);
            StringBuilder sb = new StringBuilder();

            for (var i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            return sb.ToString().Substring(0, 8);
        }

        /// <summary>
        ///3DES加密
        /// </summary>
        /// <param name="text">加密数据</param>
        /// <param name="key">24位字符的密钥字符串</param>
        /// <param name="IV">8位字符的初始化向量字符串</param>
        /// <returns></returns>
        private static string DESEncrypt(string text, string key, string IV)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            try
            {
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                var DES = new TripleDESCryptoServiceProvider();
                DES.Key = encoding.GetBytes(key);
                DES.Mode = CipherMode.ECB;
                DES.IV = encoding.GetBytes(IV);
                DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                byte[] Buffer = encoding.GetBytes(text);
                return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception ex)
            {
                Log.Error("Encrypt3DES>>" + ex.Message);
                return string.Empty;
            }
        }
        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">24位字符的密钥字符串(需要和加密时相同)</param>
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
        /// <returns></returns>
        private static string DESDecrypst(string data, string key, string IV)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            try
            {
                Encoding encoding = Encoding.GetEncoding("UTF-8");

                var DES = new TripleDESCryptoServiceProvider();
                DES.Key = encoding.GetBytes(key);
                DES.Mode = CipherMode.ECB;
                DES.IV = encoding.GetBytes(IV);
                DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(data);
                return encoding.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                Log.Error("Decrypt3DES>>" + e.Message);
                return string.Empty;
            }
        }

        private bool CompareArrays(byte[] a, byte[] b)
        {
            if (a == null || b == null)
                return false;

            if (a.Length != b.Length)
                return false;

            int len = a.Length;

            for (int i = 0; i < len; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;

        }

        public void Add(PasswordInfo pi)
        {
            pi.Id = index++;
            this.list.Add(index, pi);
        }

        public void Update(PasswordInfo pi)
        {
            if (this.list.ContainsKey(pi.Id))
                this.list[pi.Id] = pi;
            else
                this.Add(pi);
        }

        public void Save()
        {
            string iv = GetIV(this.Password);
            string key = GetKey(this.Password);

            using (FileStream fs = new FileStream(this.File, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter wr = new BinaryWriter(fs, Encoding.UTF8))
                {
                    byte[] buff = MD5(this.Password, Encoding.UTF8);

                    wr.Write(buff);

                    wr.Write((short)this.list.Count);

                    foreach (var item in list)
                    {
                        PasswordInfo pi = item.Value;

                        wr.Write(DESEncrypt(pi.Name,key, iv));
                        wr.Write(DESEncrypt(pi.Link,key, iv));
                        wr.Write(DESEncrypt(pi.User,key, iv));
                        wr.Write(DESEncrypt(pi.Password,key, iv));
                        wr.Write(DESEncrypt(pi.Memo,key, iv));

                    }
                }

            }
        }


        public static byte[] MD5(string text, Encoding encoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(encoding.GetBytes(text + "3l4j53l323hdsjfl"));
            return result;
        }
    }
}
