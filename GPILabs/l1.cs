using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPILabs
{
	internal class l1
	{

		public static List<byte> RGBToBW(List<byte> data)
		{
			List<byte> result = new List<byte>(data);
			int ColorsCount = BitConverter.ToInt32(data.GetRange(46, 50).ToArray(),0);
			Console.WriteLine(ColorsCount);

			for(int i = 0; i<ColorsCount; i++)
			{
				int R = data[54 + (i * 4)];
				int G = data[55 + (i * 4)];
				int B = data[56 + (i * 4)];
				
				//int NewColor = ((int)((R * 0.3) + (G * 0.59) + (B * 0.11)))/3; мне не понравилось затемнение
				int NewColor = ((int)(R + G + B))/3;
				result[54 + (i * 4)] = (byte)NewColor;
				result[55 + (i * 4)] = (byte)NewColor;
				result[56 + (i * 4)] = (byte)NewColor;
			}

			return result;
		}






		public static List<byte> GetBytesFromBMP(string path)
		{
			List<byte> bytes = new List<byte>();

			try
			{
				bytes = File.ReadAllBytes(path).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Ошибка при чтении файла: " + ex.Message);
				return null;
			}


			return bytes;
		}

		public static void SetBytesToBMP(string path, List<byte> bytes)
		{
			try
			{
				// Записываем массив байтов в файл
				File.WriteAllBytes(path, bytes.ToArray());
				Console.WriteLine("Файл успешно записан.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при записи файла: {ex.Message}");
			}
		}


	}
}
