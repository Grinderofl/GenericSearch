using System.Collections.Generic;

namespace Grinderofl.GenericSearch.UnitTests
{
    internal class TestEntityHelper
    {
        public static IEnumerable<TestEntity> CreateEntities()
        {
            // All true
            yield return new TestEntity(true, true, true, true, true, true);
            yield return new TestEntity(false, false, false, false, false, false);
            yield return new TestEntity(true, false, true, false, true, false);
            yield return new TestEntity(true, false, true, false, false, true);
            yield return new TestEntity(true, false, false, false, false, false);
            yield return new TestEntity(true, true, false, false, false, false);
            yield return new TestEntity(true, false, true, true, true, true);
        }
    }
}