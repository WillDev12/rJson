using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Data;
using System;
using System.Xml.Schema;

namespace rJson
{
    public class rJs
    {
        public string getVar(DataTable parsedData, string variableName)
        {
            string i = "";
            foreach (DataRow row in parsedData.Rows)
            {
                if (row["Variable"].ToString() == variableName)
                {
                    i = row["Value"].ToString();
                }
            }
            return i;
        }
        public static DataTable parse(string? rJson, string? filePath)
        {
            string fileOutput = "";
            if (filePath != null)
            {
                fileOutput = File.ReadAllText(filePath);
            } 
            else if (rJson != null)
            {
                fileOutput = rJson;
            }
            else
            {
                throw new ArgumentException("Both params cannot be null: Reading rJson, filePath");
            }

            fileOutput = Decrypt(fileOutput);

            int ix = fileOutput.IndexOf("\t");
            fileOutput = fileOutput.Substring(ix + 1);
            fileOutput = fileOutput.Replace("\t", "").Replace(")", "").Replace("\n", "").Replace(" ", "");
            
            char[] separator = { ',' };
            string[] lines = fileOutput.Split(separator);

            DataTable dt = new DataTable();

            dt.Columns.Add("Variable", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            var name = "";
            var value = "";

            foreach (var line in lines)
            {
                name = line.Substring(0, line.IndexOf("="));
                value = line.Substring(line.LastIndexOf("=")).Replace("=", "");
                dt.Rows.Add(name, value);
            }

            return dt;
        }
        public string New(string yourCode, string? filePath)
        {
            yourCode = yourCode.Replace("=", " = ");
            yourCode = yourCode.Replace("(", "(\n\t");
            yourCode = yourCode.Replace(")", "\n\t)");
            yourCode = yourCode.Replace(",", ",\n\t");
            yourCode = yourCode.Insert(0, "(\n\t");
            yourCode = yourCode.Insert(yourCode.Length, "\n)");

            yourCode = Encrypt(yourCode);
            if (filePath != null)
            {
                File.WriteAllText(filePath, yourCode);
            }
            return yourCode;
        }

        private static Random random = new Random();
        public static string idKey
        {
            get
            {
                if (!File.Exists("rJs.json"))
                {
                    using (StreamWriter writer = new StreamWriter("rJs.json"))
                    {
                        writer.WriteLine("{");
                        writer.WriteLine("\tid: \"" + RandomString() + "\"");
                        writer.WriteLine("}");
                    }
                }
                string idJson = File.ReadAllText("rJs.json");
                keyGetter o = JsonConvert.DeserializeObject<keyGetter>(idJson);
                return o.id;
            }
        }

        private class keyGetter
        {
            public string id { get; set;}
        }

        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.!@#$%^&*";
            return new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string Encrypt(string clearText)
        {
            var rand = new Random();
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                byte[] IV = new byte[15];
                rand.NextBytes(IV);
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(idKey, IV);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(IV) + Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private static string Decrypt(string cipherText)
        {
            byte[] IV = Convert.FromBase64String(cipherText.Substring(0, 20));
            cipherText = cipherText.Substring(20).Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(idKey, IV);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}