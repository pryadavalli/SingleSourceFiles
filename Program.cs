using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            char[] inputExp = { '{', '(', ')', '}', '[', ']' };
            var input = "";
            while (input != "#")
            {
                 input = Console.ReadLine(); 
                if (input != "" && input != "#")
                {
                    inputExp = input.ToCharArray();
                    if (IsValidParanthesisExpression(inputExp))
                        Console.WriteLine("Valid Expression ");
                    else
                        Console.WriteLine("Not Valid Expression ");
                }
                else
                    Console.WriteLine("Type # to Exit or exitting now");
            }
        }
        static Boolean isValidPair(char firstChar, char secondChar)
        {
            if (firstChar == '(' && secondChar == ')')
                return true;
            else if (firstChar == '{' && secondChar == '}')
                return true;
            else if (firstChar == '[' && secondChar == ']')
                return true;
            else
                return false;
        }
        static Boolean IsValidParanthesisExpression(char[] exp)
        {
            Stack<char> st = new Stack<char>();
            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '{' || exp[i] == '(' || exp[i] == '[')
                    st.Push(exp[i]);
                if (exp[i] == '}' || exp[i] == ')' || exp[i] == ']')
                {
                    if (st.Count == 0)
                    {
                        return false;
                    }

                    else if (!isValidPair(st.Pop(), exp[i]))
                    {
                        return false;
                    }
                }
            }
            if (st.Count == 0)
                return true;
            else
            {
                return false;
            }
        }

    }
}
