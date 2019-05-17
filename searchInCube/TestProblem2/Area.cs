using System;
using System.Collections.Generic;

namespace TestProblem2
{
	struct Point
	{
		public int x, y, z;
		public Point(int newX, int newY, int newZ) 
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
			MainClass.DataSlot [x , y, z] = Slot.check;
		}
		public void Take(int newX, int newY, int newZ) 
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
			MainClass.DataSlot [x , y, z] = Slot.check;
//			if (MainClass.TestMode) {
//				Console.WriteLine ($"Добавил {x} {y} {z}");
//			}
		}
	}


	/// <summary>
	/// Ячейка
	/// </summary>
	enum Slot: byte
	{
		check = 200,
		zero  =  0,
		value =  1
	}




}

