## Hex and Replace

HexAndReplace allows finding a hex sequence in any file and replacing with another. The replacement hex must be identical in length to the find hex. Great if you don't want to install a hex-editor for concerns of malware.

The file will be replaced as is, so make a backup first if you are not sure.

Usage:

```
HexAndReplace <file> <find hex> <replace hex>

General Example:
HexAndReplace mybinaryfile.bin 0xFFEEDDCC 0xAAEEDDCC

// Unity 2019.3 dark mode
HexAndReplace "C:\Program Files\Unity\Hub\Editor\2019.3.6f1\Editor\Unity.exe" "75 15 33 C0 EB 13 90" "74 15 33 C0 EB 13 90"

// Unity 2018.4 dark mode
HexAndReplace "C:\Program Files\Unity\Hub\Editor\2018.4.6f1\Editor\Unity.exe" "74 04 33 C0 EB 02 8B 03 48 8B 4C" "75 04 33 C0 EB 02 8B 03 48 8B 4C"

// Unity 2017.4 dark mode
HexAndReplace "C:\Program Files\Unity\Hub\Editor\2017.4.38f1\Editor\Unity.exe" "75 08 33 C0 48 83 C4 20 5B C3 8B 03 48 83 C4 20 5B C3" "74 08 33 C0 48 83 C4 20 5B C3 8B 03 48 83 C4 20 5B C3"
```

Enjoy!

-- Jeff
