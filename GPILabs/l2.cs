using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GPILabs
{
	internal class l2
	{
		public static List<byte> AddBorder(List<byte> data)
		{
			
			List<byte> result = new List<byte>(data.GetRange(0, 54));

			int width = BitConverter.ToInt32(data.GetRange(18, 4).ToArray(), 0);
			int height = BitConverter.ToInt32(data.GetRange(22, 4).ToArray(), 0);
			int originalStride = (((data.Count - 54) / height) % width);
			byte[] newWidth = BitConverter.GetBytes(BitConverter.ToInt32(data.GetRange(18, 4).ToArray(), 0) + 30);
			byte[] newHeight = BitConverter.GetBytes(BitConverter.ToInt32(data.GetRange(22, 4).ToArray(), 0) + 30);
			for (int i = 0; i<4; i++)
			{
				result[18+i] = newWidth[i];
				result[22+i] = newHeight[i];
			}
			//заполнение верхней стороны рамки
			Console.WriteLine(result.Count + " | ");
			for(int i = 0; i < 15; i++)
			{
				for(int j = 0; j< BitConverter.ToInt32(newWidth, 0); j++)
				{
					for(int q = 0; q < 3; q++)
					{
						result.Add(RandomByte());
					}
				}
				while ((result.Count - 54) % 4 != 0)
				{
					result.Add(0);
				}
			}

			Console.Write(result.Count + " | ");
			int currentIndex = 54;
			//заполнение существующих строк с рамками в начале и конце
			for(int i = 0; i< BitConverter.ToInt32(newHeight, 0)-30; i++)
			{
				for (int q = 0; q < 45; q++)
				{
					result.Add(RandomByte());
				}

				//костыль для пропуска уже готовых отступов для добивания до 4 байт и запись оригинального изображения
				for(int w = 0; w< (BitConverter.ToInt32(newWidth, 0) - 30)*3; w++)
				{

					result.Add(data[currentIndex]);
					currentIndex++;
				}
				currentIndex += originalStride;

				for (int q = 0; q < 45; q++)
				{
					result.Add(RandomByte());
				}
				while ((result.Count - 54) % 4 != 0)
				{
					result.Add(0);
				}

				Console.WriteLine(result.Count + " | ");
			}
			//заполнение нижней стороны рамки
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < BitConverter.ToInt32(newWidth, 0); j++)
				{
					for (int q = 0; q < 3; q++)
					{
						result.Add(RandomByte());
					}
				}
				while ((result.Count - 54) % 4 != 0)
				{
					result.Add(0);
				}
			}

			Console.WriteLine(result.Count + " | ");

			return result;
		}

		public static byte RandomByte()
		{
			var rand = new Random();
			return (byte)rand.Next(0, 255);
			/*return 0;*/
		}
	}
}
