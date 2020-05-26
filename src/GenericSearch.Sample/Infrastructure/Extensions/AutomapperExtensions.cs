using System.Linq;
using AutoMapper;

namespace GenericSearch.Sample.Infrastructure.Extensions
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Extension method to project from a queryable using the mapping engine from provided mapper.
        /// </summary>
        /// <remarks>Projections are only calculated once and cached</remarks>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Queryable source</param>
        /// <param name="mapper">Mapper instance</param>
        /// <returns>Expression to project into</returns>
        public static IQueryable<TDestination> ProjectTo<TDestination>(this IQueryable source, IMapper mapper)
        {
            return mapper.ProjectTo<TDestination>(source);
        }
    }
}
