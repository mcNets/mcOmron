using System;

namespace mcOMRON
{
	/// <summary>
	/// 
	/// Version:	1.0
	/// Author:		Joan Magnet
	/// Date:		02/2015
	/// 
	/// base interface for transport's layer class
	/// </summary>
	public interface ITransport
	{
		/// <summary>
		/// returns the connexion status
		/// </summary>
		bool Connected
		{
			get;
		}


		/// <summary>
		/// establish a connection with the PLC
		/// </summary>
		bool Connect();


		/// <summary>
		/// close the connection with the plc
		/// </summary>
		void Close();


		/// <summary>
		/// send a command to the PLC
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cmdLen"></param>
		int Send(ref Byte[] command, int cmdLen);


		/// <summary>
		/// receive a response from the PLC
		/// </summary>
		/// <param name="response"></param>
		/// <param name="respLen"></param>
		int Receive(ref Byte[] response, int respLen);
	}
}
