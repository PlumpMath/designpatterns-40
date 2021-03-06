﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            List<TokenDefinition> definitions   = Grammar.GetDefinitions();
            List<TokenPartner> partners         = Grammar.GetPartners();

            //String documents = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test.txt");
            String documents = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.txt");
            TextReader reader = new StreamReader(documents);

            Tokenizer tokenizer = new Tokenizer(reader, definitions, partners);
            tokenizer.Tokenize();

            itemGrid.ItemsSource = tokenizer.GetTokenList();

            // Send all tokens from tokenlist to parser
            ParseExpression parseExpression = new ParseExpression(tokenizer.GetTokenList());
            
        }
    }
}
