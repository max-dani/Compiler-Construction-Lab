using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class SemanticHelper
    {
        public static int GetOpWeight(string op)
        {
            int w = 0;
            switch(op)
            {
                case "+":
                case "-":
                    w = 1;
                    break;
                case "*":
                case "/":
                    w = 2;
                    break;
                
            }
            return w;
        }
        public static bool HasHigherPre(string op1, string op2)
        {
            int wOp1 = GetOpWeight(op1);
            int wOp2 = GetOpWeight(op2);
            if (wOp1 >= wOp2)
                return true;
            else
                return false;
        }

        public static int ApplyOperator(int op1, int op2, string oper)
        {
            int res = 0;
            switch(oper)
            {
                case "+":
                    res = op1 + op2;
                    break;
                case "-":
                    res = op1 - op2;
                    break;
                case "*":
                    res = op1 * op2;
                    break;
                case "/":
                    res = op1 / op2;
                    break;
            }
            return res;
        }

        public static List<string> InfixToPostfix(List<string> infix)
        {
            List<string> result = new List<string>();
            Stack<string> stk = new Stack<string>();
            
            for(int i = 0; i < infix.Count; i++)
            {
                if (infix[i] == "+" || infix[i] == "-" || infix[i] == "*" || infix[i] == "/")
                {
                    while (stk.Count > 0 && stk.Peek() != "(" && HasHigherPre(stk.Peek(), infix[i]))
                    {
                        result.Add(stk.Peek());
                        stk.Pop();
                    }
                    stk.Push(infix[i]);
                }
                else if (infix[i] == "(")
                    stk.Push(infix[i]);
                else if(infix[i] == ")")
                {
                    while (stk.Count > 0 && stk.Peek() != "(")
                    {
                        result.Add(stk.Peek());
                        stk.Pop();
                    }    
                    stk.Pop();
                }
                else 
                    result.Add(infix[i]);
            }
            while(stk.Count > 0)
            {
                result.Add(stk.Peek());
                stk.Pop();
            }

            return result;
        }

        public static int EvaluateExp(List<string> postfixExp)
        {
            Stack<int> stk = new Stack<int>();
            
            for(int i = 0; i < postfixExp.Count; i++)
            {
                if (postfixExp[i] == "+" || postfixExp[i] == "-" || postfixExp[i] == "*" || postfixExp[i] == "/")
                {
                    int op2 = stk.Pop();
                    int op1 = stk.Pop();
                    int res = ApplyOperator(op1, op2, postfixExp[i]);
                    stk.Push(res);
                }
                else
                {
                    int op1 = Int32.Parse(postfixExp[i]);
                    stk.Push(op1);
                }
            }
            int ret = stk.Pop();
            return ret;
        }

        public static float ApplyFloatOperator(float op1, float op2, string oper)
        {
            float res = 0;
            switch (oper)
            {
                case "+":
                    res = op1 + op2;
                    break;
                case "-":
                    res = op1 - op2;
                    break;
                case "*":
                    res = op1 * op2;
                    break;
                case "/":
                    res = op1 / op2;
                    break;
            }
            return res;
        }

        public static float EvaluateFloatExp(List<string> postfixExp)
        {
            Stack<float> stk = new Stack<float>();

            for (int i = 0; i < postfixExp.Count; i++)
            {
                if (postfixExp[i] == "+" || postfixExp[i] == "-" || postfixExp[i] == "*" || postfixExp[i] == "/")
                {
                    float op2 = stk.Pop();
                    float op1 = stk.Pop();
                    float res = ApplyFloatOperator(op1, op2, postfixExp[i]);
                    stk.Push(res);
                }
                else
                {
                    float op1 = float.Parse(postfixExp[i]);
                    stk.Push(op1);
                }
            }
            float ret = stk.Pop();
            return ret;
        }

    }
}
