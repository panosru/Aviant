namespace Aviant.DDD.Core.Linq.Expressions
{
    using System;
    using System.Linq.Expressions;

    internal static class ExpressionCombiner
    {
        public static Expression<Func<T, bool>>? Combine<T>(
            Expression<Func<T, bool>>? expression1,
            Expression<Func<T, bool>>? expression2)
        {
            switch (expression1)
            {
                case null when expression2 == null:
                    return null;

                case null:
                    return expression2;
            }

            if (expression2 == null)
                return expression1;

            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
            var left        = leftVisitor.Visit(expression1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
            var right        = rightVisitor.Visit(expression2.Body);

            if (left is null
             || right is null)
                throw new NullReferenceException(
                    $"Expression {nameof(left)} or {nameof(right)} is null");

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        #region Nested type: ReplaceExpressionVisitor

        private sealed class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _newValue;

            private readonly Expression _oldValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression? Visit(Expression? node) => node == _oldValue
                ? _newValue
                : base.Visit(node);
        }

        #endregion
    }
}