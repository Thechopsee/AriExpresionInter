using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq.Expressions;

namespace numpreter
{
    class Vyraz {
        public string list;
        public string vysledek;
        public string vysl2;
        public string data;
        public void computeData()
        {
            string[] cisla = list.Split(new string[] { "*", "+", "-", "/" }, StringSplitOptions.RemoveEmptyEntries);
            string[] znamenka =list.Split(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> cislal = new List<string>(cisla);
            List<string> znamenkal = new List<string>(znamenka);

            for (int i = 0; i < znamenkal.Count; i++)
            {
                if (znamenkal[i] == "/")
                {
                    cislal[i] = (Int32.Parse(cislal[i]) / Int32.Parse(cislal[i + 1])) + "";
                    cislal.RemoveAt(i+1);
                    znamenkal.RemoveAt(i);
                    continue;
                }
                else if (znamenkal[i] == "*")
                {
                    cislal[i] = (Int32.Parse(cislal[i]) * Int32.Parse(cislal[i + 1])) + "";
                    cislal.RemoveAt(i + 1);
                    znamenkal.RemoveAt(i);
                    continue;
                }
            }
            int x = 0;
            while (znamenkal.Count!=0)
            {
                if (znamenkal[x] == "+")
                {
                    cislal[x] = (Int32.Parse(cislal[x]) + Int32.Parse(cislal[x + 1])) + "";
                    cislal.RemoveAt(x + 1);
                    znamenkal.RemoveAt(x);
                    continue;
                }
                else if (znamenkal[x] == "-")
                {
                    cislal[x] = (Int32.Parse(cislal[x]) - Int32.Parse(cislal[x + 1])) + "";
                    cislal.RemoveAt(x + 1);
                    znamenkal.RemoveAt(x);
                    continue;
                }
                x++;
            }
            vysl2 = cislal[0];
            }
        public Vyraz(string data)
        {
            nevimco(data);
        }
        public void nevimco(string data)
        {
            this.data = data;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '(')
                {
                    string neco = "";
                    for (int j = i+1; j < data.Length; j++)
                    {
                        neco += data[j];
                    }
                    Vyraz temp = new Vyraz(neco);
                    list += temp.vysledek;
                    i += temp.data.Length;
                }
                else
                {
                    if (data[i] != ')')
                    {
                        list+=data[i];
                    }
                }

            }
            DataTable dt = new DataTable();
            var v = dt.Compute(this.list, "");
            vysledek += v;
            this.computeData();
            

        }
    }
    class Program
    {

        private static bool checkValidity(string expression)
        {
            Stack<char> openStack = new Stack<char>();
            int counter = 0;
            foreach (char c in expression)
            {
                switch (c)
                {
                    case '{':
                    case '(':
                    case '[':
                        openStack.Push(c);
                        break;
                    case '}':
                        if (openStack.Count == 0 || openStack.Peek() != '{')
                        {
                            return false;
                        }
                        openStack.Pop();
                        break;
                    case ')':
                        if (openStack.Count == 0 || openStack.Peek() != '(')
                        {
                            return false;
                        }
                        openStack.Pop();
                        break;
                    case ']':
                        if (openStack.Count == 0 || openStack.Peek() != '[')
                        {
                            return false;
                        }
                        openStack.Pop();
                        break;
                    default:
                        break;
                }
                counter++;
            }
            return openStack.Count == 0;
        }

        static bool opakovani(string data)
        {
            int counter=0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '/' || data[i] == '*' || data[i] == '+' || data[i] == '-')
                {
                    counter++;
                    if (data[i+1] == '/' || data[i+1] == '*' || data[i+1] == '+' || data[i+1] == '-')
                    {
                        return false;
                    }
                }
            }
            if (counter == 0)
            {
                return false;
            }
            return true;
        }

    static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("Wrong arguments use -u or -f");
                return;
            }
            if (String.Compare(args[0], "-u") == 0)
            {
                while (true)
                {
                    string data = Console.ReadLine();
                    if (String.Compare(data, "quit") == 0)
                    {
                        return;
                    }
                    bool vysl = checkValidity(data);
                    bool opak = opakovani(data);
                    if (vysl && opak)
                    {
                        DataTable dt = new DataTable();
                        var v = dt.Compute(data, "");
                        //Console.WriteLine("kontrolni vysl:" + v);

                        Vyraz vz = new Vyraz(data);
                        //Console.WriteLine(vz.list);
                        //Console.WriteLine("kotrolni vysledek bez počtu:" + vz.vysledek);
                        Console.WriteLine("final výsledek:" + vz.vysl2);
                        vz.computeData();
                    }
                    else
                    {
                        Console.WriteLine("ERROR");
                    }

                }
            }
            else if (String.Compare(args[0], "-f") == 0)
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("No file specified");
                    return;
                }
                string[] lines = File.ReadAllLines(args[1]);
                List<string> vysledky = new List<string>();
                foreach (string data in lines)
                {
                    Console.WriteLine(data);
                    if (String.Compare(data, "quit") == 0)
                    {
                        return;
                    }
                    bool vysl = checkValidity(data);
                    bool opak = opakovani(data);
                    if (vysl && opak)
                    {
                        DataTable dt = new DataTable();
                        var v = dt.Compute(data, "");
                        //Console.WriteLine("kontrolni vysl:" + v);

                        Vyraz vz = new Vyraz(data);
                        //Console.WriteLine(vz.list);
                        //Console.WriteLine("kotrolni vysledek bez počtu:" + vz.vysledek);
                        Console.WriteLine("final výsledek:" + vz.vysl2);
                        vysledky.Add(vz.vysl2);
                        //vz.computeData();
                    }
                    else
                    {
                        Console.WriteLine("ERROR");
                        vysledky.Add("ERROR");
                    }
                }
                Console.WriteLine(" ");
                foreach (string n in vysledky)
                {
                    Console.WriteLine(n);
                }
            }
            
        }
    }
}
