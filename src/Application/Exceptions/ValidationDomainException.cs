namespace Aviant.DDD.Application.Exceptions
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation.Results;

    public class ValidationDomainException : ApplicationDomainException
    {
        public ValidationDomainException()
            :
            base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationDomainException(IEnumerable<ValidationFailure> failures)
            :
            this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures { get; }
    }
}