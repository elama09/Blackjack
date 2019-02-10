using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace BlackJack_test1
{
    class Utils_IO
    {
        private const string initVector = "pemgail9uzpgzl88";
        private const string salasana = "MestariRomppu";
        private const string filename = "Applicationdata";

        public static int? HaluatkoLadataPelitilanteenRahat()
        {
            Console.WriteLine("Lataa tallennettu pelimerkkitilanne?\tY = Kyllä\tN = Ei");
            vääräkomento:
            string painettu = Console.ReadLine();
            if (painettu.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                int? rahatTiedostosta = TuoRahatTiedostosta();
                return rahatTiedostosta;
            }
            else if (painettu.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            else
            {
                Console.WriteLine("Tuntematon komento, yritä uudelleen!");
                goto vääräkomento;
            }
        }

        public static void VieRahatTiedostoon(Tili tili, int pe, int ja, int ta)
        {
            string encryptedRahat = EncryptString(tili.Rahat.ToString(), salasana);
            string encryptedVoitot = EncryptString($"{pe};{ja};{ta}", salasana);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var writer = File.CreateText(Path.Combine(path, filename));
            writer.WriteLine(encryptedRahat);
            writer.Write(encryptedVoitot);
            writer.Flush();
            Console.WriteLine("Tallennus valmis!");
        }

        public static int? TuoRahatTiedostosta()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            if (File.Exists(Path.Combine(path, filename)))
            {
                string[] cryptedKaikkiRivit = File.ReadAllLines(Path.Combine(path, filename));
                string decryptedRahat = DecryptString(cryptedKaikkiRivit[0], salasana);
                string decryptedVoitot = DecryptString(cryptedKaikkiRivit[1], salasana);
                string[] splitattuVoitot = decryptedVoitot.Split(';');
                PelinToiminnot.AktivoiLadatutVoitot(int.Parse(splitattuVoitot[0]), int.Parse(splitattuVoitot[1]), int.Parse(splitattuVoitot[2]));

                //string cryptedFileText = File.ReadAllText(Path.Combine(path, filename));
                //string decryptedText = DecryptString(cryptedFileText, salasana);
                Console.WriteLine($"Ladattu edellisestä tallennuksesta: {decryptedRahat} pelimerkkiä");
                Console.WriteLine($"Ladattu pelitilanne: Pelaajanvoitot {splitattuVoitot[0]}, Jakajanvoitot {splitattuVoitot[1]}, Tasapelit {splitattuVoitot[2]}");
                return int.Parse(decryptedRahat);
            }
            else
            {
                Console.WriteLine("Tallennusta ei löytynyt..");
                return null;
            }
        }


        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                TuhoaTiedosto();
                throw new Exception("OLET YRITTÄNYT HUIJATA VIHELIÄINEN!!! TALLENNUS TUHOTAAN!");
            }
        }

        public static void TuhoaTiedosto()
        {
            string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), filename);
            File.Delete(fullPath);
        }

    }
}
