using UnityEngine;
using System.Collections;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Maria.Encrypt
{
    public class Crypt
    {
<<<<<<< HEAD
=======
        try
        {
            Crypt_CSharp.PACKAGE pg = Crypt_CSharp.randomkey();
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            return buffer;
        }
        catch (Exception ex)
        {
            return null;
        }
        
    }
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b

        public static byte[] randomkey()
        {
            try
            {
                Crypt_CSharp.PACKAGE pg = Crypt_CSharp.randomkey();
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }

<<<<<<< HEAD
=======
            IntPtr ptr = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, ptr, src.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = src.Length;
            pg = Crypt_CSharp.desencode(pgkey, pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr1);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
        }
        catch (Exception ex)
        {
            Debug.Log("**************");
            return null;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static byte[] desencode(byte[] key, byte[] src)
        {
            try
            {
                IntPtr ptr1 = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, ptr1, key.Length);
                Crypt_CSharp.PACKAGE pgkey;
                pgkey.src = ptr1;
                pgkey.len = key.Length;

<<<<<<< HEAD
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.desencode(pgkey, pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr1);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception ex)
            {
                Debug.Log("**************");
                return null;
            }
=======
            IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
            Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = encrypted.Length;
            pg = Crypt_CSharp.desdecode(pgkey, pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            //Marshal.FreeHGlobal(ptr1);
            return buffer;
        }
        catch (Exception ex)
        {
            return null;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">必须8位</param>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public static byte[] desdecode(byte[] key, byte[] encrypted)
        {
<<<<<<< HEAD
            try
            {
                IntPtr ptr1 = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, ptr1, key.Length);
                Crypt_CSharp.PACKAGE pgkey;
                pgkey.src = ptr1;
                pgkey.len = key.Length;

                IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
                Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = encrypted.Length;
                pg = Crypt_CSharp.desdecode(pgkey, pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                Marshal.FreeHGlobal(ptr1);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
=======
            IntPtr ptr = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, ptr, src.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = src.Length;
            pg = Crypt_CSharp.hashkey(pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static byte[] hexencode(byte[] src)
    {
        try
        {
            IntPtr ptr = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, ptr, src.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = src.Length;
            pg = Crypt_CSharp.hexencode(pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
        }
        catch (Exception ex)
        {
            return null;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static byte[] hashkey(byte[] src)
        {
<<<<<<< HEAD
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.hashkey(pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
=======
            IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
            Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = encrypted.Length;
            pg = Crypt_CSharp.hexdecode(pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
        }
        catch (Exception)
        {
            return null;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static byte[] hexencode(byte[] src)
        {
<<<<<<< HEAD
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.hexencode(pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
=======
            IntPtr ptr = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, ptr, src.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = src.Length;
            pg = Crypt_CSharp.base64encode(pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static byte[] hexdecode(byte[] encrypted)
        {
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
                Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = encrypted.Length;
                pg = Crypt_CSharp.hexdecode(pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte[] base64encode(byte[] src)
        {
<<<<<<< HEAD
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.base64encode(pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
=======
            IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
            Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = encrypted.Length;
            pg = Crypt_CSharp.base64decode(pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static byte[] base64decode(byte[] encrypted)
        {
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
                Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = encrypted.Length;
                pg = Crypt_CSharp.base64decode(pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte[] dhsecret(byte[] key, byte[] src)
        {
            try
            {
                IntPtr ptr1 = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, ptr1, key.Length);
                Crypt_CSharp.PACKAGE pgkey;
                pgkey.src = ptr1;
                pgkey.len = key.Length;

<<<<<<< HEAD
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.dhsecret(pgkey, pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr1);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
=======
            IntPtr ptr = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, ptr, src.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = src.Length;
            pg = Crypt_CSharp.dhsecret(pgkey, pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr1);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
        }
        catch (Exception)
        {
            return null;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static byte[] dhexchange(byte[] src)
        {
<<<<<<< HEAD
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.dhexchange(pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
=======
            IntPtr ptr = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, ptr, src.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = src.Length;
            pg = Crypt_CSharp.dhexchange(pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
        }
        catch (Exception ex)
        {
            return null;   
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static byte[] hmac64(byte[] key1, byte[] key2)
        {
            try
            {
                IntPtr ptr1 = Marshal.AllocHGlobal(key1.Length);
                Marshal.Copy(key1, 0, ptr1, key1.Length);
                Crypt_CSharp.PACKAGE pgkey;
                pgkey.src = ptr1;
                pgkey.len = key1.Length;

<<<<<<< HEAD
                IntPtr ptr = Marshal.AllocHGlobal(key2.Length);
                Marshal.Copy(key2, 0, ptr, key2.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = key2.Length;
                pg = Crypt_CSharp.hmac64(pgkey, pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr1);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }

=======
            IntPtr ptr = Marshal.AllocHGlobal(key2.Length);
            Marshal.Copy(key2, 0, ptr, key2.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = key2.Length;
            pg = Crypt_CSharp.hmac64(pgkey, pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr1);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
        }
        catch (Exception)
        {
            return null;
        }
        
    }

    public static byte[] sha1(byte[] src)
    {
        try
        {
            IntPtr ptr = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, ptr, src.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = src.Length;
            pg = Crypt_CSharp.sha1(pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static byte[] sha1(byte[] src)
        {
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.sha1(pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] hmac_sha1(byte[] src)
        {
<<<<<<< HEAD
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.hmac_sha1(pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                // 释放pg.src;
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
=======
            IntPtr ptr = Marshal.AllocHGlobal(src.Length);
            Marshal.Copy(src, 0, ptr, src.Length);
            Crypt_CSharp.PACKAGE pg;
            pg.src = ptr;
            pg.len = src.Length;
            pg = Crypt_CSharp.hmac_sha1(pg);
            byte[] buffer = new byte[pg.len];
            Marshal.Copy(pg.src, buffer, 0, pg.len);
            // 释放pg.src;
            //Marshal.FreeHGlobal(pg.src);
            //Marshal.FreeHGlobal(ptr);
            return buffer;
>>>>>>> 8e6fcdaba6713a90c987b0721b73e81304239d1b
        }

        public static string desencode(string src, string key, string iv)
        {
            try
            {
                byte[] btKey = Encoding.UTF8.GetBytes(key);
                byte[] btIV = Encoding.UTF8.GetBytes(iv);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(src);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                            cs.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                    catch (Exception e)
                    {
                        return src;
                    }
                }
            }
            catch (Exception)
            {
                return "DES加密错误";
            }
        }

        public static string desdecode(string encrypted, string key, string iv)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(key);
            byte[] btIV = Encoding.UTF8.GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] data = Convert.FromBase64String(encrypted);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch (Exception)
                {

                    return encrypted;
                }
            }
        }

        public static string desencodestr(string src, string key, string iv)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(src);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(iv);
                ICryptoTransform encryptor = des.CreateEncryptor();
                byte[] result = encryptor.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
            catch (Exception e)
            {
                return "转换错误";
            }
        }

        public static string desdecodestr(string encrypted, string key, string iv)
        {
            try
            {
                string[] input = encrypted.Split("-".ToCharArray());
                byte[] data = new byte[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    data[i] = Byte.Parse(input[i], NumberStyles.HexNumber);

                }
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(iv);
                ICryptoTransform desencrypt = des.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch (Exception)
            {
                return "解密出错！";
            }
        }

    }
}


