using System;

namespace AillieoUtils.EasyLogger
{
    public class DefaultFormatter : IFormatter
    {
        public string Format(object message)
        {
            return Convert.ToString(message);
        }
    }
}
