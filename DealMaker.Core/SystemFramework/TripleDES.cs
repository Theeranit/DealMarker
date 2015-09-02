using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace KK.DealMaker.Core.SystemFramework
{
    public class TripleDES
    {
        private byte[] mbKey;
        private byte[] mbIV;
        private TripleDESCryptoServiceProvider tdProvider = new TripleDESCryptoServiceProvider();
        private UTF8Encoding UTEncode = new UTF8Encoding();

        // Key:  **YOUR KEY**
        // Project IV:  **YOUR IV**
        public TripleDES(string strKey, string strIV)
        {
            mbKey = UTEncode.GetBytes(strKey);
            mbIV = UTEncode.GetBytes(strIV);
        }

        public TripleDES()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public string EncryptToString(string strInput)
        {
            return Convert.ToBase64String(this.EncryptToBytes(strInput));
        }

        public byte[] EncryptToBytes(string strInput)
        {
            byte[] bInput = UTEncode.GetBytes(strInput);
            byte[] bOutput = ProcessInput(bInput, tdProvider.CreateEncryptor(mbKey, mbIV));
            return bOutput;
        }

        public string DecryptToString(string strInput)
        {
            return UTEncode.GetString(DecryptToBytes(strInput));
        }

        public byte[] DecryptToBytes(string strInput)
        {
            byte[] bInput = Convert.FromBase64String(strInput);
            byte[] bOutput = ProcessInput(bInput, tdProvider.CreateDecryptor(mbKey, mbIV));
            return bOutput;
        }

        private byte[] ProcessInput(byte[] input, ICryptoTransform ctProcessor)
        {
            MemoryStream memStream = new MemoryStream();
            CryptoStream crpStream = new CryptoStream(memStream, ctProcessor, CryptoStreamMode.Write);

            crpStream.Write(input, 0, input.Length);
            crpStream.FlushFinalBlock();

            memStream.Position = 0;

            byte[] output;
            output = new byte[memStream.Length];

            memStream.Read(output, 0, output.Length);

            memStream.Close();
            crpStream.Close();

            return output;
        }
    }
}
