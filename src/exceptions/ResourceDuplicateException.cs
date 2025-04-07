namespace backend.Exceptions{
    public class ResourceDuplicateException : Exception{
        public ResourceDuplicateException() : base() { }

        public ResourceDuplicateException(string message) : base(message) { }

        public ResourceDuplicateException(string message, Exception innerException) : base(message, innerException) { }
    }
}