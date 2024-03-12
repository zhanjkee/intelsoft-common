using System.ComponentModel.DataAnnotations;

namespace Intelsoft.Common.Configuration.UnitTests;

public class TestOptions
{
    [Required]
    public string RequiredSetting { get; set; }
}