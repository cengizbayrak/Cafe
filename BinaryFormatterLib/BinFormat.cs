using System;
using System.Text;

namespace BinFormat {
    /// <summary>
    /// Helper to convert a plain string to binary formatted string or vice versa
    /// </summary>
    public class BinFormat {
        /// <summary>
        /// Converts a plain string to binary formatted via base64
        /// </summary>
        /// <param name="plain">plain string</param>
        /// <returns>binary formatted or plain string on error</returns>
        public string ToBin(string plain) {
            try {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var bytes = Encoding.Unicode.GetBytes(plain);
                bf.Serialize(ms, bytes);
                return Convert.ToBase64String(ms.GetBuffer());
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return plain;
        }

        /// <summary>
        /// Converts a binary formatted string to plain via base 64
        /// </summary>
        /// <param name="bin">binary formatted string</param>
        /// <returns>plain string or binary formatted string on error</returns>
        public string ToPlain(string bin) {
            try {
                var binData = Convert.FromBase64String(bin);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(binData);
                var bytes = (byte[])bf.Deserialize(ms);
                return Encoding.Unicode.GetString(bytes);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return bin;
        }
    }
}