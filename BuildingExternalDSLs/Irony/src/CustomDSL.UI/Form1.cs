using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomDsl;
using CustomDsl.Ast;
using CustomDsl.ExpressionBuilding;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace CustomDSL.UI
{
    public partial class Form1 : Form
    {
        private Parser _parser;

        public Form1()
        {
            InitializeComponent();

            var grammar = new CustomDslGrammar();
            _parser = new Parser(grammar);
        }

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            ParseTree tree = _parser.Parse(txtExpression.Text);

            if (tree.HasErrors())
            {
                MessageBox.Show(tree.ParserMessages.First().Message, "Error parsing message", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }


            var rootNode = (AstNode)tree.Root.AstNode;
            var visitor = new CSharpExpressionBuilder();
            Func<object> func = visitor.Build(rootNode);

            txtResult.Text = func().ToString();
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            lblResult.BackColor = Color.Transparent;
            lblResult.Text = string.Empty;
        }

        private void btnEvalRule_Click(object sender, EventArgs e)
        {
            var person = new Person
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Age = (int) nudAge.Value
            };

            var tree = _parser.Parse(txtRule.Text);

            if (tree.HasErrors())
            {
                MessageBox.Show(tree.ParserMessages.First().Message, "Error parsing message", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            var rootNode = (AstNode)tree.Root.AstNode;
            var visitor = new CSharpExpressionBuilder();
            var func = visitor.Build<Person>(rootNode);

            var result = func(person);

            if (result is bool)
            {
                var evalResult = (bool) result;

                if (evalResult)
                {
                    lblResult.Text = "Match!";
                    lblResult.BackColor = Color.LawnGreen;
                }
                else
                {
                    lblResult.Text = "Not a match!";
                    lblResult.BackColor = Color.Red;
                }
            }
        }
    }
}
