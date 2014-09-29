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
            //writer.Write("x = 3 + 2 * 4;");
            writer.Write("while(x < 2) { x = x + 1 }");
            writer.Flush();
            stream.Position = 0;
            
            TextReader reader = new StreamReader(stream);

            var tokenizer = new Tokenizer(reader, definitions, partners);
            tokenizer.Tokenize();

            return tokenizer.GetTokenList();
        }
    }
}
