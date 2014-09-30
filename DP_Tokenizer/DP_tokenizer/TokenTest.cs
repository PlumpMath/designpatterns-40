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

            writer.Write("y = 4; x = 3 + 6 * y; while ( x != 0 || y < 3 ){ x = x - 1; y = y - 1; } show( x + \" , \"number\" \" + y );");

            writer.Flush();
            stream.Position = 0;
            
            TextReader reader = new StreamReader(stream);

            var tokenizer = new Tokenizer(reader, definitions, partners);
            tokenizer.Tokenize();

            return tokenizer.GetTokenList();
        }
    }
}
