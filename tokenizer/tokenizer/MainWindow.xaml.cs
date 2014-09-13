using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tokenizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitTokenizer();
        }

        // Regular expressions to match the code
        public void InitTokenizer()
        {
            Tokenizer tokenizer = new Tokenizer();
            tokenizer.Add("sin|cos|exp|ln|sqrt", 1); // function
            tokenizer.Add("\\(", 2); // open bracket
            tokenizer.Add("\\)", 3); // close bracket
            tokenizer.Add("[+-]", 4); // plus or minus
            tokenizer.Add("[*/]", 5); // mult or divide
            tokenizer.Add("\\^", 6); // raised
            tokenizer.Add("[0-9]+", 7); // integer number
            tokenizer.Add("[a-zA-Z][a-zA-Z0-9_]*", 8); // variable
            tokenizer.Add("\\{", 10); // open curly bracket
            tokenizer.Add("\\}", 11); // close curly bracket
            tokenizer.Add("\\[", 12); // open blokhaak
            tokenizer.Add("\\]", 13); // close blokhaak
            tokenizer.Add("\\<", 14); // lt
            tokenizer.Add("\\>", 15); // gt
            tokenizer.Add("\\=", 16); // equals

            try
            {
                tokenizer.Tokenize("if(x < 4) { x = 8 }");

                foreach (Token tok in tokenizer.GetTokenList())
                {
                    Console.WriteLine(tok.token + " " + tok.text + " " + tok.lineNumber + " " + tok.linePosition + " " + tok.level);
                }
            }
            catch (ParserException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
