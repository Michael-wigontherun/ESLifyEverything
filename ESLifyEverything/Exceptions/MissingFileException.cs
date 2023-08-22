using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverything.Exceptions
{
    internal class MissingFileException : Exception
    {
        public MissingFileException() { }

        public MissingFileException(string? message) : base(message) { }

        public MissingFileException(string? message, Exception? innerException) : base(message, innerException) { }

        protected MissingFileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
