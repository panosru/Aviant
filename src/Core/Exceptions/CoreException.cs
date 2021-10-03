namespace Aviant.DDD.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Timing;

    [Serializable]
    public class CoreException : Exception
    {
        public CoreException()
        { }

        public CoreException(string message)
            : base(message, null)
        { }

        public CoreException(string message, Exception inner)
            : base(message, inner)
        { }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="familyCode">{ familyCode ∈ R ∣ familyCode &gt;= 0 }</param>
        /// <param name="inner"></param>
        public CoreException(
            string     message,
            int        errorCode,
            int?       familyCode = null,
            Exception? inner      = null)
            : base(message, inner) => SetHResult(errorCode, familyCode);

        protected CoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorCode     = (int)info.GetValue(nameof(ErrorCode),        typeof(int))!;
            FamilyCode    = (int)info.GetValue(nameof(FamilyCode),       typeof(int))!;
            Occurred      = (DateTime)info.GetValue(nameof(Occurred),    typeof(DateTime))!;
            ExceptionId   = (Guid)info.GetValue(nameof(ExceptionId),     typeof(Guid))!;
            ExceptionName = (string)info.GetValue(nameof(ExceptionName), typeof(string))!;
        }

        public int ErrorCode
        {
            get => GetCode("ErrorCode");

            protected set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public int FamilyCode
        {
            get => GetCode("FamilyCode");

            protected set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public DateTime Occurred { get; protected set; } = Clock.Now;

        public Guid ExceptionId { get; protected set; } = Guid.NewGuid();

        public string ExceptionName
        {
            get => GetType().FullName!;

            protected set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
            }
        }

        private void SetHResult(int errorCode, int? familyCode)
        {
            if (0 > familyCode)
                throw new CoreException(
                    $@"FamilyCode must be a positive number {{ {nameof(familyCode)} ∈ R ∣ {nameof(familyCode)} >= 0 }}. 
                               '{familyCode}' received.");

            Data.Add("ErrorCode", errorCode);

            if (familyCode is not null)
                Data.Add("FamilyCode", familyCode);

            HResult = errorCode + familyCode ?? -1;
        }

        private int GetCode(string key)
        {
            if (!Data.Contains(key))
                return -1;

            var errorCode = Data[key];

            if (errorCode is null)
                return -1;

            return (int)errorCode;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ErrorCode),     ErrorCode,     typeof(int));
            info.AddValue(nameof(FamilyCode),    FamilyCode,    typeof(int));
            info.AddValue(nameof(Occurred),      Occurred,      typeof(DateTime));
            info.AddValue(nameof(ExceptionId),   ExceptionId,   typeof(Guid));
            info.AddValue(nameof(ExceptionName), ExceptionName, typeof(string));
            base.GetObjectData(info, context);
        }
    }
}
