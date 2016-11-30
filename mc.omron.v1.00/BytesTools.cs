using System;

namespace mcOMRON
{
	/// <summary>
	/// Conversion between Bytes[], UInt16, UInt32, Int16, Int32
	/// </summary>
	public static class BTool
	{
		/// <summary>
		/// convert UInt16 into Byte[]
		/// </summary>
		/// <param name="value"></param>
		public static Byte[] Uint16toBytes(UInt16 value)
		{
			Byte[] resp = new Byte[2];

			resp[1] = (Byte)(value & 0xFF);
			resp[0] = (Byte)((value >> 8) & 0xFF);

			return resp;
		}

	

		/// <summary>
		/// convert Int16 into Byte[]
		/// </summary>
		/// <param name="value"></param>
		public static Byte[] Int16toBytes(Int16 value)
		{
			Byte[] resp = new Byte[2];

			resp[1] = (Byte)(value & 0xFF);
			resp[0] = (Byte)((value >> 8) & 0xFF);

			return resp;
		}



		/// <summary>
		/// converts UInt32 into UInt16[]
		/// </summary>
		/// <param name="value"></param>
		public static UInt16[] Uint32toUInt16(UInt32 value)
		{
			UInt16[] resp = new UInt16[2];

			resp[1] = (UInt16)(value & 0xFFFF);
			resp[0] = (UInt16)((value >> 16) & 0xFFFF);

			return resp;
		}



		/// <summary>
		/// converts Int32 into Int16[]
		/// </summary>
		/// <param name="value"></param>
		public static Int16[] Int32toInt16(Int32 value)
		{
			Int16[] resp = new Int16[2];

			resp[1] = (Int16)(value & 0xFFFF);
			resp[0] = (Int16)((value >> 16) & 0xFFFF);

			return resp;
		}



		/// <summary>
		/// convert 2 Bytes into UInt16
		/// </summary>
		/// <param name="B1"></param>
		/// <param name="B2"></param>
		/// <returns></returns>
		public static UInt16 BytesToUInt16(Byte B1, Byte B2)
		{
			UInt16 value = B1;
			value <<= 8;
			value += Convert.ToUInt16(B2);
			return value;
		}



		/// <summary>
		/// convert 2 Bytes into Int16
		/// </summary>
		/// <param name="B1"></param>
		/// <param name="B2"></param>
		/// <returns></returns>
		public static Int16 BytesToInt16(Byte B1, Byte B2)
		{
			Int16 value = B1;
			value <<= 8;
			value += Convert.ToInt16(B2);
			return value;
		}



		/// <summary>
		/// check if specific bit is set
		/// </summary>
		/// <param name="value"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static bool IsBitSet(Byte value, int position)
		{
			if (position < 0 || position > 7)
				throw new ArgumentOutOfRangeException("position", "position must be in the range 0 - 7");

			return (value & (1 << position)) != 0;
		}



		/// <summary>
		/// set specific bit
		/// </summary>
		/// <param name="value"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static Byte SetBit(Byte value, int position)
		{
			if (position < 0 || position > 7)
				throw new ArgumentOutOfRangeException("position", "position must be in the range 0 - 7");

			return (Byte)(value | (1 << position));
		}



		/// <summary>
		/// unset specific bit
		/// </summary>
		/// <param name="value"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static Byte UnsetBit(Byte value, int position)
		{
			if (position < 0 || position > 7)
				throw new ArgumentOutOfRangeException("position", "position must be in the range 0 - 7");

			return (Byte)(value & ~(1 << position));
		}



		/// <summary>
		/// toggle specific bit
		/// </summary>
		/// <param name="value"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static Byte ToggleBit(Byte value, int position)
		{
			if (position < 0 || position > 7)
				throw new ArgumentOutOfRangeException("position", "position must be in the range 0 - 7");

			return (Byte)(value ^ (1 << position));
		}



		/// <summary>
		/// binary string representation
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToBinaryString(Byte value)
		{
			return Convert.ToString(value, 2).PadLeft(8, '0');
		}
	}


	#region [DDM] manage 2 DM's like one single 32 bits value


	/// <summary>
	/// DDM
	/// 
	/// class to manage 2 DM's like one single 32 bits value
	/// </summary>
	public class DDM
	{
		/// <summary>
		/// unsigned int 32 bits value
		/// </summary>
		private UInt32 _UValue;


		/// <summary>
		/// signed int 32 bits value
		/// </summary>
		private Int32 _SValue;



		/// <summary>
		/// return the converted unsigned 32 bits value
		/// </summary>
		public UInt32 Value
		{
			get { return _UValue; }
		}



		/// <summary>
		/// returns the converted signed 32 bits value
		/// </summary>
		public Int32 SValue
		{
			get { return _SValue; }
		}


		/// <summary>
		/// constructor
		/// </summary>
		public DDM()
		{
			this._UValue = 0;
			this._SValue = 0;
		}


		/// <summary>
		/// convert two UInt16 values into one UInt32 value
		/// </summary>
		/// <param name="word1"></param>
		/// <param name="word2"></param>
		public UInt32 Set(UInt16 word1, UInt16 word2)
		{
			this._UValue = (UInt32)(word1 + (word2 << 16));
			return this._UValue;
		}


		/// <summary>
		/// convert two Int16 values into one Int32 value
		/// </summary>
		/// <param name="word1"></param>
		/// <param name="word2"></param>
		public Int32 Set(Int16 word1, Int16 word2)
		{
			this._SValue = (Int32)(word1 + (word2 << 16));
			return this._SValue;
		}


		/// <summary>
		/// return a formatted string
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public string ToString(string format = "")
		{
			return this.Value.ToString(format);
		}
	}

	#endregion

	// this is just to check git changes
}
