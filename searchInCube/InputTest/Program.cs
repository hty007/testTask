using System;
using System.IO;
using System;
//Имеется куб целочисленных данных (куб признаков 0 или 1). Отдельные элементы 
//куба будем называть ячейками. Размеры куба заданы (в направлении I: Nx элелентов, 
//	в направлении J: Ny, в направлении K: Nz). Куб хранится в одномерном массиве 
//(размером Nx*Ny*Nz), данные в нем уложены в следующем порядке: вначале меняется 
//индекс I, потом меняется индекс J, затем меняется индекс K.


namespace InputTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// =============Ввод с клавиатуры =================
			string path = Path.Combine (Environment.CurrentDirectory, "input.txt");
			//Console.WriteLine (path);
			Console.WriteLine ("Создаем куб целочисленных данных (куб признаков 0 или 1).\n\r" 
				+"Размеры куба Nx*Ny*Nz. Куб хранится в файле \n\r" + path);
			Console.Write ("Введите целое число: Nx = ");
			int Nx = int.Parse(Console.ReadLine());
			Console.Write ("Введите целое число: Ny = ");
			int Ny = int.Parse(Console.ReadLine());
			Console.Write ("Введите целое число: Nz = ");
			int Nz = int.Parse(Console.ReadLine());
			Console.Write ("Введите частоту выподаний ''1'', от 1(редко) до 5(часто): V = ");
			int V = int.Parse(Console.ReadLine());

			// ============= Обработка данных, вывод в фаил =================
			Random rnd = new Random();
			StreamWriter input = new  StreamWriter(path,false,System.Text.Encoding.Default);
			input.WriteLine (Nx);
			input.WriteLine (Ny);
			input.WriteLine (Nz);
			int m ;

 
			for (int k = 1; k <= Nz; k++) {					
				for (int j = 1; j <= Ny; j++) {	
					for (int i = 1; i <= Nx; i++) {
						m = rnd.Next (1, V+9);
						input.Write (Math.Round ((double)m/15));
					}
					input.WriteLine ("");	
				}
				input.WriteLine ("");
			}
			input.Close ();	}
	}
}
