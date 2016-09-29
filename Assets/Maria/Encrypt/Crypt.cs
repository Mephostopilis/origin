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
        public static byte[] randomkey()
        {
            try
            {
                Crypt_CSharp.PACKAGE pg = Crypt_CSharp.randomkey();
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                Marshal.FreeHGlobal(pg.src);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] desencode(byte[] key, byte[] src)
        {
            Debug.Assert(key.Length > 0);
            Debug.Assert(src.Length > 0);
            try
            {
                IntPtr ptr1 = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, ptr1, key.Length);
                Crypt_CSharp.PACKAGE pgkey;
                pgkey.src = ptr1;
                pgkey.len = key.Length;

                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.desencode(pgkey, pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">必须8位</param>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public static byte[] desdecode(byte[] key, byte[] encrypted)
        {
            Debug.Assert(key.Length == 8);
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
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                Marshal.FreeHGlobal(ptr1);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] hashkey(byte[] src)
        {
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
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
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
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
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
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
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

                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                pg = Crypt_CSharp.dhsecret(pgkey, pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr1);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte[] dhexchange(byte[] src)
        {
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
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
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

                IntPtr ptr = Marshal.AllocHGlobal(key2.Length);
                Marshal.Copy(key2, 0, ptr, key2.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = key2.Length;
                pg = Crypt_CSharp.hmac64(pgkey, pg);
                byte[] buffer = new byte[pg.len];
                Marshal.Copy(pg.src, buffer, 0, pg.len);
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr1);
                Marshal.FreeHGlobal(ptr);
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
                Marshal.FreeHGlobal(pg.src);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte[] hmac_hash(byte[] key, byte[] data)
        {
            try
            {
                Debug.Assert(key.Length == 8);
                IntPtr keyp = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, keyp, key.Length);
                Crypt_CSharp.PACKAGE key_pg;
                key_pg.src = keyp;
                key_pg.len = key.Length;
                IntPtr datap = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, datap, data.Length);
                Crypt_CSharp.PACKAGE data_pg;
                data_pg.src = datap;
                data_pg.len = data.Length;
                Crypt_CSharp.PACKAGE pg;
                pg = Crypt_CSharp.hmac_hash(key_pg, data_pg);
                byte[] res = new byte[pg.len];
                Marshal.Copy(pg.src, res, 0, pg.len);
                Marshal.FreeHGlobal(key_pg.src);
                Marshal.FreeHGlobal(data_pg.src);
                Marshal.FreeHGlobal(pg.src);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}