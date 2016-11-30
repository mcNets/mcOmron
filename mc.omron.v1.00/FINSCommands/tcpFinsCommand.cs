using System;
using System.Net;
using System.Text;

namespace mcOMRON
{
	/// <summary>
	/// 
	/// Version:	1.0
	/// Author:		Joan Magnet
	/// Date:		02/2015
	/// 
	/// FINS command object
	/// 
	/// Implemented FINS commands:
	/// 
	/// - MEMORY AREA READ
	/// - MEMORY AREA WRITE
	/// - CONNECTION DATA READ
	/// 
	/// </summary>
	public class tcpFINSCommand : IFINSCommand
	{
		#region **** properties

		/// <summary>
		/// return the connection status
		/// </summary>
		public bool Connected
		{
			get { return this._transport.Connected; }
		}


		/// <summary>
		/// return last error detected
		/// </summary>
		public string LastError
		{
			get { return this._lastError; }
		}


		/// <summary>
		/// transport layer
		/// </summary>
		public mcOMRON.ITransport Transport
		{
			get { return this._transport; }
		}


		/// <summary>
		/// return the last FINS response
		/// </summary>
		public Byte [] Response
		{
			get { return this.respFinsData; }
		}



		#region **** FINS Command's fields

		/// <summary>
		/// ICF Information control field
		/// </summary>
		public Byte ICF
		{
			get { return this.cmdFins[0]; }
			set { this.cmdFins[0] = value; }
		}

		/// <summary>
		/// RSC Reserved 
		/// </summary>
		public Byte RSC
		{
			get { return this.cmdFins[1]; }
			set { this.cmdFins[1] = value; }
		}


		/// <summary>
		/// GTC Gateway count
		/// </summary>
		public Byte GTC
		{
			get { return this.cmdFins[2]; }
			set { this.cmdFins[2] = value; }
		}


		/// <summary>
		/// DNA Destination network address (0=local network)
		/// </summary>
		public Byte DNA
		{
			get { return this.cmdFins[3]; }
			set { this.cmdFins[3] = value; }
		}


		/// <summary>
		/// DA1 Destination node number
		/// </summary>
		public Byte DA1
		{
			get { return this.cmdFins[4]; }
			set { this.cmdFins[4] = value; }
		}


		/// <summary>
		/// DA2 Destination unit address
		/// </summary>
		public Byte DA2
		{
			get { return this.cmdFins[5]; }
			set { this.cmdFins[5] = value; }
		}


		/// <summary>
		/// SNA Source network address (0=local network)
		/// </summary>
		public Byte SNA
		{
			get { return this.cmdFins[6]; }
			set { this.cmdFins[6] = value; }
		}


		/// <summary>
		/// SA1 Source node number
		/// </summary>
		public Byte SA1
		{
			get { return this.cmdFins[7]; }
			set { this.cmdFins[7] = value; }
		}


		/// <summary>
		/// SA2 Source unit address
		/// </summary>
		public Byte SA2
		{
			get { return this.cmdFins[8]; }
			set { this.cmdFins[8] = value; }
		}


		/// <summary>
		/// SID Service ID
		/// </summary>
		public Byte SID
		{
			get { return this.cmdFins[9]; }
			set { this.cmdFins[9] = value; }
		}


		/// <summary>
		/// MC Main command
		/// </summary>
		public Byte MC
		{
			get { return this.cmdFins[10]; }
			set { this.cmdFins[10] = value; }
		}


		/// <summary>
		/// SC Subcommand
		/// </summary>
		public Byte SC
		{
			get { return this.cmdFins[11]; }
			set { this.cmdFins[11] = value; }
		}
		
		#endregion
		

		
		#region **** frame send command & response fields

		/// <summary>
		/// FRAME SEND command length
		/// </summary>
		public UInt16 FS_LEN
		{
			get { return BTool.BytesToUInt16(this.cmdFS[6], this.cmdFS[7]); }
			set
			{
				this.cmdFS[6] = (Byte)((value >> 8) & 0xFF);
				this.cmdFS[7] = (Byte)(value & 0xFF);
			}
		}


		/// <summary>
		/// FRAME SEND response length
		/// </summary>
		public UInt16 FSR_LEN
		{
			get { return BTool.BytesToUInt16(this.respFS[6], this.respFS[7]); }
		}


		/// <summary>
		/// FRAME SEND response error
		/// </summary>
		public string FSR_ERR
		{
			get
			{
				return respFS[8].ToString()
						+ respFS[9].ToString()
						+ respFS[10].ToString()
						+ respFS[11].ToString();
			}
		}


		/// <summary>
		/// FRAME SEND response main code error
		/// </summary>
		public Byte FSR_MER
		{
			get { return respFins[12]; }
		}


		/// <summary>
		/// FRAME SEND response subcode error
		/// </summary>
		public Byte FSR_SER
		{
			get { return respFins[13]; }
		}
		
		#endregion

		#endregion



		#region **** constructor
		
		/// <summary>
		/// constructor, by default SID=0x01
		/// </summary>
		/// <param name="transportLayer"></param>
		/// <param name="ServiceID"></param>
		public tcpFINSCommand(Byte ServiceID=0x01)
		{
			// transport layer
			//
			this._transport = new mcOMRON.tcpTransport();

			// Default fins command fields
			//
			this.SID = ServiceID;
		} 


		/// <summary>
		/// set ip and port
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		public void SetTCPParams(IPAddress ip, int port)
		{
			this._transport.SetTCPParams(ip, port);
		}

		#endregion



		#region **** connect & close
		
		/// <summary>
		/// open the connection
		/// </summary>
		/// <returns></returns>
		public bool Connect()
		{
			try
			{
				this._transport.Connect();

				return NodeAddressDataSend();
			}
			catch (Exception ex)
			{

				this._lastError = ex.Message;
				return false;
			}
		}


		/// <summary>
		/// close the connection
		/// </summary>
		public void Close()
		{
			this._transport.Close();
		} 

		#endregion



		#region **** methods

		#region **** implemented fins commands

		/// <summary>
		/// MEMORY AREA READ
		/// </summary>
		/// <param name="area"></param>
		/// <param name="address"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public bool MemoryAreaRead(MemoryArea area, UInt16 address, Byte bit_position, UInt16 count)
		{
			try
			{
				// command & subcomand
				//
				this.MC = 0x01;
				this.SC = 0x01;

				// memory area
				//
				this.cmdFins[F_PARAM] = (Byte)area;

				// address
				//
				this.cmdFins[F_PARAM + 1] = (Byte)((address >> 8) & 0xFF);
				this.cmdFins[F_PARAM + 2] = (Byte)(address & 0xFF);

				// no bit position
				//
				this.cmdFins[F_PARAM + 3] = bit_position;

				// count items
				//
				this.cmdFins[F_PARAM + 4] = (Byte)((count >> 8) & 0xFF);
				this.cmdFins[F_PARAM + 5] = (Byte)(count & 0xFF);

				// set command lenght (12 + additional params)
				//
				this.finsCommandLen = 18;

				// send the message
				//
				return FrameSend(null);
			}
			catch (Exception ex)
			{
				this._lastError = ex.Message;
				return false;
			}
		}
		

		/// <summary>
		/// MEMORY AREA WRITE
		/// </summary>
		/// <param name="area"></param>
		/// <param name="address"></param>
		/// <param name="count"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool MemoryAreaWrite(MemoryArea area, UInt16 address, Byte bit_position, UInt16 count, ref Byte[] data)
		{
			try
			{
				// command & subcomand
				//
				this.MC = 0x01;
				this.SC = 0x02;

				// memory area
				//
				this.cmdFins[F_PARAM] = (Byte)area;

				// address
				//
				this.cmdFins[F_PARAM + 1] = (Byte)((address >> 8) & 0xFF);
				this.cmdFins[F_PARAM + 2] = (Byte)(address & 0xFF);

				// bit position
				//
				this.cmdFins[F_PARAM + 3] = bit_position;

				// count items
				//
				this.cmdFins[F_PARAM + 4] = (Byte)((count >> 8) & 0xFF);
				this.cmdFins[F_PARAM + 5] = (Byte)(count & 0xFF);

				// set command lenght (12 + additional params)
				//
				this.finsCommandLen = 18;

				// send the message
				//
				return FrameSend(data);
			}
			catch (Exception ex)
			{
				this._lastError = ex.Message;
				return false;
			}
		}
		

		/// <summary>
		/// CONNECTION DATA READ
		/// </summary>
		/// <param name="area"></param>
		/// <returns></returns>
		public bool ConnectionDataRead(Byte area)
		{
			try
			{
				// command & subcomand
				//
				this.MC = 0x05;
				this.SC = 0x01;

				// memory area
				//
				this.cmdFins[F_PARAM] = (Byte)area;

				// set command lenght (12 + additional params)
				//
				this.finsCommandLen = 13;

				return FrameSend(null);
			}
			catch (Exception ex)
			{
				this._lastError = ex.Message;
				return false;
			}
		} 
		
		#endregion


		#region **** node address data send & frame send

		/// <summary>
		/// 
		/// NODE ADDRESS DATA SEND
		/// 
		/// Must be executed once and before any other command. 
		/// Return an ID for PLC and PC
		/// </summary>
		public bool NodeAddressDataSend()
		{
			// NODE ADDRESS DATA SEND buffer
			//
			Byte[] cmdNADS = new Byte[] 
			{
			0x46, 0x49, 0x4E, 0x53, // 'F' 'I' 'N' 'S'
			0x00, 0x00, 0x00, 0x0C,	// 12 Bytes expected
			0x00, 0x00, 0x00, 0x00,	// NADS Command (0 Client to server, 1 server to client)
			0x00, 0x00, 0x00, 0x00,	// Error code (Not used)
			0x00, 0x00, 0x00, 0x00	// Client node address, 0 = auto assigned
			};


			// send NADS command
			//
			this.Transport.Send(ref cmdNADS, cmdNADS.Length);

			// wait for a plc response
			//
			Byte[] respNADS = new Byte[24];
			this.Transport.Receive(ref respNADS, respNADS.Length);


			// checks response error
			//
			if (respNADS[15] != 0)
			{
				this._lastError = "NASD command error: " + respNADS[15];

				// no more actions
				//
				Close();
				return false;
			}


			// checking header error
			//
			if (respNADS[8] != 0 || respNADS[9] != 0 || respNADS[10] != 0 || respNADS[11] != 1)
			{
				this._lastError = "Error sending NADS command. "
									+ respNADS[8].ToString() + " "
									+ respNADS[9].ToString() + " "
									+ respNADS[10].ToString() + " "
									+ respNADS[11].ToString();

				// no more actions
				//
				Close();

				return false;
			}


			// save the client & server node in the FINS command for all next conversations
			//
			this.DA1 = respNADS[23];
			this.SA1 = respNADS[19];

			return true;
		}



		/// <summary>
		/// FRAME SEND
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private bool FrameSend(Byte[] data)
		{
			// clear FS response buffer
			//
			for (int x = 0; x < this.respFS.Length; respFS[x++] = 0);


			// data lenght plus 8 bytes (4 bytes for command & 4 bytes for error)
			//
			int fsLen = this.finsCommandLen + 8;
			if (data != null)
			{
				fsLen += data.Length;
			}


			// set length [6]+[7]
			//
			this.FS_LEN = (UInt16)fsLen;


			// send frame header
			//
			this.Transport.Send(ref cmdFS, cmdFS.Length);


			// send FINS command
			//
			this.Transport.Send(ref this.cmdFins, this.finsCommandLen);


			// send additional data
			//
			if (data != null)
			{
				this.Transport.Send(ref data, data.Length);
			}


			// frame response
			//
			this.Transport.Receive(ref this.respFS, this.respFS.Length);


			// check frame error [8]+[9]+[10]+[11]
			//
			if (this.FSR_ERR != "0002")
			{
				this._lastError = "FRAME SEND error: " + this.FSR_ERR;
				return false;
			}


			// checks response error
			//
			if (this.respFS[15] != 0)
			{
				this._lastError = "Error receving FS command: " + this.respFS[15];
				return false;
			}


			// calculate the expedted response lenght
			//
			// 16 bits word ([6] + [7])
			// substract the additional 8 bytes
			//
			this.finsResponseLen = this.FSR_LEN;
			this.finsResponseLen -= 8;


			// fins command response
			//
			this.Transport.Receive(ref respFins, 14);


			if (finsResponseLen > 14)
			{
				// fins command response data
				//
				this.Transport.Receive(ref respFinsData, finsResponseLen - 14);
			}


			// check response code
			//
			if (this.FSR_MER != 0 || this.FSR_SER != 0)
			{
				this._lastError += string.Format("Response Code error: (Code: {0}  Subcode: {1})",
													this.FSR_MER, this.FSR_SER);
				return false;
			}

			return true;
		}
		
		#endregion

		#endregion



		#region **** debug dialog
		
		/// <summary>
		/// return last dialog between pc and plc
		/// </summary>
		/// <param name="Caption"></param>
		/// <returns></returns>
		public string LastDialog(string Caption)
		{
			StringBuilder dialog = new StringBuilder(Caption + Environment.NewLine);
			dialog.Append(Environment.NewLine);
			dialog.Append("FS HEADER" + Environment.NewLine);
			dialog.Append(BitConverter.ToString(this.cmdFS) + Environment.NewLine);
			dialog.Append("FINS COMMAND" + Environment.NewLine);
			dialog.Append(BitConverter.ToString(this.cmdFins,0,finsCommandLen) + Environment.NewLine);
			dialog.Append("FS RESPONSE" + Environment.NewLine);
			dialog.Append(BitConverter.ToString(this.respFS) + Environment.NewLine);
			dialog.Append("FINS RESPONSE" + Environment.NewLine);
			dialog.Append(BitConverter.ToString(this.respFins, 0, 14) + Environment.NewLine);
			dialog.Append("FINS DATA" + Environment.NewLine);
			dialog.Append(BitConverter.ToString(this.respFinsData, 0, finsResponseLen-14) + Environment.NewLine);
			dialog.Append("Last error: " + this._lastError + Environment.NewLine);
			dialog.Append(Environment.NewLine);

			return dialog.ToString();
		}

		#endregion		
		


		#region **** variables

		// last detected error
		//
		private string _lastError = "";


		// TCP transport layer
		//
		mcOMRON.tcpTransport _transport = null;


		// FRAME SEND header array
		//
		Byte[] cmdFS = new Byte[] 
		{
			0x46, 0x49, 0x4E, 0x53,		// 'F' 'I' 'N' 'S'
			0x00, 0x00, 0x00, 0x00,		// Expected number of bytes for response
			0x00, 0x00, 0x00, 0x02,		// Command FS  Sending=2 / Receiving=3
			0x00, 0x00, 0x00, 0x00		// Error code
		};


		// FRAME SEND Response array
		//
		Byte[] respFS = new Byte[]
			{
			0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00
			};


		// FINS RESPONSE (command)
		//
		Byte[] respFins = new Byte[2048];


		// FINS RESPONSE (data, 2KB reserved memory)
		//
		Byte[] respFinsData = new Byte[2048];


		// response data length
		//
		UInt16 finsResponseLen = 0;


		/// <summary>
		/// FINS COMMAND (2KB reserved memory)
		/// </summary>
		Byte[] cmdFins = new Byte[]
		{
			//---- COMMAND HEADER -------------------------------------------------------
			0x80,				// 00 ICF Information control field 
			0x00,				// 01 RSC Reserved 
			0x02,				// 02 GTC Gateway count
			0x00,				// 03 DNA Destination network address (0=local network)
			0x00,				// 04 DA1 Destination node number
			0x00,				// 05 DA2 Destination unit address
			0x00,				// 06 SNA Source network address (0=local network)
			0x00,				// 07 SA1 Source node number
			0x00,				// 08 SA2 Source unit address
			0x00,				// 09 SID Service ID
			//---- COMMAND --------------------------------------------------------------
			0x00,				// 10 MC Main command
			0x00,				// 11 SC Subcommand
			//---- PARAMS ---------------------------------------------------------------
			0x00,				// 12 reserved area for additional params
			0x00,				// depending on fins command
			0x00,
			0x00,
			0x00,
			0x00,
			0x00,
			0x00,
			0x00,
			0x00,
		};
		
		// command length
		//
		UInt16 finsCommandLen = 0;

		/// <summary>
		/// first position of PARAM area in the fins command
		/// </summary>
		const int F_PARAM = 12;

		#endregion	
	}
}
 