namespace PhotinoX.App;

/// <summary>
/// Represents Linux-specific PhotinoX settings.
/// </summary>
public sealed class PhotinoLinuxSettings
{
    /// <summary>
    /// Gets or sets the native chromeless drag region height.
    /// </summary>
    public int? ChromelessDragRegionHeight { get; set; }

    /// <summary>
    /// Gets or sets the left inset excluded from the native chromeless drag region.
    /// </summary>
    public int? ChromelessDragRegionLeftInset { get; set; }

    /// <summary>
    /// Gets or sets the right inset excluded from the native chromeless drag region.
    /// </summary>
    public int? ChromelessDragRegionRightInset { get; set; }

    /// <summary>
    /// Gets or sets the native chromeless resize border thickness.
    /// </summary>
    public int? ChromelessResizeBorderThickness { get; set; }
}