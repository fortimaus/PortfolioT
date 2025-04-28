namespace PortfolioT.BusinessLogic.Exceptions
{
    public class BusyUserException : Exception
    {
        public BusyUserException(string message)
        : base(message) { }
    }
}
