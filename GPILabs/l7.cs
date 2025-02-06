using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GPILabs
{
	internal class l7
	{
		public static List<byte> TextToBMP(List<byte> data, List<byte> text, int bits)
		{
			List<byte> result = new List<byte>(data.GetRange(0, 54));
			List<bool> bitList = ByteListToBitList(text);
			int width = BitConverter.ToInt32(data.GetRange(18, 4).ToArray(), 0);
			int height = BitConverter.ToInt32(data.GetRange(22, 4).ToArray(), 0);
			int currentIndex = 54;
			int originalStride = ((data.Count-54) / height) % width;
			int currentBit = 0;

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					for (int q = 0; q < 3; q++)
					{
						byte tempByte = data[currentIndex];
						for(int w = 0; w<bits; w++)
						{
							if(currentBit != bitList.Count - 1)
								{
								
							
								if (bitList[currentBit])
								{
									tempByte |= (byte)(1 << w);
								}
								else
								{
									tempByte &= (byte)~(1 << w); 
								}
								currentBit++;
							}
						}


						result.Add(tempByte);

						currentIndex++;
					}
				}
				currentIndex += originalStride;
				while ((result.Count - 54) % 4 != 0)
				{
					result.Add(0);
				}
			}


			return result;
		}


		public static List<byte> BMPToText(List<byte> data, int bits)
		{
			List<byte> result = new List<byte>(data.GetRange(0, 54));
			List<bool> bitList = new List<bool>();
			int width = BitConverter.ToInt32(data.GetRange(18, 4).ToArray(), 0);
			int height = BitConverter.ToInt32(data.GetRange(22, 4).ToArray(), 0);
			int currentIndex = 54;
			int originalStride = ((data.Count - 54) / height) % width;
			int currentBit = 0;

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					for (int q = 0; q < 3; q++)
					{
						byte tempByte = data[currentIndex];
						for (int w = 0; w < bits; w++)
						{

							bool bitValue = (tempByte & (1 << w)) != 0;

						
							bitList.Add(bitValue);

							currentBit++;
						}


						

						currentIndex++;
					}
				}
				currentIndex += originalStride;

			}


			return BitListToByteList(bitList);
		}

		public static List<bool> ByteListToBitList(List<byte> byteList)
		{
			BitArray bitArray = new BitArray(byteList.ToArray());
			List<bool> bitList = new List<bool>();

			foreach (bool bit in bitArray)
			{
				bitList.Add(bit);
			}

			return bitList;
		}


		public static List<byte> BitListToByteList(List<bool> bitList)
		{
			List<byte> byteList = new List<byte>();
			int byteCount = (bitList.Count + 7) / 8; 

			for (int i = 0; i < byteCount; i++)
			{
				byte b = 0;

				
				for (int j = 0; j < 8; j++)
				{
					int bitIndex = i * 8 + j;

					
					if (bitIndex < bitList.Count && bitList[bitIndex])
					{
						b |= (byte)(1 << j); 
					}
				}

				byteList.Add(b); 
			}

			return byteList;
		}



	}
}
