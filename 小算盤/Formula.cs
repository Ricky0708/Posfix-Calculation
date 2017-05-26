using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 小算盤
{
    public static class Formula
    {
        private static Dictionary<char, Priority> OperatorMap = new Dictionary<char, Priority>()
        {
            { '(', new Priority(){ OP = "(", ISP = 0,  ICP=20 } },
            { ')', new Priority(){ OP = ")", ISP = 19, ICP=19 } },
            { '+', new Priority(){ OP = "+", ISP = 12, ICP=12 } },
            { '-', new Priority(){ OP = "-", ISP = 12, ICP=12 } },
            { '*', new Priority(){ OP = "*", ISP = 13, ICP=13 } },
            { '/', new Priority(){ OP = "/", ISP = 13, ICP=13 } },
            { '%', new Priority(){ OP = "%", ISP = 13, ICP=13 } },
            { '\\', new Priority(){ OP = "\\", ISP = 13, ICP=13 } },
            { '^', new Priority(){ OP = "^", ISP = 13, ICP=13 } },
        };

        public static double Calculate(string formula)
        {
            double result = 0;
            var stack = new Stack<double>();
            var posfix = ConvertToPosfix(formula);
            while (posfix.Count > 0)
            {
                string op = posfix.Dequeue();
                if (double.TryParse(op, out double operand))
                {
                    stack.Push(operand);
                }
                else
                {
                    double rightNum = stack.Pop();
                    double leftNum = stack.Pop();
                    switch (op)
                    {
                        case "+":
                            stack.Push(leftNum + rightNum);
                            break;
                        case "-":
                            stack.Push(leftNum - rightNum);
                            break;
                        case "*":
                            stack.Push(leftNum * rightNum);
                            break;
                        case "/":
                            stack.Push(leftNum / rightNum);
                            break;
                        case "%":
                            stack.Push(leftNum % rightNum);
                            break;
                        case "\\":
                            stack.Push(Math.Floor(leftNum / rightNum));
                            break;
                        case "^":
                            stack.Push(Math.Pow(leftNum, rightNum));
                            break;

                        default:
                            break;
                    }
                }
            }
            result = stack.Pop();
            return result;
        }

        private static Queue<string> ConvertToPosfix(string infix)
        {
            var charAry = infix.Replace(" ", "").ToCharArray();
            StringBuilder sbTemp = new StringBuilder();
            char @char;
            Queue<string> posfix = new Queue<string>();
            Stack<Priority> stack = new Stack<Priority>();

            int position = 0;
            int length = charAry.Count();
            while (position < length)
            {
                @char = charAry[position];
                //如果是數字，先搜集
                if (Char.IsDigit(@char) || @char == '.')
                {
                    sbTemp.Append(@char);
                }
                else
                {
                    var @operator = OperatorMap[@char];
                    //暫存內不是空值，且遇到 (，表示沒有輸入 乘號，先處理 乘號再處理 (
                    if (sbTemp.Length != 0 && @operator.OP == "(")
                    {
                        ProcessOperator(OperatorMap['*'], stack, posfix);
                    }
                    if (sbTemp.Length != 0)
                    {
                        //先將暫存數字放入佇列
                        posfix.Enqueue(sbTemp.ToString());
                    }
                    ProcessOperator(@operator, stack, posfix);
                    sbTemp.Clear();
                }
                position += 1;
            }
            if (sbTemp.Length != 0)
            {
                posfix.Enqueue(sbTemp.ToString());
            }
            while (stack.Count != 0)
            {
                posfix.Enqueue(stack.Pop().OP);
            }
            return posfix;
        }

        private static void ProcessOperator(Priority @operator, Stack<Priority> stack, Queue<string> posfix)
        {
            while (true)
            {
                if (@operator.OP == ")")
                {
                    var op = stack.Pop().OP;
                    if (op == "(")
                    {
                        break;
                    }
                    else
                    {
                        posfix.Enqueue(op);
                    }
                }
                else if (stack.Count == 0 || @operator.ICP > stack.Peek().ISP)
                {
                    stack.Push(@operator);
                    break;
                }
                else if (@operator.ICP <= stack.Peek().ISP)
                {
                    var op = stack.Pop().OP;
                    posfix.Enqueue(op);
                }
                else
                {
                    break;
                }
            }
        }

        private class Priority
        {
            public string OP { get; set; }
            public int ISP { get; set; } //In-Stack
            public int ICP { get; set; } // In-Coming
        }
    }
}
