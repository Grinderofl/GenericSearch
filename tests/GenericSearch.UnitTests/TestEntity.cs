namespace GenericSearch.UnitTests
{
    public class TestEntity
    {
        public TestEntity(bool always, bool whenTrue, bool whenTrueDefaultTrue, bool notNull, bool notNullDefaultTrue, bool notNullDefaultFalse)
        {
            MatchAlways = always;
            MatchWhenTrue = whenTrue;
            MatchWhenTrueDefaultTrue = whenTrueDefaultTrue;
            MatchWhenNotNull = notNull;
            MatchWhenNotNullDefaultTrue = notNullDefaultTrue;
            MatchWhenNotNullDefaultFalse = notNullDefaultFalse;

            Id = $"Always = '{MatchAlways}', True = '{MatchWhenTrue}', TrueDefaultTrue = '{MatchWhenTrueDefaultTrue}', NotNull = '{MatchWhenNotNull}', NotNullDefaultTrue = '{MatchWhenNotNullDefaultTrue}', NotNullDefaultFalse = '{MatchWhenNotNullDefaultFalse}'";
        }

        public string Id { get; set; }

        // BooleanSearch, match based on value, no default
        public bool MatchAlways { get; set; }

        // TrueBooleanSearch, match when true, no default
        public bool MatchWhenTrue { get; set; }

        // TrueBooleanSearch, match when true, default true
        public bool MatchWhenTrueDefaultTrue { get; set; }

        // OptionalBooleanSearch, match when not null, no default
        public bool MatchWhenNotNull { get; set; }

        // OptionalBooleanSearch, match when not null, default true
        public bool MatchWhenNotNullDefaultTrue { get; set; }

        // OptionalBooleanSearch, match when not null, default false
        public bool MatchWhenNotNullDefaultFalse { get; set; }
    }
}