using System;
using System.Collections.Generic;
using System.IO;
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

namespace DP_Tokenizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set definitions and text file
            List<TokenDefinition> definitions = Grammar.GetDefinitions();
            TextReader reader = new StreamReader(@"C:\Users\Stefan\Documents\test.txt");

            Tokenizer tokenizer = new Tokenizer(reader, definitions);
            tokenizer.Tokenize();

            itemGrid.ItemsSource = tokenizer.GetTokenList();
        }
    }
}
