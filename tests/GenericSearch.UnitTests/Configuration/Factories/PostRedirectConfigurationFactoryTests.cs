using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Configuration.Factories;
using GenericSearch.Definition.Expressions;
using GenericSearch.UnitTests.Scoping;
using Xunit;

namespace GenericSearch.UnitTests.Configuration.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class PostRedirectConfigurationFactoryTests : ConfigurationFactoryTestBase
    {
        private PostRedirectGetConfigurationFactory Factory => new PostRedirectGetConfigurationFactory(Options);

        [Fact]
        public void Create_NoDefinition_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var configuration = Factory.Create(definition);
            configuration.ActionName.Should().Be("Index");
            configuration.Enabled.Should().BeTrue();
        }

        [Fact]
        public void Create_ActionName_Succeeds()
        {
            var list = TestListDefinition.Create<Request, Item, Result>();
            var definition = new PostRedirectGetExpression();
            definition.UseActionName("List");
            list.PostRedirectGetDefinition = definition;

            var configuration = Factory.Create(list);

            configuration.ActionName.Should().Be("List");
            configuration.Enabled.Should().BeTrue();
        }

        [Fact]
        public void Create_Enabled_Succeeds()
        {
            var list = TestListDefinition.Create<Request, Item, Result>();
            var definition = new PostRedirectGetExpression();
            definition.Enable();
            list.PostRedirectGetDefinition = definition;

            var configuration = Factory.Create(list);

            configuration.ActionName.Should().Be("Index");
            configuration.Enabled.Should().BeTrue();
        }

        [Fact]
        public void Create_Disabled_Succeeds()
        {
            var list = TestListDefinition.Create<Request, Item, Result>();
            var definition = new PostRedirectGetExpression();
            definition.Disable();
            list.PostRedirectGetDefinition = definition;
            var configuration = Factory.Create(list);

            configuration.ActionName.Should().Be("Index");
            configuration.Enabled.Should().BeFalse();
        }

        
    }
}