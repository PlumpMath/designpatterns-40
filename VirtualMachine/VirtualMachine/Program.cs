using Compiler;
using DP_Tokenizer;

namespace VirtualMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokens = TokenTest.GetTestTokens();
            var compiler = new CCompiler(tokens);
            compiler.Compile();
            var compilerTokens = compiler.GetCompilerTokens();
            var symbolTable = compiler.GetSymbolTable();

            var virtualMachine = new VVirtualMachine(compilerTokens, symbolTable);
            virtualMachine.RunCode();
        }
    }
}
