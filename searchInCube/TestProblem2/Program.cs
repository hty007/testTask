using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace TestProblem2
{
	class MainClass
	{	
		const int Nmax = 1000;

		//public static bool TestMode = false;
		public static List<int> OutArea = new List<int>();
		public static Slot[, ,] DataSlot = new Slot[Nmax, Nmax, Nmax];


		static int Nx, Ny, Nz;
		//static int NowArea;

		public static void AddSlot(int i, int j, int k)
		{
			int x = i; int y = j; int z = k;

			// Область
			DataSlot[x, y, z] = Slot.check;
			Queue<Point> Area = new Queue<Point>();
			Point m	= new Point (x, y, z);// оперативная единица		 
			Area.Enqueue (m);// Первая ячейка в стеке

			do {//Найти всех соседей этой ячейки и добавить их в очередь
				m = Area.Dequeue ();
				x = m.x;
				y = m.y;
				z = m.z;

				OutArea.Add(Nx*Ny*(m.z)+Nx*(m.y)+m.x+1);//Добовляем ячейку для вывода
				if ((x < Nx - 1) && (DataSlot [x + 1, y, z] == Slot.value))
					{m.Take(x + 1, y, z);
					Area.Enqueue (m);}
				if ((x-1 > 0) && (DataSlot [x - 1, y, z] == Slot.value))
					{m.Take(x - 1, y, z);
					Area.Enqueue (m);}									
				if ((y < Ny - 1) && (DataSlot [x, y + 1, z] == Slot.value))
					{m.Take(x, y + 1, z);
					Area.Enqueue (m);}											
				if ((y-1 > 0) && (DataSlot [x, y - 1, z] == Slot.value))
					{m.Take(x, y - 1, z);
					Area.Enqueue (m);}											
				if ((z < Nz - 1) && (DataSlot [x, y, z + 1] == Slot.value))
				{
					m.Take(x, y, z + 1);
					Area.Enqueue (m);
				}
				if ((z-1 > 0) && (DataSlot [x, y, z - 1] == Slot.value))
				{
					m.Take(x, y, z - 1);
					Area.Enqueue (m);
				}
//				if (TestMode) 	
//					Console.WriteLine ("Количество ячеек: "+Area.Count+"\t"+
//					"Текущая ячейка: "+ x.ToString() +" "+ y.ToString() + " "+ z.ToString()+"\r");				
			} while (Area.Count > 0);// Выполняем цикл пока очередь не опустеет

		}


		// Рекурсия слишком "глубокая" вызывает переполнение стека!!!
//		public static void AddSlot(int i, int j, int k)
//		{
//			
//			if (DataSlot [i, j, k] == Slot.value) {
//				DataSlot [i, j, k] = Slot.check;
//				Area[NowArea]= i+ Nx*(j-1)+Ny*Nx*(k-1);
//				NowArea++;
//				Console.Write ($"Нашла {NowArea} ячеек! \r");
//				if (i < Nx - 1)
//					AddSlot (i + 1, j, k);
//				if (i > 0)
//					AddSlot (i - 1, j, k);
//				if (j < Ny - 1)
//					AddSlot (i, j + 1, k);
//				if (j > 0)
//					AddSlot (i, j - 1, k);
//				if (k < Nz - 1)
//					AddSlot (i, j, k + 1);
//				if (k > 0)
//					AddSlot (i, j, k - 1);
////				
//
//			}
//		}



		public static void Main (string[] args)
		{	Stopwatch watch = new Stopwatch();
			watch.Start();
		// ============= Ввод ==================
			string path = Path.Combine (Environment.CurrentDirectory, "input.txt");
			StreamReader input = new StreamReader (path);
			Nx = int.Parse (input.ReadLine());
			Ny = int.Parse (input.ReadLine());
			Nz = int.Parse (input.ReadLine());

			Console.WriteLine ( "Nx = "+ Nx + "\t Ny = "+ Ny + "\t Nz = "+ Nz);
			Console.WriteLine ("Загружаю фаил:   "+path);
			int m;
			for (int k = 0; k < Nz; k++) {					
				for (int j = 0; j < Ny; j++) {	
					for (int i = 0; i < Nx; i++) {
						m = input.Read ();
						DataSlot [i, j, k] = (Slot)(m - 48);
					}
					input.ReadLine ();	
				}
				input.ReadLine ();
				Console.Write ($"Загружаю слой #{k}  \r");
			}
			input.Close();
			Console.WriteLine ("Фаил загружен. Время "+ watch.ElapsedMilliseconds + "мс.");
		// =====================================
			/*    ---Работа программы---   */
		// =====================================
			path = Path.Combine (Environment.CurrentDirectory, "output.txt");
			StreamWriter output = new StreamWriter (path);
			//NowArea = 0;
			int CountArea = 0;
			for (int k = 0; k < Nz; k++) {					
				for (int j = 0; j < Ny; j++) {	
					for (int i = 0; i < Nx; i++) {
						if (DataSlot [i, j, k] == Slot.value) {
							AddSlot (i, j, k);
							CountArea++;
							// Вывод на экран и в фаил
							Console.Write ($"Область №{CountArea} ({OutArea.Count})\t\t\r");
							output.WriteLine ($"Область №{CountArea} ({OutArea.Count})");
							int count = 0;
							foreach (int element in OutArea)
							{
								count++;
								output.WriteLine($"Ячейка #{count}: {element}");
							}
							OutArea.Clear ();
						} else
							DataSlot [i, j, k] = Slot.check;
					}
				}
			}
			output.Close ();

		// ============= Вывод =================


		// =====================================
			watch.Stop();
			Console.WriteLine ("");
			Console.WriteLine(
				"\n Время выполнения программы в миллисекундах : " + watch.ElapsedMilliseconds + "мс.\r\n"+
				"Время выполнения программы в секундах : " + watch.Elapsed.Minutes+ " мин. "+ watch.Elapsed.Seconds + " сек.\r\n"+
				"Время выполнения программы в Тиках :" + watch.ElapsedTicks+"\r\n"

			);
			Console.Write ("Нажми клавишу!");
			Console.ReadKey ();
		}
	}
}
