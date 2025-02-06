using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPILabs
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string fileOpenPath = "";
		string fileSavePath = "";
		string logoOpenPath = "";
		public MainWindow()
		{
			InitializeComponent();
			
		}

		private void l1OpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();


			if (openFileDialog.ShowDialog() == true)
			{
				fileOpenPath = openFileDialog.FileName;
			}
			Console.WriteLine(fileOpenPath);
		}

		private void l1Edit_Click(object sender, RoutedEventArgs e)
		{
			List<byte> data = l1.GetBytesFromBMP(fileOpenPath);
			List<byte> result = l1.RGBToBW(data);

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (saveFileDialog.ShowDialog() == true)
			{
				fileSavePath = saveFileDialog.FileName;
			}
			l1.SetBytesToBMP(fileSavePath, result);
		
		}

		private void l2Edit_Click(object sender, RoutedEventArgs e)
		{
			List<byte> data = l1.GetBytesFromBMP(fileOpenPath);
			List<byte> result = l2.AddBorder(data);

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (saveFileDialog.ShowDialog() == true)
			{
				fileSavePath = saveFileDialog.FileName;
			}
			l1.SetBytesToBMP(fileSavePath, result);
		}

        private void l3Edit_Click(object sender, RoutedEventArgs e)
        {
            List<byte> data = l1.GetBytesFromBMP(fileOpenPath);
            List<byte> result = l3.RotateBMP(data);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
				fileSavePath = saveFileDialog.FileName;
            }
            l1.SetBytesToBMP(fileSavePath, result);
        }

        private void l4Edit_Click(object sender, RoutedEventArgs e)
        {
            List<byte> data = l1.GetBytesFromBMP(fileOpenPath);
			l4.printBMP(data, outputImage);

        }

		private void l5Downscale_Click(object sender, RoutedEventArgs e)
		{
            int scale = Int32.Parse(scaleTextBox.Text);
            List<byte> data = l1.GetBytesFromBMP(fileOpenPath);
            List<byte> result = l5.DownscaleBMP(data, scale);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
				fileSavePath = saveFileDialog.FileName;
            }
            l1.SetBytesToBMP(fileSavePath, result);
        }

		private void l5Upscale_Click(object sender, RoutedEventArgs e)
		{
			int scale = Int32.Parse(scaleTextBox.Text);
            List<byte> data = l1.GetBytesFromBMP(fileOpenPath);
            List<byte> result = l5.UpscaleBMP(data, scale);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
				fileSavePath = saveFileDialog.FileName;
            }
            l1.SetBytesToBMP(fileSavePath, result);
        }

		private void l6OpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();


			if (openFileDialog.ShowDialog() == true)
			{
				logoOpenPath = openFileDialog.FileName;
			}
			Console.WriteLine(logoOpenPath);
		}

		private void l6Edit_Click(object sender, RoutedEventArgs e)
		{
			List<byte> data = l1.GetBytesFromBMP(fileOpenPath);
			List<byte> logo = l1.GetBytesFromBMP(logoOpenPath);
			float k = float.Parse(opacityTextBox.Text);
			List<byte> result = l6.SetLogo(data, logo, k, 100, 100);

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (saveFileDialog.ShowDialog() == true)
			{
				fileSavePath = saveFileDialog.FileName;
			}
			l1.SetBytesToBMP(fileSavePath, result);
		}

	}
}
