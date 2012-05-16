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
            var tree = _parser.Parse(txtExpression.Text);
            var rootNode = (AstNode)tree.Root.AstNode;
            var visitor = new CustomDslCSharpExpressionVisitor();
            visitor.Visit(rootNode);

            var expression = visitor.GetCShaprExpression();

            var compiled = expression.Compile();

            txtResult.Text = compiled().ToString();
        }
    }
}
