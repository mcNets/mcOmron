using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_btools
{
	class Program
	{
		static void Main(string[] args)
		{
			Byte value = 0;

			Console.WriteLine("SetBit 4");
			value = mcOMRON.BTool.SetBit(value, 4);
			PrintInfo(value);

			Console.WriteLine("value=175");
			value = 175;
			PrintInfo(value);

			Console.WriteLine("UnsetBit 2");
			value = mcOMRON.BTool.UnsetBit(value, 2);
			PrintInfo(value);
		}


		static void PrintInfo(Byte value)
		{
			Console.WriteLine("Value =  " + value.ToString() + "  Bin: " + mcOMRON.BTool.ToBinaryString(value));
			for (int x=0; x < 8; x++)
			{
				string msg = string.Format("IsBitSet  Pos:{0}  {1}", x, ((mcOMRON.BTool.IsBitSet(value, x) ? "yes" : "no")));
				Console.WriteLine(msg);
			}
			Console.WriteLine("");

			Console.ReadKey();
		}
	}
}
