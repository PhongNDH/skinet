using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; } // lambda expression that takes a single parameter of type T and returns a boolean value (bool)
        List<Expression<Func<T, object>>> Includes { get; }
    }
}