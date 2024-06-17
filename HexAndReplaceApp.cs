using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
            if (args.Length != 0 && args[0] == "test")
            {
                DoTests();
                return -2;
            }

            if (args.Length < 3)
            {
                Console.WriteLine("Replace first instance of one hex sequence with another. Usage: <File Name> <Find Hex> <Replacement Hex>.");
                return -1;
            }
            byte[] find = ConvertHexStringToByteArray(Regex.Replace(args[1], "0x|[ ,]", string.Empty).Normalize().Trim());
            byte[] replace = ConvertHexStringToByteArray(Regex.Replace(args[2], "0x|[ ,]", string.Empty).Normalize().Trim());
            using BinaryReplacer replacer = new(File.Open(args[0], FileMode.Open));
            long pos = replacer.Replace(find, replace);

            if (pos >= 0)
            {
                Console.WriteLine($"Pattern found and replaced at position {pos}");
                return 0;
            }
            Console.WriteLine("Pattern not found");
            return -1;
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

        private static void DoTests()
        {
            Console.WriteLine("Running tests...");

            static void DoTest(int bufferSize)
            {
                MemoryStream ms = new();
                ms.Write([0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08]);
                ms.Seek(0, SeekOrigin.Begin);
                using BinaryReplacer replacer = new(ms, bufferSize);
                long pos = replacer.Replace([0x03, 0x04], [0x0A, 0x0B]);
                if (pos != 2)
                {
                    throw new ApplicationException("Test failed");
                }
                pos = replacer.Replace([0x03, 0x04], [0x0A, 0x0B]);
                if (pos != -1)
                {
                    throw new ApplicationException("Test failed");
                }
                pos = replacer.Replace([0x07, 0x08], [0x0C, 0x0D]);
                if (pos != 6)
                {
                    throw new ApplicationException("Test failed");
                }
                pos = replacer.Replace([0x07, 0x08], [0x0C, 0x0D]);
                if (pos != -1)
                {
                    throw new ApplicationException("Test failed");
                }

                var finalSequence = new byte[] { 0x01, 0x02, 0x0A, 0x0B, 0x05, 0x06, 0x0C, 0x0D };
                if (!ms.ToArray().SequenceEqual(finalSequence))
                {
                    throw new ApplicationException("Test failed");
                }
            }
            for (var i = 2; i <= 16; i++)
            {
                DoTest(i);
            }

            Console.WriteLine("All passed");
        }
    }
}