using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Maria.Sharp;

namespace Maria.Encrypt {
    public class Crypt {
        public static byte[] randomkey() {
            try {
                IntPtr resptr = Crypt_CSharp.randomkey();
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] desencode(byte[] key, byte[] src) {
            UnityEngine.Debug.Assert(key.Length > 0);
            UnityEngine.Debug.Assert(src.Length > 0);
            try {
                IntPtr keyptr = Package.package_packarray(key);
                IntPtr srcptr = Package.package_packarray(src);
                IntPtr resptr = Crypt_CSharp.desencode(keyptr, srcptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(keyptr);
                Package.package_free(srcptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] desdecode(byte[] key, byte[] encrypted) {
            UnityEngine.Debug.Assert(key.Length == 8);
            try {
                IntPtr keyptr = Package.package_packarray(key);
                IntPtr encryptedptr = Package.package_packarray(encrypted);
                IntPtr resptr = Crypt_CSharp.desdecode(keyptr, encryptedptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(keyptr);
                Package.package_free(encryptedptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] hashkey(byte[] src) {
            try {
                IntPtr srcptr = Package.package_packarray(src);
                IntPtr resptr = Crypt_CSharp.hashkey(srcptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(srcptr);
                Package.package_free(resptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] hexencode(byte[] src) {
            try {
                IntPtr srcptr = Package.package_packarray(src);
                IntPtr resptr = Crypt_CSharp.hexencode(srcptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(srcptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception ex) {
                throw;
            }
        }

        public static byte[] hexdecode(byte[] encrypted) {
            try {
                IntPtr encryptedptr = Package.package_packarray(encrypted);
                IntPtr resptr = Crypt_CSharp.hexdecode(encryptedptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(encryptedptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] base64encode(byte[] src) {
            try {
                IntPtr srcptr = Package.package_packarray(src);
                IntPtr resptr = Crypt_CSharp.base64encode(srcptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(srcptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] base64decode(byte[] encrypted) {
            try {
                IntPtr encryptedptr = Package.package_packarray(encrypted);
                IntPtr resptr = Crypt_CSharp.base64decode(encryptedptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(encryptedptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] dhsecret(byte[] key, byte[] src) {
            try {
                IntPtr keyptr = Package.package_packarray(key);
                IntPtr srcptr = Package.package_packarray(src);
                IntPtr resptr = Crypt_CSharp.dhsecret(keyptr, srcptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(keyptr);
                Package.package_free(srcptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] dhexchange(byte[] src) {
            try {
                IntPtr srcptr = Package.package_packarray(src);
                IntPtr resptr = Crypt_CSharp.dhexchange(srcptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(srcptr);
                Package.package_free(resptr);

                return buffer;
            } catch (Exception ex) {
                throw;
            }
        }

        public static byte[] hmac64(byte[] key1, byte[] key2) {
            try {
                IntPtr key1ptr = Package.package_packarray(key1);
                IntPtr key2ptr = Package.package_packarray(key2);
                IntPtr resptr = Crypt_CSharp.hmac64(key1ptr, key2ptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_unpackarray(key1ptr);
                Package.package_unpackarray(key2ptr);
                Package.package_unpackarray(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }

        }

        public static byte[] sha1(byte[] src) {
            try {
                IntPtr srcptr = Package.package_packarray(src);
                IntPtr resptr = Crypt_CSharp.sha1(srcptr);
                byte[] buffer = Package.package_unpackarray(srcptr);
                Package.package_free(srcptr);
                Package.package_free(resptr);
                
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] hmac_sha1(byte[] src) {
            try {
                IntPtr srcptr = Package.package_packarray(src);
                IntPtr resptr = Crypt_CSharp.hmac_sha1(srcptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(srcptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }

        public static byte[] hmac_hash(byte[] key, byte[] data) {
            try {
                IntPtr keyptr = Package.package_packarray(key);
                IntPtr dataptr = Package.package_packarray(data);
                IntPtr resptr = Crypt_CSharp.hmac_hash(keyptr, dataptr);
                byte[] buffer = Package.package_unpackarray(resptr);
                Package.package_free(keyptr);
                Package.package_free(dataptr);
                Package.package_free(resptr);
                return buffer;
            } catch (Exception) {
                throw;
            }
        }
    }
}