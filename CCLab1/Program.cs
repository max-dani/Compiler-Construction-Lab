using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCLab1
{
	public class Program
	{
        public static int Main(string[] args)

        {
			Console.WriteLine("Scientific Calculator");
			Console.WriteLine("Choose an operation:");
			Console.WriteLine("1. Sine");
			Console.WriteLine("2. Cosine");
			Console.WriteLine("3. Tangent");
			Console.WriteLine("4. Logarithm");

			int choice = int.Parse(Console.ReadLine());

			Console.WriteLine("Enter the number:");
			double number = double.Parse(Console.ReadLine());

			double result = 0;

			switch (choice)
			{
				case 1:
					result = Math.Sin(number);
					break;
				case 2:
					result = Math.Cos(number);
					break;
				case 3:
					result = Math.Tan(number);
					break;
				case 4:
					result = Math.Log(number);
					break;
				default:
					Console.WriteLine("Invalid choice");
					break;
			}

			Console.WriteLine("The result is: " + result);
			return 0;
        }
    }
}
