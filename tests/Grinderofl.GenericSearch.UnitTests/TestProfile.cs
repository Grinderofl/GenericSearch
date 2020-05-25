namespace Grinderofl.GenericSearch.UnitTests
{
    public class TestProfile : GenericSearchProfile
    {
        public TestProfile()
        {
            CreateTestFilter();
        }

        private void CreateTestFilter()
        {
            CreateFilterCore();
        }
        
        protected virtual IFilterExpression<TestEntity, TestRequest, TestResult> CreateFilterCore()
        {
            return CreateFilter<TestEntity, TestRequest, TestResult>();
        }
    }
}