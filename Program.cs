using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace rock_paper_scissors
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3 || args.Length % 2 == 0)
            {
                Console.WriteLine("Incorrect arguments quantity --- must be >=3 and odd");
                return;
            }
            HashSet<string> set = new HashSet<string>();
            foreach (var el in args) set.Add(el);
            if (set.Count != args.Length)
            {
                Console.WriteLine("strings must be unique, try again");
                return;
            }
            RandomNumberGenerator rnd = RandomNumberGenerator.Create();
            byte[] bytes = new byte[16];
            rnd.GetBytes(bytes);
            string key = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
            long choice = BitConverter.ToUInt32(bytes, 0) % args.Length;
            var encoding = new ASCIIEncoding();
            byte[] msg_bytes = encoding.GetBytes(args[choice]);
            HMACSHA256 hmac = new HMACSHA256(bytes);
            string hmac_value = BitConverter.ToString(hmac.ComputeHash(msg_bytes), 0).Replace("-", string.Empty);
            PrintInfo(args, hmac_value);
            List<int> win = new List<int>();
            for (int i = 1; i <= args.Length / 2; i++)
            {
                int temp = (int)choice + i;
                if (temp >= args.Length) temp = temp - args.Length;
                win.Add(temp);
            }
            int p = Convert.ToInt32(Console.ReadLine());
            Handler(args, key, choice, win, p);
        }

        private static int Handler(string[] args, string key, long choice, List<int> win, int p)
        {
            if (p == 0) Environment.Exit(0);
            p = p - 1;
            Console.WriteLine("Your move: " + args[Convert.ToInt32(p)]);
            Console.WriteLine("Computer move: " + args[choice]);
            if (choice == p)
            {
                Console.WriteLine("Draw!");
                Console.WriteLine("HMAC Key: " + key);
            }
            else if (win.Contains(p))
            {
                Console.WriteLine("You win!");
                Console.WriteLine("HMAC Key: " + key);
            }

            else
            {
                Console.WriteLine("You lose!");
                Console.WriteLine("HMAC Key: " + key);
            }

            return p;
        }

        private static void PrintInfo(string[] args, string hmac_value)
        {
            Console.WriteLine("HMAC: " + hmac_value);
            Console.WriteLine("Available moves:");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(i + 1 + " - " + args[i]);
            }
            Console.WriteLine("0 - exit");
            Console.Write("Enter your move: ");
        }
    }
}
