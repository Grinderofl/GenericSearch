using System.Linq;
using GenericSearch.Searches;

namespace GenericSearch.Sample.Features.Customers
{
    public class CustomerFreeTextSearch : Search<Index.Projection>
    {
        public string Term { get; set; }

        public override bool IsActive() => !string.IsNullOrWhiteSpace(Term);

        protected override IQueryable<Index.Projection> ApplyToQuery(IQueryable<Index.Projection> query)
        {
            if (!IsActive())
            {
                return query;
            }

            return query.Where(x => x.CompanyName.Contains(Term) ||
                                    x.ContactName.Contains(Term) ||
                                    x.Region.Contains(Term) ||
                                    x.City.Contains(Term));
        }
    }
}