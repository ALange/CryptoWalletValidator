# Crypto Wallet Validator Library

`CryptoWalletValidator` is a lightweight C# library designed to validate and identify the type of cryptocurrency wallet address based on a given string. It currently supports popular cryptocurrencies like Bitcoin, Ethereum, Ripple, Monero, Dash, and ZCash.

## Features
- Identify wallet address types (e.g., Bitcoin, Ethereum, etc.).
- Validate wallet address format and checksum.
- Extensible to support additional cryptocurrencies.

## Getting Started

### Prerequisites
- .NET Framework or .NET Core SDK
- `Org.BouncyCastle` NuGet package (for Ethereum and Monero checksum calculations)

### Installation
1. Create a new project or open your existing one.
2. Add the library (`CryptoWalletValidator.dll`) as a reference to your project.
3. Install the required dependency using NuGet:
   ```bash
   dotnet add package BouncyCastle
   ```

### Usage

To use the `CryptoWalletValidator`, import the namespace and call the `GuessAndValidateWalletType` method with a wallet address string.

```csharp
using CryptoWalletValidator;

class Program
{
    static void Main(string[] args)
    {
        string walletAddress = "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa"; // Example Bitcoin address
        string walletType = Validator.GuessAndValidateWalletType(walletAddress);

        Console.WriteLine($"Wallet Type: {walletType}");
    }
}
```

### Example Output
For the input wallet address `1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa`:
```
Wallet Type: Bitcoin
```

## Supported Cryptocurrencies
1. **Bitcoin (BTC)**
   - Validates Base58 format and checksum.
2. **Ethereum (ETH)**
   - Validates hexadecimal format and EIP-55 checksum.
3. **Ripple (XRP)**
   - Validates Base58 format and checksum.
4. **Monero (XMR)**
   - Validates Base58 format and checksum using Keccak.
5. **Dash (DASH)**
   - Validates Base58 format and checksum.
6. **ZCash (ZEC)**
   - Validates Base58 format and checksum.

## Function Overview

### `GuessAndValidateWalletType(string address)`
Identifies and validates the type of wallet address.
- **Input**: A string representing the wallet address.
- **Output**: A string representing the type of wallet (e.g., "Bitcoin", "Ethereum") or "Unknown or Invalid Address".

### Internal Utility Functions
- `IsValidAddress`: Checks if the address matches a specific regex pattern.
- `VerifyChecksum`: Decodes the address and validates its checksum.
- `Base58Decode`: Decodes Base58-encoded strings.
- `CalculateDoubleSHA256Checksum`: Computes a double SHA-256 checksum.
- `CalculateKeccakChecksum`: Computes a Keccak-256 checksum (used by Monero).
- `VerifyEthereumChecksum`: Validates Ethereum EIP-55 checksum.

## Building the Library
1. Open the project in Visual Studio or another IDE.
2. Build the project as a Class Library.
3. The output `.dll` file will be located in the `bin` directory of your project.

## Contributing
Feel free to fork the repository and add support for additional cryptocurrencies or improve existing functionality. Submit a pull request with your changes.

## License
This library is released under the MIT License.

