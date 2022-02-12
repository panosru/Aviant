namespace Aviant.Foundation.Core.Validators;

using Messages;

public static class AssertionsConcernValidator
{
    public static bool HasNotifications() => MessagesFacade.HasMessages();

    public static bool IsSatisfiedBy(params Func<bool>[] asserts)
    {
        var isSatisfied = true;

        foreach (Func<bool> unused in asserts.Where(assert => !assert()))
            isSatisfied = false;

        return isSatisfied;
    }

    public static Func<bool> IsEqual(
        string left,
        string right,
        string notification)
    {
        return delegate
        {
            if (left == right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsEqual(
        int    left,
        int    right,
        string notification)
    {
        return delegate
        {
            if (left == right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsDateEqual(
        DateTime left,
        DateTime right,
        string   notification)
    {
        return delegate
        {
            if (left.Date != right.Date)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsLowerThan(
        int?   left,
        int    right,
        string notification)
    {
        return delegate
        {
            if (left > right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsLowerThan(
        decimal? left,
        decimal  right,
        string   notification)
    {
        return delegate
        {
            if (left > right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsLowerThan(
        DateTime? left,
        DateTime  right,
        string    notification)
    {
        return delegate
        {
            if (left > right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsLowerThanOrEqual(
        int?   left,
        int    right,
        string notification)
    {
        return delegate
        {
            if (left >= right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsLowerThanOrEqual(
        decimal? left,
        decimal  right,
        string   notification)
    {
        return delegate
        {
            if (left >= right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsLowerThanOrEqual(
        DateTime? left,
        DateTime  right,
        string    notification)
    {
        return delegate
        {
            if (left >= right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsGreaterThan(
        int?   left,
        int    right,
        string notification)
    {
        return delegate
        {
            if (left > right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsGreaterThan(
        decimal? left,
        decimal  right,
        string   notification)
    {
        return delegate
        {
            if (left > right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsGreaterThan(
        DateTime? left,
        DateTime  right,
        string    notification)
    {
        return delegate
        {
            if (left > right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsGreaterThanOrEqual(
        int?   left,
        int    right,
        string notification)
    {
        return delegate
        {
            if (left <= right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsGreaterThanOrEqual(
        decimal? left,
        decimal  right,
        string   notification)
    {
        return delegate
        {
            if (left <= right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsGreaterThanOrEqual(
        DateTime? left,
        DateTime  right,
        string    notification)
    {
        return delegate
        {
            if (left <= right)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsStringNotNullOrWhiteSpace(string value, string notification)
    {
        return delegate
        {
            if (!string.IsNullOrWhiteSpace(value))
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> HasMinimumLength(
        string value,
        int    minLength,
        string notification)
    {
        return delegate
        {
            if (value.Length > minLength)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> HasLengthEqual(
        string value,
        int    length,
        string notification)
    {
        return delegate
        {
            if (value.Length == length)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsGuidNotEmpty(Guid guid, string notification)
    {
        return delegate
        {
            if (Guid.Empty != guid)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsGuidNotNull(Guid? guid, string notification)
    {
        return delegate
        {
            if (guid is not null)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> ValidateGuidFromString(
        string guid,
        string stringIsEmptyMessage,
        string guidIsEmptyMessage,
        string guidIsInvalidMessage)
    {
        return delegate
        {
            var isValid = IsSatisfiedBy(
                IsStringNotNullOrWhiteSpace(
                    guid,
                    stringIsEmptyMessage));

            if (!isValid)
                return false;

            if (!Guid.TryParse(guid, out var parsed))
                return false;

            isValid = IsSatisfiedBy(
                IsGuidNotNull(parsed, guidIsInvalidMessage),
                IsGuidNotEmpty(parsed, guidIsEmptyMessage));

            return isValid;
        };
    }

    public static Func<bool> IsNotNull(object? obj, string notification)
    {
        return delegate
        {
            if (obj is not null)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }

    public static Func<bool> IsNull(object? obj, string notification)
    {
        return delegate
        {
            if (obj is null)
                return true;

            MessagesFacade.AddMessage(notification);

            return false;
        };
    }
}
