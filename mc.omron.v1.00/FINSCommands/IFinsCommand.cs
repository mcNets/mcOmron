using System;

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
