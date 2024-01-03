using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Aviant.Core.Linq.Expressions;

// The code below is taken from https://github.com/scottksmith95/LINQKit project.

/// <summary> The Predicate Operator </summary>
public enum PredicateOperator
{
    /// <summary> The "Or" </summary>
    Or,

    /// <summary> The "And" </summary>
    And
}

/// <summary>
///     See http://www.albahari.com/expressions for information and examples.
/// </summary>
public static class PredicateBuilder
{
    /// <summary> Start an expression </summary>
    public static ExpressionStarter<T> New<T>() => new();

    /// <summary> Start an expression </summary>
    /// <param name="expression">Expression to be used when starting the builder.</param>
    public static ExpressionStarter<T> New<T>(Expression<Func<T, bool>> expression) =>
        new(expression);

    /// <summary> Create an expression with a stub expression true or false to use when the expression is not yet started. </summary>
    public static ExpressionStarter<T> New<T>(bool defaultExpression) =>
        new(defaultExpression);

    /// <summary>
    ///     Create an expression using an <see cref="IEnumerable{T}" />.
    ///     May be used to instantiate ExpressionStarter objects with anonymous types.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="enumerable">The value is NOT used. Only serves as a way to provide the generic type.</param>
    public static ExpressionStarter<T> New<T>(IEnumerable<T> enumerable)
        => New<T>();

    /// <summary>
    ///     Create an expression using an <see cref="IEnumerable{T}" />.
    ///     May be used to instantiate ExpressionStarter objects with anonymous types.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="enumerable">The value is NOT used. Only serves as a way to provide the generic type.</param>
    /// <param name="expression">Expression to be used when starting the builder.</param>
    public static ExpressionStarter<T> New<T>(IEnumerable<T> enumerable, Expression<Func<T, bool>> expression)
        => New(expression);

    /// <summary>
    ///     Create an expression using an <see cref="IEnumerable{T}" /> with default starting state.
    ///     May be used to instantiate ExpressionStarter objects with anonymous types.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="enumerable">The value is NOT used. Only serves as a way to provide the generic type.</param>
    /// <param name="state">Boolean state used when there is not starting expression yet.</param>
    public static ExpressionStarter<T> New<T>(IEnumerable<T> enumerable, bool state)
        => New<T>(state);

    /// <summary> OR </summary>
    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>>      expr2)
    {
        var expr2Body = new RebindParameterVisitor(expr2.Parameters[0], expr1.Parameters[0]).Visit(expr2.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, expr2Body), expr1.Parameters);
    }

    /// <summary> AND </summary>
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>>      expr2)
    {
        var expr2Body = new RebindParameterVisitor(expr2.Parameters[0], expr1.Parameters[0]).Visit(expr2.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, expr2Body), expr1.Parameters);
    }

    /// <summary> NOT </summary>
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        => Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), expr.Parameters);

    /// <summary>
    ///     Extends the specified source Predicate with another Predicate and the specified PredicateOperator.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="first">The source Predicate.</param>
    /// <param name="second">The second Predicate.</param>
    /// <param name="operator">The Operator (can be "And" or "Or").</param>
    /// <returns>Expression{Func{T, bool}}</returns>
    public static Expression<Func<T, bool>> Extend<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>>      second,
        PredicateOperator              @operator = PredicateOperator.Or) => @operator == PredicateOperator.Or
        ? first.Or(second)
        : first.And(second);

    /// <summary>
    ///     Extends the specified source Predicate with another Predicate and the specified PredicateOperator.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="first">The source Predicate.</param>
    /// <param name="second">The second Predicate.</param>
    /// <param name="operator">The Operator (can be "And" or "Or").</param>
    /// <returns>Expression{Func{T, bool}}</returns>
    public static Expression<Func<T, bool>>? Extend<T>(
        this ExpressionStarter<T> first,
        Expression<Func<T, bool>> second,
        PredicateOperator         @operator = PredicateOperator.Or) => @operator == PredicateOperator.Or
        ? first.Or(second)
        : first.And(second);

    #region Nested type: RebindParameterVisitor

    private sealed class RebindParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _newParameter;

        private readonly ParameterExpression? _oldParameter;

        public RebindParameterVisitor(ParameterExpression? oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node) => node == _oldParameter
            ? _newParameter
            : base.VisitParameter(node);
    }

    #endregion
}

/// <summary>
///     ExpressionStarter{T} which eliminates the default 1=0 or 1=1 stub expressions
/// </summary>
/// <typeparam name="T">The type</typeparam>
public sealed class ExpressionStarter<T>
{
    private Expression<Func<T, bool>>? _predicate;

    internal ExpressionStarter()
        : this(false)
    { }

    internal ExpressionStarter(bool defaultExpression)
    {
        if (defaultExpression)
            DefaultExpression = f => true;
        else
            DefaultExpression = f => false;
    }

    internal ExpressionStarter(Expression<Func<T, bool>> exp)
        : this(false) => _predicate = exp;

    /// <summary>The actual Predicate. It can only be set by calling Start.</summary>
    private Expression<Func<T, bool>>? Predicate => IsStarted || !UseDefaultExpression
        ? _predicate
        : DefaultExpression;

    /// <summary>Determines if the predicate is started.</summary>
    public bool IsStarted => _predicate is not null;

    /// <summary> A default expression to use only when the expression is null </summary>
    public bool UseDefaultExpression => DefaultExpression is not null;

    /// <summary>The default expression</summary>
    public Expression<Func<T, bool>>? DefaultExpression { get; set; }

    /// <summary>Set the Expression predicate</summary>
    /// <param name="exp">The first expression</param>
    public Expression<Func<T, bool>> Start(Expression<Func<T, bool>> exp)
    {
        if (IsStarted)
            throw new NotSupportedException("Predicate cannot be started again.");

        return _predicate = exp;
    }

    /// <summary>Or</summary>
    public Expression<Func<T, bool>>? Or(Expression<Func<T, bool>> expr2) => IsStarted
        ? _predicate = Predicate?.Or(expr2)
        : Start(expr2);

    /// <summary>And</summary>
    public Expression<Func<T, bool>>? And(Expression<Func<T, bool>> expr2) => IsStarted
        ? _predicate = Predicate?.And(expr2)
        : Start(expr2);

    /// <summary>Not</summary>
    public Expression<Func<T, bool>>? Not()
    {
        if (IsStarted)
            _predicate = Predicate?.Not();
        else
            Start(x => false);
        return _predicate;
    }

    /// <summary> Show predicate string </summary>
    public override string? ToString() => Predicate?.ToString();

    #region Implicit Operators

    /// <summary>
    ///     Allows this object to be implicitly converted to an Expression{Func{T, bool}}.
    /// </summary>
    /// <param name="right"></param>
    public static implicit operator Expression<Func<T, bool>>?(ExpressionStarter<T> right) => right.Predicate;

    /// <summary>
    ///     Allows this object to be implicitly converted to an Expression{Func{T, bool}}.
    /// </summary>
    /// <param name="right"></param>
    public static implicit operator Func<T, bool>?(ExpressionStarter<T> right) =>
        right.IsStarted || right.UseDefaultExpression
            ? right.Predicate?.Compile()
            : null;

    /// <summary>
    ///     Allows this object to be implicitly converted to an Expression{Func{T, bool}}.
    /// </summary>
    /// <param name="right"></param>
    public static implicit operator ExpressionStarter<T>?(Expression<Func<T, bool>>? right) => right is null
        ? null
        : new ExpressionStarter<T>(right);

    #endregion

    #region Implement LamdaExpression methods and properties

    /// <summary></summary>
    public Expression? Body => Predicate?.Body;


    /// <summary></summary>
    public ExpressionType? NodeType => Predicate?.NodeType;

    /// <summary></summary>
    public ReadOnlyCollection<ParameterExpression>? Parameters => Predicate?.Parameters;

    /// <summary></summary>
    public Type? Type => Predicate?.Type;

    #endregion
}
