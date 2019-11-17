## Hex and Replace

HexAndReplace allows finding a hex sequence in any file and replacing with another. The replacement hex must be identical in length to the find hex.

The file will be replaced as is, so make a backup first if you are not sure.

Usage:

```
dotnet HexAndReplace.dll <file> <find hex> <replace hex>

Example: dotnet HexAndReplace.dll mybinaryfile.bin 0xFFEEDDCC 0xAAEEDDCC
```

Enjoy!

-- Jeff