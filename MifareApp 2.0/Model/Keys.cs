using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MifareApp_2._0.Model
{
    public class Keys
    {
        private static byte[] IV = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private readonly byte[] KeyA = new byte[6];
        private readonly byte[] KeyB = new byte[6];
        private byte[] MasterKey = new byte[Constants.MASTER_KEY_LENGTH];

        public Keys(byte[] cardId, byte sectorNumber)
        {
            readMasterKey();
            generateKeys(cardId, MasterKey, sectorNumber);
        }

        public byte[] getA()
        {
            return KeyA;
        }

        public byte[] getB()
        {
            return KeyB;
        }

        private void readMasterKey()
        {
            MasterKey = File.ReadAllBytes("MASTER_KEY.pem");

            for (int i = 0; i < Constants.MASTER_KEY_LENGTH; ++i)
            {
                MasterKey[i] = (byte)(MasterKey[i] - Constants.ASCII_OFFSET);
            }
        }

        public Keys generateKeys(byte[] cardId, byte[] masterKey, byte sectorNumber)
        {
            byte[] hash = doHash(cardId, masterKey, sectorNumber);
            hash = Conversions.toHexByteArrayFromDecimalArray(hash);

            Array.Copy(hash, KeyA, 6);
            Array.Copy(hash, 6, KeyB, 0, 6);

            return this;
        }

        private byte[] doHash(byte[] cardId, byte[] masterKey, byte sectorNumber)
        {
            AesManaged aes;
            byte[] encrypted;
            ICryptoTransform encryptor;
            byte[] sectorNumberArray = new byte[1];
            byte[] masterKeyWithServiceID = new byte[masterKey.Length];

            if (sectorNumber != 0x00)
            {
                sectorNumberArray[0] = sectorNumber;
                masterKey.CopyTo(masterKeyWithServiceID, 0);
                sectorNumberArray.CopyTo(masterKeyWithServiceID, (masterKey.Length - 1));
            }
            else
            {
                masterKeyWithServiceID = masterKey;
            }

            try
            {
                aes = new AesManaged();

                aes.Mode = CipherMode.ECB;
                aes.IV = IV;
                aes.Key = masterKeyWithServiceID;
                aes.Padding = PaddingMode.ANSIX923;

                encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(cardId.Concat(sectorNumberArray).ToArray());
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return encrypted;
        }

    }
}
