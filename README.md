# mcOmron
C# OMRON PLC TCP Interface

This class uses synchronous sockets to communicate with the PLC via TCP. Keep in mind that a communication issue could block the current thread. I think it is advisable to use a secondary thread to avoid this problem. As you can see, this class does not launch neither message box windows or console messages, most functions return bool values and you should use .LastError() function to check the result.

The current version implements 3 PLC messages:

[1,1] MEMORY AREA READ finsMemoryAreaRead()
[1,2] MEMORY AREA WRITE finsMemoryAreaWrite()
[5,1] CONTROLLER DATA READ finsConnectionDataRead()

And some methods to deal with DM area:

ReadDM()
ReadDMs()
WriteDM()
ClearDMs()

And two new methods to deal with CIO Bit area:

ReadCIOBit()
WriteCIOBit()

Take a look at tcpFINSCommand.cs and you'll notice that it's really easy to add new methods for another PLC message.

It is not currently implemented, but the class can be easily modified to use UDP instead of TCP.

Please, take a look at: https://www.codeproject.com/Tips/878194/OMRON-PLC-TCP-Interface.
