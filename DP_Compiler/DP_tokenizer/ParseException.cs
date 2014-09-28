using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    class ParserException : Exception, ISerializable
    {
        public ParserException()
        {
            // Add implementation.
        }
        public ParserException(string message)
            : base(message)
        {
            // Add implementation.
        }
        public ParserException(string message, Exception inner)
            : base(message, inner)
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected ParserException(SerializationInfo info, StreamingContext context)
        {
            // Add implementation.
        }
    }
}
