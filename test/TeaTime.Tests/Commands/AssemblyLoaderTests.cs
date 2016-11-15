namespace TeaTime.Tests.Commands
{
    using System.Linq;
    using TeaTime.Commands.Loader;
    using Xunit;

    public class AssemblyLoaderTests
    {
        [Fact]
        public void CanLoadAssemblies()
        {
            var assemblies = CommandLoader.GetAssemblies();

            Assert.NotNull(assemblies);
            Assert.True(assemblies.Any());

            Assert.All(assemblies, assembly => assembly.FullName.StartsWith("TeaTime"));
        }
    }
}
