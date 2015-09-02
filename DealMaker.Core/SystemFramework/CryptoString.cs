using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace KK.DealMaker.Core.SystemFramework
{
    public sealed class CryptoString
    {
        /// <summary>
        /// Constructor - Pass key and encrypt/decrypt action
        /// </summary>
        /// <param name="Key">Key to hash</param>
        /// <param name="Action">Encrypt/Descrypt</param>
        public CryptoString(string Key, CryptoString.Method Action)
        {
            _keyValue = Key;
            _encyptAction = Action;
        }
        /// <summary>
        /// Empty Construction
        /// </summary>
        public CryptoString()
        {


        }

        /// <summary>
        /// 
        /// </summary>
        public enum Method
        {
            /// <summary> </summary>
            Encrypt,
            /// <summary> </summary>
            Decrypt
        }

        private string _textValue = string.Empty;
        private string _keyValue = string.Empty;
        private Method _encyptAction = Method.Encrypt;
        private string _resultValue = string.Empty;
        private byte[] _SymKey;
        private byte[] SymIV;

        private static byte[] savedKey = null;
        private static byte[] savedIV = null;

        public static byte[] sKey
        {
            get { return savedKey; }
            set { savedKey = value; }
        }

        public static byte[] sIV
        {
            get { return savedIV; }
            set { savedIV = value; }
        }

        private static void RdGenerateSecretKey(RijndaelManaged rdProvider)
        {
            if (savedKey == null)
            {
                rdProvider.KeySize = 256;
                rdProvider.GenerateKey();
                savedKey = rdProvider.Key;
            }
        }

        private static void RdGenerateSecretInitVector(RijndaelManaged rdProvider)
        {
            if (savedIV == null)
            {
                rdProvider.GenerateIV();
                savedIV = rdProvider.IV;
            }
        }

        public static string Encrypt(string originalStr)
        {
            // Encode data string to be stored in memory
            byte[] originalStrAsBytes = Encoding.ASCII.GetBytes(originalStr);
            byte[] originalBytes = {};

            // Create MemoryStream to contain output
            MemoryStream memStream = new MemoryStream(originalStrAsBytes.Length);

            RijndaelManaged rijndael = new RijndaelManaged();

            // Generate and save secret key and init vector
            RdGenerateSecretKey(rijndael);
            RdGenerateSecretInitVector(rijndael);

            if (savedKey == null || savedIV == null)
            {
                throw (new NullReferenceException(
                    "savedKey and savedIV must be non-null."));
            }

            // Create encryptor, and stream objects
            ICryptoTransform rdTransform = 
                rijndael.CreateEncryptor((byte[])savedKey.Clone(),
                                        (byte[])savedIV.Clone());
            CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform, 
                CryptoStreamMode.Write);

            // Write encrypted data to the MemoryStream
            cryptoStream.Write(originalStrAsBytes, 0, originalStrAsBytes.Length);
            cryptoStream.FlushFinalBlock();
            originalBytes = memStream.ToArray();

            // Release all resources
            memStream.Close();
            cryptoStream.Close();
            rdTransform.Dispose();
            rijndael.Clear();

            // Convert encrypted string
            string encryptedStr = Convert.ToBase64String(originalBytes);
            return (encryptedStr);
        }

        public static string Decrypt(string encryptedStr)
        {
            // Unconvert encrypted string
            byte[] encryptedStrAsBytes = Convert.FromBase64String(encryptedStr);
            byte[] initialText = new Byte[encryptedStrAsBytes.Length];

            RijndaelManaged rijndael = new RijndaelManaged();
            MemoryStream memStream = new MemoryStream(encryptedStrAsBytes);

            if (savedKey == null || savedIV == null)
            {
                throw (new NullReferenceException(
                    "savedKey and savedIV must be non-null."));
            }

            // Create decryptor, and stream objects
            ICryptoTransform rdTransform = 
                rijndael.CreateDecryptor((byte[])savedKey.Clone(), 
                                        (byte[])savedIV.Clone());
            CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform, 
                CryptoStreamMode.Read);

            // Read in decrypted string as a byte[]
            cryptoStream.Read(initialText, 0, initialText.Length);

            // Release all resources
            memStream.Close();
            cryptoStream.Close();
            rdTransform.Dispose();
            rijndael.Clear();

            // Convert byte[] to string
            string decryptedStr = Encoding.ASCII.GetString(initialText);
            return (decryptedStr);
        }

        /// <summary>
        /// Constructor - Pass key, and encrypt/decrypt action, and text to encrypt and decrypt.
        /// </summary>
        /// <param name="Key">Key to hash</param>
        /// <param name="Action">Encrypt/Decrypt</param>
        /// <param name="Text">Text to Encrypt/Decrypt</param>
        public CryptoString(string Key, CryptoString.Method Action, string Text)
        {
            _keyValue = Key;
            _textValue = Text;
            _encyptAction = Action;

        }
        /// <summary>
        /// Encrypts/Decrypts Text.
        /// <returns>Encrypted/Decypted string</returns>
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {

            if (_textValue == string.Empty) throw new ApplicationException("Text property not set");
            return this.DecryptEncrypt();

        }
        /// <summary>
        /// Generates encryption key based on phrase string.
        /// </summary>
        /// <param name="phrase">The phrase to create key from.</param>
        private void GenerateKey(string phrase)
        {
            if (_keyValue == string.Empty) throw new ApplicationException("Key not set");
            // change the pass phrase into a form we can use
            int i;
            int len;
            char[] cp = phrase.ToCharArray();
            len = cp.GetLength(0);
            // convert to byte array -  discard upper byte since is usually 0 in Eng.
            byte[] bp = new byte[len];
            for (i = 0; i < len; i++) bp[i] = (byte)cp[i]; // truncate char value

            // Crypto specific code starts here
            // allocate the key and IV arrays, assumes DES size
            _SymKey = new byte[8];
            SymIV = new byte[8];
            //do the hash operation
            SHA1CryptoServiceProvider oSha = new SHA1CryptoServiceProvider();
            oSha.ComputeHash(bp);
            byte[] result = oSha.Hash;

            // Crypto specific code ends here
            // use the low 64-bits for the key value
            for (i = 0; i < 8; i++) _SymKey[i] = result[i];
            for (i = 8; i < 16; i++) SymIV[i - 8] = result[i];

        }

        /// <summary>
        /// Encypts of Decrpts a string given the internal text, key, and action variables.
        /// </summary>
        /// <returns></returns>
        private string DecryptEncrypt()
        {

            GenerateKey(_keyValue);
            MemoryStream s = new MemoryStream();
            byte[] abytText;
            string result = string.Empty;
            //Convert string to byte array

            if (_encyptAction == CryptoString.Method.Decrypt)
            {
                abytText = Convert.FromBase64String(_textValue);
            }
            else
            {
                abytText = System.Text.Encoding.UTF8.GetBytes(_textValue);
            }

            //DES instance with random key
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //create DES Encryptor from this instance
            des.IV = SymIV;
            des.Key = _SymKey;
            ICryptoTransform desEncrypt;
            if (_encyptAction == CryptoString.Method.Decrypt)
            {
                desEncrypt = des.CreateDecryptor();
            }
            else
            {
                desEncrypt = des.CreateEncryptor();
            }

            //Create Crypto Stream that transforms file stream using des encryption
            CryptoStream cryptostream = new CryptoStream(s, desEncrypt, CryptoStreamMode.Write);

            //write out DES encrypted file
            cryptostream.Write(abytText, 0, abytText.Length);
            cryptostream.FlushFinalBlock();
            cryptostream.Close();

            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            //create file stream to read encrypted file back
            if (_encyptAction == CryptoString.Method.Decrypt)
            {
                result = encoding.GetString(s.ToArray());
            }
            else
            {
                result = Convert.ToBase64String(s.ToArray());
            }
            return result;

        }

        /// <summary>
        /// Text to encrypt or decrypt
        /// </summary>
        public string Text
        {
            get { return _textValue; }
            set { _textValue = value; }
        }
        /// <summary>
        /// Encryption key
        /// </summary>
        public string Key
        {
            get { return _keyValue; }
            set { _keyValue = value; }
        }
        /// <summary>
        /// Enum that determines whether the execute method encrypts or decrypts text.
        /// </summary>
        public CryptoString.Method Action
        {
            get { return _encyptAction; }
            set { _encyptAction = value; }
        }
    }
}
