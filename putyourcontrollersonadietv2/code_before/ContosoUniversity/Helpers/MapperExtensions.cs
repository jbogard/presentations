namespace ContosoUniversity.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using DelegateDecompiler;
    using PagedList;

    public static class MapperExtensions
    {
        public static List<TDestination> ProjectToList<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().Decompile().ToList();
        }

        public static IQueryable<TDestination> ProjectToQueryable<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().Decompile();
        }

        public static IPagedList<TDestination> ProjectToPagedList<TDestination>(this IQueryable queryable, int pageNumber, int pageSize)
        {
            return queryable.ProjectTo<TDestination>().Decompile().ToPagedList(pageNumber, pageSize);
        }

        public static TDestination ProjectToSingleOrDefault<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().Decompile().SingleOrDefault();
        }
    }
}