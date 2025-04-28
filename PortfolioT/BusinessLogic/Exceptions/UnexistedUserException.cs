namespace PortfolioT.BusinessLogic.Exceptions
{
    public class UnexistedUserException : Exception
    {
        public UnexistedUserException(string message)
        : base(message) { }
    }
}
