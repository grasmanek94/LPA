using System;
using System.Runtime.Serialization;

namespace PressureControl
{
    public class NoDeviceFoundException : Exception
    {
        public NoDeviceFoundException()
        { }

        public NoDeviceFoundException(string message)
        : base(message)
        { }

        public NoDeviceFoundException(string message, Exception innerException)
        : base(message, innerException)
        { }

        public NoDeviceFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        { }
    }
}
