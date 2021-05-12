using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Shared
{
    public static class StreamUtil
    {
		//Need to read on this class, but for now am too tired to take this in
		private const int HEADER_SIZE = 4;

		/**
		 * Optimized 'Available' check that FIRST checks if header data is available and THEN checks 
		 * if the actual data is available as well.
		 */
		public static bool Available(TcpClient pClient)
		{
			if (pClient.Available < HEADER_SIZE) return false;
			byte[] sizeHeader = new byte[HEADER_SIZE];
			pClient.Client.Receive(sizeHeader, HEADER_SIZE, SocketFlags.Peek);
			int messageSize = BitConverter.ToInt32(sizeHeader, 0);
			return pClient.Available >= HEADER_SIZE + messageSize;
		}

		/**
		 * Writes the size of the given byte array into the stream and then the bytes themselves.
		 */
		public static void Write(NetworkStream pStream, byte[] pMessage)
		{
			Console.WriteLine("Doing our best to write something to the stream");

			//convert message length to 4 bytes and write those bytes into the stream
			pStream.Write(BitConverter.GetBytes(pMessage.Length), 0, HEADER_SIZE);
			//now send the bytes of the message themselves
			pStream.Write(pMessage, 0, pMessage.Length);
		}

		/**
		 * Reads the amount of bytes to receive from the stream and then the bytes themselves.
		 */
		public static byte[] Read(NetworkStream pStream)
		{
			Console.WriteLine("Doing our best to read something from the stream");

			//get the message size first
			int byteCountToRead = BitConverter.ToInt32(Read(pStream, HEADER_SIZE), 0);
			//then read that amount of bytes
			return Read(pStream, byteCountToRead);
		}

		/**
		 * Read the given amount of bytes from the stream
		 */
		private static byte[] Read(NetworkStream pStream, int pByteCount)
		{
			//create a buffer to hold all the requested bytes
			byte[] bytes = new byte[pByteCount];
			//keep track of how many bytes we read last read operation
			int bytesRead = 0;
			//and keep track of how many bytes we've read in total
			int totalBytesRead = 0;

			try
			{
				//keep reading bytes until we've got what we are looking for or something bad happens.
				while (
					totalBytesRead != pByteCount &&
					(bytesRead = pStream.Read(bytes, totalBytesRead, pByteCount - totalBytesRead)) > 0
				)
				{
					totalBytesRead += bytesRead;
				}
			}
			catch { }

			return (totalBytesRead == pByteCount) ? bytes : null;
		}
	}

}

