using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinFormat {
    /// <summary>
    /// Helper to convert a plain string to binary formatted string or vice versa
    /// </summary>
    public sealed class BinFormat {
        /// <summary>
        /// Converts a plain string to binary formatted via base64
        /// </summary>
        /// <param name="plain">plain string</param>
        /// <returns>binary formatted or plain string on error</returns>
        public string ToBin(string plain) {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            var bytes = Encoding.Unicode.GetBytes(plain);
            bf.Serialize(ms, bytes);
            return Convert.ToBase64String(ms.GetBuffer());
        }

        /// <summary>
        /// Converts a binary formatted string to plain via base 64
        /// </summary>
        /// <param name="bin">binary formatted string</param>
        /// <returns>plain string or binary formatted string on error</returns>
        public string ToPlain(string bin) {
            var binData = Convert.FromBase64String(bin);
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(binData);
            var bytes = (byte[])bf.Deserialize(ms);
            return Encoding.Unicode.GetString(bytes);
        }
    }
}