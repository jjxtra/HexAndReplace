using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HexAndReplace
{
    /// <summary>
    /// App
    /// </summary>
    public static class HexAndReplaceApp
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">Args</param>
        public static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Replace first instance of one hex sequence with another. Usage: <File Name> <Find Hex> <Replacement Hex>.");
                return -1;
            }
            byte[] find = ConvertHexStringToByteArray(Regex.Replace(args[1], "0x|[ ,]", string.Empty).Normalize().Trim());
            byte[] replace = ConvertHexStringToByteArray(Regex.Replace(args[2], "0x|[ ,]", string.Empty).Normalize().Trim());
            if (find.Length != replace.Length)
            {
                throw new ArgumentException("Find and replace hex must be same length");
            }
            byte[] bytes = File.ReadAllBytes(args[0]);
            foreach (int index in PatternAt(bytes, find))
            {
                for (int i = index, replaceIndex = 0; i < bytes.Length && replaceIndex < replace.Length; i++, replaceIndex++)
                {
                    bytes[i] = replace[replaceIndex];
                }
                File.WriteAllBytes(args[0], bytes);
                Console.WriteLine("Pattern found at offset {0} and replaced.", index);
                return 0;
            }
            Console.WriteLine("Pattern not found");
            return -1;
        }

        private static IEnumerable<int> PatternAt(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }

        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
    }
}
