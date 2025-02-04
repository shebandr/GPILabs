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
		string lab1path = "";
		public MainWindow()
		{
			InitializeComponent();
			
		}

		private void l1OpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();


			if (openFileDialog.ShowDialog() == true)
			{
				lab1path = openFileDialog.FileName;
			}
			Console.WriteLine(lab1path);
		}

		private void l1Edit_Click(object sender, RoutedEventArgs e)
		{
			List<byte> data = l1.GetBytesFromBMP(lab1path);
			List<byte> result = l1.RGBToBW(data);

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (saveFileDialog.ShowDialog() == true)
			{
				lab1path = saveFileDialog.FileName;
			}
			Console.WriteLine(lab1path);
			l1.SetBytesToBMP(lab1path, result);
		
		}

		private void l2Edit_Click(object sender, RoutedEventArgs e)
		{
			List<byte> data = l1.GetBytesFromBMP(lab1path);
			List<byte> result = l2.AddBorder(data);

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (saveFileDialog.ShowDialog() == true)
			{
				lab1path = saveFileDialog.FileName;
			}
			Console.WriteLine(lab1path);
			l1.SetBytesToBMP(lab1path, result);
		}

        private void l3Edit_Click(object sender, RoutedEventArgs e)
        {
            List<byte> data = l1.GetBytesFromBMP(lab1path);
            List<byte> result = l3.RotateBMP(data);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                lab1path = saveFileDialog.FileName;
            }
            Console.WriteLine(lab1path);
            l1.SetBytesToBMP(lab1path, result);
        }

        private void l4Edit_Click(object sender, RoutedEventArgs e)
        {
            List<byte> data = l1.GetBytesFromBMP(lab1path);
			l4.printBMP(data, outputImage);

        }
    }
}
