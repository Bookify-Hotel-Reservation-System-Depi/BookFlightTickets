using DAL.models;
using System.Linq.Expressions;

namespace BLLProject.Specifications
{
    public interface ISpecification<T> where T : BaseClass
    {
        public Expression<Func<T, bool>> Criteria { get;}
        public List<Expression<Func<T, object>>> Includes { get; }
        public List<Func<IQueryable<T>, IQueryable<T>>> ComplexIncludes { get; }
    }
}
