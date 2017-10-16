# CryptoExplorer
Crypto Explorer - A simple utility to graphically demonstrate the weaknesses in poor Cryptography implementations. Specifically, it demonstrates the weakness in Stream Cipher key stream reuse, and Block cipher ECB mode.

Update (16-Oct-2017): I have updated this utility with demos for Bitwise operators and Padding oracle attack. The core padding oracle attack logic has been taken as-is from [@martani's padding oracle implementation](https://github.com/martani/Padding-Oracle-Attack). Martani's implementation was based on a console app. I have migrated it to WPF and integrated it into my Crypto Explorer utility.
