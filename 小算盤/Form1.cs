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
        Dictionary<string, Priority> OperatorMap = new Dictionary<string, Priority>()
        {
            { "(", new Priority(){ OP = "(", ISP = 0,  ICP=20 } },
            { ")", new Priority(){ OP = ")", ISP = 19, ICP=19 } },
            { "+", new Priority(){ OP = "+", ISP = 12, ICP=12 } },
            { "-", new Priority(){ OP = "-", ISP = 12, ICP=12 } },
            { "*", new Priority(){ OP = "*", ISP = 13, ICP=13 } },
            { "/", new Priority(){ OP = "/", ISP = 13, ICP=13 } },
            { "%", new Priority(){ OP = "%", ISP = 13, ICP=13 } },
            { "\\", new Priority(){ OP = "\\", ISP = 13, ICP=13 } },
            { "^", new Priority(){ OP = "^", ISP = 13, ICP=13 } },
        };
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = @"(10+98*5+(99-100)+7)/2";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var infix = ConverToFormulaQue(textBox1.Text);
            var posfix = ConvertToPosfix(infix);
            var stack = new Stack<double>();
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
            MessageBox.Show(stack.Pop().ToString());
        }

        private Queue<string> ConvertToPosfix(Queue<string> infix)
        {
            var postfix = new Queue<string>();
            var stack = new Stack<Priority>();
            while (infix.Count > 0)
            {
                var item = infix.Dequeue();
                if (double.TryParse(item, out double result))
                {
                    postfix.Enqueue(item);
                }
                else
                {
                    var @operator = OperatorMap[item as string];
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
                                postfix.Enqueue(op);
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
                            postfix.Enqueue(op);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            while (stack.Count > 0)
            {
                var op = stack.Pop().OP;
                if (!(op == "(" || op == ")"))
                {
                    postfix.Enqueue(op);
                }
            }
            return postfix;
        }

        private Queue<string> ConverToFormulaQue(string formula)
        {
            Queue<string> queue = new Queue<string>();
            formula = textBox1.Text.Replace(" ", "");
            int formulaPosition = 0;
            StringBuilder tempOP = new StringBuilder();
            while (formulaPosition < formula.Length)
            {
                if (!double.TryParse(formula.Substring(formulaPosition, 1), out var op))
                {
                    switch (formula.Substring(formulaPosition, 1))
                    {
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                        case "%":
                        case "\\":
                        case "^":
                        case ")":
                            if (!string.IsNullOrEmpty(tempOP.ToString()))
                            {
                                queue.Enqueue(tempOP.ToString());
                            }
                            queue.Enqueue(formula.Substring(formulaPosition, 1));
                            tempOP.Clear();
                            break;
                        case "(":
                            if (double.TryParse(tempOP.ToString(), out var check))
                            {
                                queue.Enqueue(tempOP.ToString());
                                queue.Enqueue("*");
                                queue.Enqueue(formula.Substring(formulaPosition, 1));
                            }
                            else
                            {
                                queue.Enqueue(formula.Substring(formulaPosition, 1));
                            }
                            tempOP.Clear();
                            break;
                        case ".":
                            tempOP.Append(formula.Substring(formulaPosition, 1));
                            break;
                    }
                }
                else
                {
                    tempOP.Append(formula.Substring(formulaPosition, 1));
                }
                formulaPosition += 1;
            }
            if (tempOP.Length != 0)
            {
                queue.Enqueue(tempOP.ToString());
            }
            return queue;
        }

        public class Priority
        {
            public string OP { get; set; }
            public int ISP { get; set; } //In-Stack
            public int ICP { get; set; } // In-Coming
        }
    }
}
