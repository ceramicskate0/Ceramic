
              _____                               _       
             /  __ \                             (_)      
             | /  \/  ___  _ __  __ _  _ __ ___   _   ___ 
             | |     / _ \| '__|/ _` || '_ ` _ \ | | / __|
             | \__/\|  __/| |  | (_| || | | | | || || (__ 
             \____/  \___||_|   \__,_||_| |_| |_||_| \___|
              created by: Ceramicskate0

A simple dotNET 5.0 application I built to do common Red Teaming things for me. This way no dotnet core and will run on Linux and windows. Think of this like a multi tool om for educational red teaming.

# Like the work dont forget to hit that Star Button and Sponsor

# Contrib
- Open an Issue
- Do it yourself via a Pull Request

# Usage:
            CeramicSkate0's 1 stop shop in dotnet core to do random Red Team tasks via 1 app.  
            Cuz I cant remember all the random commandline args and tools to do it.      
            So yes things are case sensitive and yes the commandline inputs must be in order shown below.

            Commandline Params:
            
            -AVFileCheck {Input File Path}
            Run a modified version of DefenderCheck by matterpreter to try and trigger and AV response to test a file. Inspired by code from https://github.com/matterpreter/DefenderCheck          

            -DefenderCheck {Input File Path}
             matterpreter tool. Takes a binary as input and splits it until it pinpoints that exact byte that Microsoft Defender will flag on, and then prints those offending bytes to the screen. This can be helpful when trying to identify the specific bad pieces of code in your tool/payload. This code is from https://github.com/matterpreter/DefenderCheck
            
            -b64 {Input File Path}
            The command above will base64 encode a input file and save it to an output file Base64FileOutput.txt.

            -xor {Input .bin File Path} {XOR KEY}
            The command above will xor a byte file with a key and output it to XorShellcode.bin. This mean when you un-xor it you will need the same key.

            -aes {Input .bin File Path} {AES KEY} {AES IV}
             Read a byte file and will aes encrypt it with the provided Key and IV. Output file is EncryptedShellcode.bin

            -far {Input File or the file you want to search thru} {What you want to change} {What you want to change it to (File or string)(Will check to see if file exists if not assumes you wanted to use a string)}
            'FAR' (Find and Replace) will take a input file(1st arg) and then replace in that file the 2nd arg you specify with either the string your specify or the conents of a file you specify in the 3rd arg.

            -ChunkHTAShellcode {Input a already B64 encoded shellcode File Path} {Optional: Number of chunks}
            Attempts chunk and encode a shellcode input file and output it into a HTA ready to copy and paste output. Optional 2nd arg to tell it how many chunks. Default 100.
            
            -ChunckRAWtoVBArrys {Input File Path}
            Attempts chunk and encode a byte input file and output it into a VBA ready to copy and paste output. Optional 2nd arg to tell it how many chunks. Default 100.

            -Reverse {Input File Path}
            Reads the entire text file as 1 string and will write another file with the first files contents reversed. Output file will be same file name .reverse
    
            -AddJunk {Input File Path}
            Reads a text file and will randomly add a randomly generated junk string into the files contents and then output a new file with the junk in it. Output file will be same file name .junked

            -ByteReplaceBin {Input File Path/Name} {Ouput File Path/Name} {A single term, web site, or file 1 per line of things to randomly replace in the binary} {optional: when using a single term replace it with this term/string}
            Will read in a compiled (.Net) file and bin replace the bad strings with random list (1 per line) from a file or web page, or a single term (randomly generated string if no 4th arg is supplied. If a 4th is applied it will be used to replace the single bad string. (will check length for you).

            -GZIP {Input File Path} {Output File Path}(optional)
            Take a byte file read all the bytes in it and gzip the file and output a compressed version of it. Optional output file can be given. Default output file 'GZIPFileOutput.gz' in cwd

            -ConvertToHEX {Input File Path}
            Take a byte file and output a hex version of it.Ouputs the byte file to a file of string HEX output file is 'HexCodeOuput.txt'

            -ConvertToIntArray {Input File Path}
            Take a byte file and output a txt file with an array of INT's. Output file is ConvertedINTArray.txt

            -ConvertToINT64Array {Input File Path}
            Take a byte file and output a txt file with an array of INT64's. Output file is ConvertedINT64Array.txt

            -ChunkShellcodeTextToVariableCSharp (Input File Path}
            Take a text file (ie b64 shellcode) and obsure it in a way that the shellcode is now variable names chunked. It will also give you the code to reverse this. Output File is ChunkShellcodeTextToVariableCSharp.cs
            
            -ConvertToGUIDArray (Input File Path}
            Take in a byte file and output a text file that is a array of GUID's that represents the input shellcode. Output file is ConvertToGUIDArray.txt.

            
            
## Install dotnet on Ubuntu:

Most up to date Reference for installing dotnet and support for other Linux platforms is https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu

Below is what was used/tested:

``wget https://packages.microsoft.com/config/ubuntu/21.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb``

``sudo dpkg -i packages-microsoft-prod.deb``

``sudo apt-get update``

``sudo apt-get install -y apt-transport-https``

``sudo apt-get install -y dotnet-sdk-5.0``
  
``sudo apt-get install -y aspnetcore-runtime-5.0``

``sudo apt-get install -y dotnet-runtime-5.0``

``git clone --recurse-submodules https://github.com/ceramicskate0/Ceramic``

``cd Ceramic/Ceramic``

### Option 1 (Build it yourself)

``$ ~/Ceramic/Ceramic > dotnet run``

### Option 2 (Run Prebuilt)

``wget https://github.com/ceramicskate0/Ceramic/releases/download/1.0/Ceramic.exe``

``wget https://github.com/ceramicskate0/Ceramic/releases/download/1.0/Ceramic.dll``

``wget https://github.com/ceramicskate0/Ceramic/releases/download/1.0/Ceramic.runtimeconfig.json``

``wget https://github.com/ceramicskate0/Ceramic/releases/download/1.0/Ceramic.deps.json``

### Option 2.5 (Run Prebuilt)

``wget https://github.com/ceramicskate0/Ceramic/releases/download/1.0/Ceramic.zip``

``sudo apt install unzip -y``

``unzip Ceramic.zip``

### Run (from Ceramic Dir)

``dotnet run``

### OR

``dotnet Ceramic.(exe/dll)``
