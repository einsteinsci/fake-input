using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FakeInput.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Thread trinity = new Thread(() => {
				Thread.Sleep(1000);
				InputFaker.SendString("Wake up Neo ...", 100);
				Thread.Sleep(1500);
				InputFaker.SendString("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
				InputFaker.SendString("The Matrix has you.", 150);
				Thread.Sleep(3000);
				InputFaker.SendString("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
				InputFaker.SendString("Follow the white rabbit.", 100);
				Thread.Sleep(2000);
				InputFaker.SendString("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
				InputFaker.SendString("Knock, knock, Neo.", 100);
				InputFaker.SendEnter();
			});
			trinity.Start();
			InputFaker.SetWindowTitle("Trinity");
			
			Console.WriteLine();
			Console.Write("  ");
			Console.ReadLine();
			trinity.Abort();

			Console.WriteLine();
			Console.Write("> ");
			Console.ReadKey();
		}
	}
}
