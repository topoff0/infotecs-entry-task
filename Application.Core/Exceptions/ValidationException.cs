namespace Application.Core.Exceptions
{
    public class CustomValidationException : Exception
    {
        public CustomValidationException(string message)
            : base(message)
        {

        }

        public CustomValidationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}