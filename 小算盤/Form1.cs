using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 小算盤
{
    public partial class Form1 : Form
    {
        Dictionary<char, Priority> OperatorMap = new Dictionary<char, Priority>()
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
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = @"(10+98*5+(99-100)+7)/2 ^ 5 % 99 \ 2";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string result = "";
            var stack = new Stack<double>();
            var posfix = ConvertToPosfix(textBox1.Text);
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
            result = stack.Pop().ToString();

            MessageBox.Show(result);
        }

        private Queue<string> ConvertToPosfix(string infix)
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
                        Process(OperatorMap['*'], stack, posfix);
                    }
                    if (sbTemp.Length != 0)
                    {
                        //先將暫存數字放入佇列
                        posfix.Enqueue(sbTemp.ToString());
                    }
                    Process(@operator, stack, posfix);
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

        private void Process(Priority @operator, Stack<Priority> stack, Queue<string> posfix)
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

        public class Priority
        {
            public string OP { get; set; }
            public int ISP { get; set; } //In-Stack
            public int ICP { get; set; } // In-Coming
        }


    }
}
//private void button2_Click(object sender, EventArgs e)
//{


//    var infix = ConverToFormulaQue(textBox1.Text);
//    var a = ConvertToPosfix(infix);
//    var b = ConvertToPosfixB(textBox1.Text);
//    label1.Text = "";
//    label2.Text = "";
//    while (a.Count() != 0)
//    {
//        label1.Text += a.Dequeue();
//    }
//    while (b.Count() != 0)
//    {
//        label2.Text += b.Dequeue();
//    }

//}

//private Queue<string> ConvertToPosfix(Queue<string> infix)
//{
//    var postfix = new Queue<string>();
//    var stack = new Stack<Priority>();
//    while (infix.Count > 0)
//    {
//        var item = infix.Dequeue();
//        if (double.TryParse(item, out double result))
//        {
//            postfix.Enqueue(item);
//        }
//        else
//        {
//            var @operator = OperatorMap[item.ToCharArray()[0]];
//            while (true)
//            {
//                if (@operator.OP == ")")
//                {
//                    var op = stack.Pop().OP;
//                    if (op == "(")
//                    {
//                        break;
//                    }
//                    else
//                    {
//                        postfix.Enqueue(op);
//                    }
//                }
//                else if (stack.Count == 0 || @operator.ICP > stack.Peek().ISP)
//                {
//                    stack.Push(@operator);
//                    break;
//                }
//                else if (@operator.ICP <= stack.Peek().ISP)
//                {
//                    var op = stack.Pop().OP;
//                    postfix.Enqueue(op);
//                }
//                else
//                {
//                    break;
//                }
//            }
//        }
//    }
//    while (stack.Count > 0)
//    {
//        var op = stack.Pop().OP;
//        if (!(op == "(" || op == ")"))
//        {
//            postfix.Enqueue(op);
//        }
//    }
//    return postfix;
//}

//private Queue<string> ConverToFormulaQue(string formula)
//{
//    Queue<string> queue = new Queue<string>();
//    formula = textBox1.Text.Replace(" ", "");
//    int formulaPosition = 0;
//    StringBuilder tempOP = new StringBuilder();
//    while (formulaPosition < formula.Length)
//    {
//        if (!double.TryParse(formula.Substring(formulaPosition, 1), out var op))
//        {
//            switch (formula.Substring(formulaPosition, 1))
//            {
//                case "+":
//                case "-":
//                case "*":
//                case "/":
//                case "%":
//                case "\\":
//                case "^":
//                case ")":
//                    if (!string.IsNullOrEmpty(tempOP.ToString()))
//                    {
//                        queue.Enqueue(tempOP.ToString());
//                    }
//                    queue.Enqueue(formula.Substring(formulaPosition, 1));
//                    tempOP.Clear();
//                    break;
//                case "(":
//                    if (double.TryParse(tempOP.ToString(), out var check))
//                    {
//                        queue.Enqueue(tempOP.ToString());
//                        queue.Enqueue("*");
//                        queue.Enqueue(formula.Substring(formulaPosition, 1));
//                    }
//                    else
//                    {
//                        queue.Enqueue(formula.Substring(formulaPosition, 1));
//                    }
//                    tempOP.Clear();
//                    break;
//                case ".":
//                    tempOP.Append(formula.Substring(formulaPosition, 1));
//                    break;
//            }
//        }
//        else
//        {
//            tempOP.Append(formula.Substring(formulaPosition, 1));
//        }
//        formulaPosition += 1;
//    }
//    if (tempOP.Length != 0)
//    {
//        queue.Enqueue(tempOP.ToString());
//    }
//    return queue;
//}