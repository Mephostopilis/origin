using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Maria.Encrypt {
    public class Crypt {
        public static byte[] randomkey() {
            try {
                Crypt_CSharp.PACKAGE res = Crypt_CSharp.randomkey();
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] desencode(byte[] key, byte[] src) {
            Debug.Assert(key.Length > 0);
            Debug.Assert(src.Length > 0);
            try {
                IntPtr keyptr = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, keyptr, key.Length);
                Crypt_CSharp.PACKAGE pgkey;
                pgkey.src = keyptr;
                pgkey.len = key.Length;

                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.desencode(pgkey, pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(keyptr);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] desdecode(byte[] key, byte[] encrypted) {
            Debug.Assert(key.Length == 8);
            try {
                IntPtr keyptr = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, keyptr, key.Length);
                Crypt_CSharp.PACKAGE pgkey;
                pgkey.src = keyptr;
                pgkey.len = key.Length;

                IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
                Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = encrypted.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.desdecode(pgkey, pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(keyptr);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] hashkey(byte[] src) {
            try {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;
                Crypt_CSharp.PACKAGE res = Crypt_CSharp.hashkey(pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] hexencode(byte[] src) {
            try {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.hexencode(pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception ex) {
                throw;
            }
        }

        public static byte[] hexdecode(byte[] encrypted) {
            try {
                IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
                Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = encrypted.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.hexdecode(pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] base64encode(byte[] src) {
            try {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.base64encode(pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] base64decode(byte[] encrypted) {
            try {
                IntPtr ptr = Marshal.AllocHGlobal(encrypted.Length);
                Marshal.Copy(encrypted, 0, ptr, encrypted.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = encrypted.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.base64decode(pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] dhsecret(byte[] key, byte[] src) {
            try {
                IntPtr keyptr = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, keyptr, key.Length);
                Crypt_CSharp.PACKAGE pgkey;
                pgkey.src = keyptr;
                pgkey.len = key.Length;

                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.dhsecret(pgkey, pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(keyptr);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] dhexchange(byte[] src) {
            try {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.dhexchange(pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception ex) {
                throw;
            }
        }

        public static byte[] hmac64(byte[] key1, byte[] key2) {
            try {
                IntPtr keyptr1 = Marshal.AllocHGlobal(key1.Length);
                Marshal.Copy(key1, 0, keyptr1, key1.Length);
                Crypt_CSharp.PACKAGE pgkey1;
                pgkey1.src = keyptr1;
                pgkey1.len = key1.Length;

                IntPtr keyptr2 = Marshal.AllocHGlobal(key2.Length);
                Marshal.Copy(key2, 0, keyptr2, key2.Length);
                Crypt_CSharp.PACKAGE pgkey2;
                pgkey2.src = keyptr2;
                pgkey2.len = key2.Length;
                Crypt_CSharp.PACKAGE res = Crypt_CSharp.hmac64(pgkey1, pgkey2);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(keyptr1);
                Marshal.FreeHGlobal(keyptr2);
                return buffer;
            } catch (Exception) {
                throw;
            }

        }

        public static byte[] sha1(byte[] src) {
            try {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.sha1(pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] hmac_sha1(byte[] src) {
            try {
                IntPtr ptr = Marshal.AllocHGlobal(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = src.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.hmac_sha1(pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] hmac_hash(byte[] key, byte[] data) {
            try {
                Debug.Assert(key.Length == 8);
                IntPtr keyptr = Marshal.AllocHGlobal(key.Length);
                Marshal.Copy(key, 0, keyptr, key.Length);
                Crypt_CSharp.PACKAGE keypg;
                keypg.src = keyptr;
                keypg.len = key.Length;

                IntPtr ptr = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, ptr, data.Length);
                Crypt_CSharp.PACKAGE pg;
                pg.src = ptr;
                pg.len = data.Length;

                Crypt_CSharp.PACKAGE res = Crypt_CSharp.hmac_hash(keypg, pg);
                byte[] buffer = new byte[res.len];
                Marshal.Copy(res.src, buffer, 0, res.len);
                pfree(res);

                Marshal.FreeHGlobal(keyptr);
                Marshal.FreeHGlobal(ptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static void pfree(Crypt_CSharp.PACKAGE data) {
            try {
                Crypt_CSharp.pfree(data);
            } catch (Exception) {
                throw;
            }
        }
    }
}