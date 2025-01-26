using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace CryptoWalletValidator
{
    public class Validator
    {
        private const string Base58Chars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        public static string GuessAndValidateWalletType(string address)
        {
            if (IsValidAddress(address, "^(1|3|bc1)[a-zA-HJ-NP-Z0-9]{25,39}$") && VerifyChecksum(address, Base58Decode, CalculateDoubleSHA256Checksum))
                return "Bitcoin";
            if (IsValidAddress(address, "^0x[a-fA-F0-9]{40}$") && VerifyEthereumChecksum(address))
                return "Ethereum";
            if (IsValidAddress(address, "^r[a-zA-Z0-9]{24,34}$") && VerifyChecksum(address, Base58Decode, CalculateDoubleSHA256Checksum))
                return "Ripple";
            if (IsValidAddress(address, "^[48][0-9AB][1-9A-HJ-NP-Za-km-z]{93}$") && VerifyChecksum(address, MoneroBase58Decode, CalculateKeccakChecksum))
                return "Monero";
            if (IsValidAddress(address, "^X[1-9A-HJ-NP-Za-km-z]{33}$") && VerifyChecksum(address, Base58Decode, CalculateDoubleSHA256Checksum))
                return "Dash";
            if (IsValidAddress(address, "^t1[1-9A-HJ-NP-Za-km-z]{33}$") && VerifyChecksum(address, Base58Decode, CalculateDoubleSHA256Checksum))
                return "ZCash";

            return "Unknown or Invalid Address";
        }

        private static bool IsValidAddress(string address, string pattern)
        {
            return Regex.IsMatch(address, pattern);
        }

        private static bool VerifyChecksum(string address, Func<string, byte[]> decodeFunc, Func<byte[], byte[]> checksumFunc)
        {
            try
            {
                byte[] decoded = decodeFunc(address);
                byte[] payload = decoded.Take(decoded.Length - 4).ToArray();
                byte[] checksum = decoded.Skip(decoded.Length - 4).ToArray();
                byte[] calculatedChecksum = checksumFunc(payload);
                return checksum.SequenceEqual(calculatedChecksum);
            }
            catch
            {
                return false;
            }
        }

        private static byte[] Base58Decode(string input)
        {
            var output = new byte[25];
            foreach (char c in input)
            {
                int index = Base58Chars.IndexOf(c);
                if (index == -1)
                    throw new FormatException("Invalid Base58 character");

                for (int i = output.Length - 1; i >= 0; i--)
                {
                    int temp = output[i] * 58 + index;
                    output[i] = (byte)(temp % 256);
                    index = temp / 256;
                }

                if (index != 0)
                    throw new FormatException("Invalid Base58 encoding");
            }
            return output.SkipWhile(b => b == 0).ToArray();
        }

        private static byte[] MoneroBase58Decode(string input)
        {
            return Base58Decode(input);
        }

        private static byte[] CalculateDoubleSHA256Checksum(byte[] data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash1 = sha256.ComputeHash(data);
                byte[] hash2 = sha256.ComputeHash(hash1);
                return hash2.Take(4).ToArray();
            }
        }

        private static byte[] CalculateKeccakChecksum(byte[] data)
        {
                var sha3 = new Org.BouncyCastle.Crypto.Digests.KeccakDigest(256);
                byte[] output = new byte[sha3.GetDigestSize()];
                sha3.BlockUpdate(data, 0, data.Length);
                sha3.DoFinal(output, 0);
                return output;
        }

        private static bool VerifyEthereumChecksum(string address)
        {
            string addressWithoutPrefix = address.Substring(2);
            string addressHash = BitConverter.ToString(SHA3Hash(Encoding.UTF8.GetBytes(addressWithoutPrefix.ToLower()))).Replace("-", "").ToLower();

            for (int i = 0; i < addressWithoutPrefix.Length; i++)
            {
                char c = addressWithoutPrefix[i];
                if (char.IsLetter(c))
                {
                    if ((char.IsUpper(c) && addressHash[i] <= '7') || (char.IsLower(c) && addressHash[i] > '7'))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static byte[] SHA3Hash(byte[] input)
        {
            var sha3 = new Org.BouncyCastle.Crypto.Digests.KeccakDigest(256);
            byte[] output = new byte[sha3.GetDigestSize()];
            sha3.BlockUpdate(input, 0, input.Length);
            sha3.DoFinal(output, 0);
            return output;
        }

    }
}
