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

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = @"(10+98*5+(99-100)+7)/2 ^ 5 % 99 \ 2";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var formula = textBox1.Text;
            int aa = 1000000;
            var check = FormulaUseQueue.Calculate(formula).ToString();

            for (int i = 0; i < 4; i++)
            {
                System.Threading.Thread th = new System.Threading.Thread(p =>
                {
                    while (aa > 0)
                    {
                        var n = Formula.Calculate(formula).ToString();
                        if (n != check)
                        {
                            MessageBox.Show("QQ");
                        }
                        aa -= 1;
                        if (aa == 0)
                        {
                            MessageBox.Show(n.ToString());
                        }
                    }
                });
                th.Start();
            }

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