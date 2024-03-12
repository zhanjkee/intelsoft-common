using Microsoft.Extensions.Configuration;

namespace Intelsoft.Common.Configuration.UnitTests;

public class ConfigurationUnitTests
{
    [Fact]
    public void GetValidOptions_WithValidConfiguration_ShouldReturnValidOptions()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string>
        {
            { "TestSection:RequiredSetting", "SomeValue" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        // Act
        var options = configuration.GetValidOptions<TestOptions>("TestSection");

        // Assert
        Assert.NotNull(options);
        Assert.Equal("SomeValue", options.RequiredSetting);
    }

    [Fact]
    public void GetValidOptions_WithMissingSection_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => configuration.GetValidOptions<TestOptions>("MissingSection"));
    }

    [Fact]
    public void GetValidOptions_WithInvalidConfiguration_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "TestSection:RequiredSetting", null } // RequiredSetting is missing
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act & Assert
        var exception =
            Assert.Throws<InvalidOperationException>(() => configuration.GetValidOptions<TestOptions>("TestSection"));

        // Additional Assert to check if the error message contains the expected text
        Assert.Contains("Validation failed", exception.Message);
    }
}