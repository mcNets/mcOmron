using System;
using System.Collections.Generic;

namespace mcOMRON
{
	#region [enum] - PLC memory areas - FINS commands

	/// <summary>
	/// enums PLC memory areas
	/// </summary>
	public enum MemoryArea : byte
	{
		CIO_Bit = 0x30,
		WR_Bit = 0x31,
		HR_Bit = 0x32,
		AR_Bit = 0x33,
		CIO_Bit_FS = 0x70,
		WR_Bit_FS = 0x71,
		HR_Bit_FS = 0x72,
		CIO = 0xB0,
		WR = 0xB1,
		HR = 0xB2,
		AR = 0xB3,
		CIO_FS = 0xF0,
		WR_FS = 0xF1,
		HR_FS = 0xF2,
		TIM = 0x09,
		CNT = 0x09,
		TIM_FS = 0x49,
		CNT_FS = 0x49,
		TIM_PV = 0x89,
		CNT_PV = 0x89,
		DM_Bit = 0x02,
		DM = 0x82,
		DME0 = 0x90,
		DME1 = 0x91,
		DME2 = 0x92,
		DME3 = 0x93,
		DME4 = 0x94,
		DME5 = 0x95,
		DME6 = 0x97,
		TK_Bit = 0x06,
		TK = 0x46
	};



	/// <summary>
	/// FINS commands
	/// </summary>
	enum FinsCommands
	{
		MemoryAreaRead,
		MemoryAreaWrite,
		ControllerDataRead,
	};

	class FinsErrorCodes
	{
		public static IDictionary<byte, string> ErrorCodes { get; } = new Dictionary<byte, string> {
			{ 0x0, "No Error" },
			{ 0x1, "Invalid Memory Address Parameter" },
			{ 0x2, "Invalid or Illegal Command Param" },
			{ 0x3, "Response SID did not match" },
			{ 0x4, "NSB did not respond to send req" },
			{ 0x5, "Timed Out - No Response" },
			{ 0x6, "Timed Out Waiting For Response" },
			{ 0x7, "Bad Received CRC" },
			{ 0x8, "Unmatched Message IDs" },
			{ 0x9, "Unmatched Command/Response" },
			{ 0xa, "Unknown Error" },
			{ 0xb, "Network Address Out Of Range" },
			{ 0xc, "Node Address Out Of Range" },
			{ 0xd, "Unit Address Out Of Range" },
			{ 0xe, "Invalid Address Parameter" },
			{ 0xf, "Timed Out waiting for echo" },
			{ 0x10, "Bad Received FCS" },
			{ 0x11, "Response from different Host Link Unit" },
			{ 0x12, "No Valid Response Code" },
			{ 0x13, "No FINS Response Packet" },
			{ 0x14, "Unknown Error Code" },
			{ 0x15, "Local Node not part of Network" },
			{ 0x16, "Token Timeout, Node # too High" },
			{ 0x17, "Number of Transmit Retries Exceeded" },
			{ 0x18, "Max # of Frames Exceeded" },
			{ 0x19, "Node # Setting Error" },
			{ 0x1a, "Node # Duplication Error" },
			{ 0x1b, "Dest Node not part of Network" },
			{ 0x1c, "No Node with Node # Specified" },
			{ 0x1d, "Third Node not part of Network" },
			{ 0x1e, "Busy Error, Dest Node Busy" },
			{ 0x1f, "Response Timeout, Noise or Watchdog" },
			{ 0x20, "Error in Comm Controller" },
			{ 0x21, "PLC Error in Dest Node" },
			{ 0x23, "Undefined Command Used" },
			{ 0x24, "Cannot Process Command" },
			{ 0x25, "Routing Error" },
			{ 0x26, "Command is too Long" },
			{ 0x27, "Command is too Short" },
			{ 0x28, "Specified Data Items != Actual" },
			{ 0x29, "Incorrect Command Format" },
			{ 0x2a, "Incorrect Header" },
			{ 0x2b, "Memory Area Code Error" },
			{ 0x2c, "Access Size Specified is Wrong" },
			{ 0x2d, "First Address is Inaccessible" },
			{ 0x2e, "Address Range Exceeded" },
			{ 0x2f, "Unknown Error Code" },
			{ 0x30, "Non-existent Program # Specified" },
			{ 0x31, "Unknown Error Code" },
			{ 0x32, "Unknown Error Code" },
			{ 0x33, "Data Size in Command is Wrong" },
			{ 0x34, "Unknown Error Code" },
			{ 0x35, "Response Block too Long" },
			{ 0x36, "Incorrect Parameter Code" },
			{ 0x37, "Program Area Protected" },
			{ 0x38, "Registered Table Error" },
			{ 0x39, "Area Read-Only or Write Protected" },
			{ 0x3b, "Mode is Wrong" },
			{ 0x3c, "Mode is Wrong (Running)" },
			{ 0x3d, "PLC is in Program Mode" },
			{ 0x3e, "PLC is in Debug Mode" },
			{ 0x3f, "PLC is in Monitor Mode" },
			{ 0x40, "PLC is in Run Mode" },
			{ 0x41, "Specified Node is not Control Node" },
			{ 0x42, "Specified Memory does not Exist" },
			{ 0x43, "No Clock Exists" },
			{ 0x44, "Data Link Table Error" },
			{ 0x45, "Unit Error" },
			{ 0x46, "Command Error" },
			{ 0x47, "Destination Address Setting Error" },
			{ 0x48, "No Routing Tables" },
			{ 0x49, "Routing Table Error" },
			{ 0x4a, "Too Many Relays" },
			{ 0x4b, "The Header is not FINS" },
			{ 0x4c, "The Data Length is too Long" },
			{ 0x4d, "The Command is not Supported" },
			{ 0x50, "Timed Out waiting for Port Semaphore" },
			{ 0x5e, "All Connections are in Use" },
			{ 0x5f, "The Specified Node is Already Connected" },
			{ 0x60, "Attempt to Access Protected Node from Unspecified IP" },
			{ 0x61, "The Client FINS Node Address is OOR" },
			{ 0x62, "Same FINS Node Address is being used by Client and Server" },
			{ 0x63, "No Node Addresses are Available to Allocate" },
		};
	}

	#endregion



	/// <summary>
	/// 
	/// Version:	1.0
	/// Author:		Joan Magnet
	/// Date:		02/2015
	/// 
	/// base interface for FINS command layer
	/// 
	/// </summary>
	public interface IFINSCommand
	{
		/// <summary>
		/// return the connection status of the transport layer
		/// </summary>
		bool Connected
		{
			get;
		}


		/// <summary>
		/// return last error detected
		/// </summary>
		string LastError
		{
			get;
		}


		/// <summary>
		/// current transport layer
		/// </summary>
		ITransport Transport
		{
			get;
		}


		/// <summary>
		/// response array of the last FINS command
		/// </summary>
		Byte[] Response
		{
			get;
		}


		/// <summary>
		/// opens a connection 
		/// </summary>
		/// <returns></returns>
		bool Connect();


		/// <summary>
		/// close connection
		/// </summary>
		void Close();
		

		// Implemented FINS commands
		//
		bool MemoryAreaRead(MemoryArea area, UInt16 address, Byte bit_position, UInt16 count);
		bool MemoryAreaWrite(MemoryArea area, UInt16 address, Byte bit_position, UInt16 count, ref Byte[] data);
		bool ConnectionDataRead(Byte area);


		/// <summary>
		/// return the last dialog between PC and PLC
		/// </summary>
		/// <param name="Caption"></param>
		/// <returns></returns>
		string LastDialog(string Caption);
	}
}
