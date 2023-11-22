namespace Wingrid.Collector.Models.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(string message, Exception ix) : base(message, ix)
        {

        }
    }
}