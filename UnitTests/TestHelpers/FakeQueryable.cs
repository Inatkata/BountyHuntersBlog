using System.Collections.Generic;
using System.Linq;

namespace BountyHuntersBlog.UnitTests.TestHelpers
{
    public static class FakeQueryable
    {
        public static IQueryable<T> AsQ<T>(this IEnumerable<T> items) => items.AsQueryable();
    }
}