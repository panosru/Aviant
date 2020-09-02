namespace Aviant.DDD.Domain.Exceptions
{
    using System;

    [SerializableAttribute]
    public class DomainException : Exception
    {
        public DomainException()
        { }

        public DomainException(string message)
            : base(message, null)
        { }

        public DomainException(string message, Exception inner)
            : base(message, inner)
        { }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="familyCode">{ familyCode ∈ R ∣ familyCode >= 0 }</param>
        /// <param name="inner"></param>
        public DomainException(
            string     message,
            int        errorCode,
            int?       familyCode = null,
            Exception? inner      = null)
            : base(message, inner)
        {
            SetHResult(errorCode, familyCode);
        }

        public int ErrorCode => Getcode("ErrorCode");

        public int FamilyCode => Getcode("FamilyCode");

        public DateTime Occurred { get; } = DateTime.UtcNow;

        public Guid ExceptionId { get; } = Guid.NewGuid();

        public string ExceptionName => GetType().FullName!;

        private void SetHResult(int errorCode, int? familyCode)
        {
            if (0 > familyCode)
                throw new Exception(
                    $@"FamilyCode must be a positive number {{ familyCode ∈ R ∣ familyCode >= 0 }}. 
                               '{familyCode}' received.");

            Data.Add("ErrorCode", errorCode);

            if (!(familyCode is null))
                Data.Add("FamilyCode", familyCode);

            HResult = errorCode + familyCode ?? -1;
        }

        private int Getcode(string key)
        {
            if (!Data.Contains(key))
                return -1;

            var errorCode = Data[key];

            if (errorCode is null)
                return -1;

            return (int) errorCode;
        }
    }
}