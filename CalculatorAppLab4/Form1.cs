using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorAppLab4
{
    public partial class Form1 : Form
    {
        private double resultValue = 0;
        private string operationPerformed = "";
        private bool isOperationPerformed = false;
        private string expression = "";
        private bool justEvaluated = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            btn_0.Click += NumberButton_Click;
            btn_1.Click += NumberButton_Click;
            btn_2.Click += NumberButton_Click;
            btn_3.Click += NumberButton_Click;
            btn_4.Click += NumberButton_Click;
            btn_5.Click += NumberButton_Click;
            btn_6.Click += NumberButton_Click;
            btn_7.Click += NumberButton_Click;
            btn_8.Click += NumberButton_Click;
            btn_9.Click += NumberButton_Click;
            btn_point.Click += NumberButton_Click;

            btn_add.Click += Operator_Click;
            btn_subtract.Click += Operator_Click;
            btn_multiply.Click += Operator_Click;
            btn_divide.Click += Operator_Click;

            btn_equals.Click += Btn_equal_Click;
            btn_c.Click += Btn_c_Click;
            btn_ce.Click += Btn_ce_Click;
            btn_del.Click += Btn_del_Click;
            btn_sign.Click += Btn_sign_Click;
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (textBox1.Text == "0" || isOperationPerformed || justEvaluated)
            {
                textBox1.Clear();

                if (justEvaluated)
                {
                    expression = "";
                    label1.Text = "";
                }
            }

            isOperationPerformed = false;
            justEvaluated = false;

            textBox1.Text += button.Text;
            expression += button.Text;
            UpdateExpressionDisplay();
        }




        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string op = button.Text;

            if (justEvaluated)
            {
                expression = textBox1.Text + op;
                justEvaluated = false;
            }
            else
            {
                expression += op;
            }

            textBox1.Clear();
            isOperationPerformed = true;
            UpdateExpressionDisplay();
        }


        private void Btn_equal_Click(object sender, EventArgs e)
        {
            try
            {
                var resultObj = new System.Data.DataTable().Compute(expression, null);
                double result = Convert.ToDouble(resultObj);

                if (double.IsInfinity(result) || double.IsNaN(result) || Math.Abs(result) > 1e+308)
                {
                    textBox1.Text = "MATH ERROR";
                    label1.Text = expression + " =";
                    expression = "";
                    justEvaluated = true;
                    return;
                }

                textBox1.Text = result.ToString();
                label1.Text = expression + " =";
                expression = result.ToString();
                justEvaluated = true;
            }
            catch
            {
                MessageBox.Show("Invalid Expression");
                textBox1.Text = "0";
                label1.Text = "";
                expression = "";
                justEvaluated = false;
            }
        }



        private void Btn_c_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            label1.Text = "";
            expression = "";
        }

        private void Btn_ce_Click(object sender, EventArgs e)
        {
            int lengthToRemove = textBox1.Text.Length;

            if (expression.Length >= lengthToRemove)
            {
                expression = expression.Substring(0, expression.Length - lengthToRemove);
            }
            else
            {
                expression = "";
            }

            textBox1.Text = "0";
            UpdateExpressionDisplay();
        }



        private void Btn_del_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "0" && textBox1.Text.Length > 0)
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);

                if (expression.Length > 0)
                    expression = expression.Remove(expression.Length - 1);

                UpdateExpressionDisplay();
            }

            if (string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = "0";

            UpdateExpressionDisplay();
        }



        private void Btn_sign_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text == "0")
                return;

            if (double.TryParse(textBox1.Text, out double value))
            {
                value = -value;
                string newValue = value.ToString();

                int i = expression.Length - 1;
                while (i >= 0 && (char.IsDigit(expression[i]) || expression[i] == '.' || expression[i] == '-'))
                    i--;

                string before = expression.Substring(0, i + 1);
                string updatedExpression = before + newValue;

                expression = updatedExpression;
                textBox1.Text = newValue;
                UpdateExpressionDisplay();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UpdateExpressionDisplay()
        {
            label1.Text = expression;

            int textWidth = TextRenderer.MeasureText(label1.Text, label1.Font).Width;

            if (textWidth > panel1.Width)
            {
                label1.Left = panel1.Width - textWidth;
            }
            else
            {
                label1.Left = 0;
            }
        }
    }
}