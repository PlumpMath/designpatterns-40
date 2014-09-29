using System;
using System.IO;

namespace DP_Tokenizer
{
    public class TokenTest
    {
        public static TokenList<Token> GetTestTokens()
        {
            var definitions = Grammar.GetDefinitions();
            var partners = Grammar.GetPartners();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            //writer.Write("show( x );");

            writer.Write("x = 4; if ( x == 0 ) show( x + 3 ); else show( x - 2 );");

            writer.Flush();
            stream.Position = 0;
            
            TextReader reader = new StreamReader(stream);

            var tokenizer = new Tokenizer(reader, definitions, partners);
            tokenizer.Tokenize();

            return tokenizer.GetTokenList();
        }
    }
}
