using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.Configuration;

namespace Intelsoft.Common.Configuration;

public static class ConfigurationExtensions
{
    /// <summary>
    ///     Retrieves a configuration section by name and binds it to an instance of <typeparamref name="T"/>.
    ///     Validates the bound instance using data annotations and throws an exception if validation fails.
    /// </summary>
    /// <typeparam name="T">The type of the options to be bound and validated. This type should be a class with public properties and data annotation validations.</typeparam>
    /// <param name="configuration">The configuration instance from which to retrieve the section.</param>
    /// <param name="sectionName">The name of the configuration section to bind to <typeparamref name="T"/>.</param>
    /// <returns>An instance of <typeparamref name="T"/> with properties bound to the configuration values found in the specified section. This instance is validated against data annotations defined on the type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="sectionName"/> is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if validation fails for the created <typeparamref name="T"/> instance. The exception will contain details about which validations failed.</exception>
    public static T GetValidOptions<T>(this IConfiguration configuration, string sectionName)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(sectionName);

        var configurationSection = configuration.GetSection(sectionName);
        var optionsInstance = Activator.CreateInstance<T>();

        if (!configurationSection.Exists())
        {
            throw new InvalidOperationException($"Configuration section '{sectionName}' not found");
        }

        configurationSection.Bind(optionsInstance);

        ValidateOptions(optionsInstance);

        return optionsInstance;
    }

    private static void ValidateOptions<T>(T options) where T : class
    {
        var validationContext = new ValidationContext(options);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(options, validationContext, validationResults, true);

        if (isValid) return;
        
        var validationErrors = validationResults.Select(result => result.ErrorMessage);
        var errorMessage = $"Validation failed for type {typeof(T).Name}: {string.Join(", ", validationErrors)}";
        throw new InvalidOperationException(errorMessage);
    }
}