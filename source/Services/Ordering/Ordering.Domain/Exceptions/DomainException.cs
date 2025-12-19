namespace Ordering.Domain.Exceptions
{
    class DomainException : Exception
    {
        public DomainException(string message) 
            : base($"Domain exception: \"{message}\" throw from Domain exception")
        {
        }
    }
}
