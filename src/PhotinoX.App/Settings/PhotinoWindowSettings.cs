namespace PhotinoX.App;

/// <summary>
/// Represents native window settings.
/// </summary>
public sealed class PhotinoWindowSettings
{
    /// <summary>
    /// Gets or sets the window title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the initial window width in pixels.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the initial window height in pixels.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the initial window left position in pixels.
    /// </summary>
    public int? Left { get; set; }

    /// <summary>
    /// Gets or sets the initial window top position in pixels.
    /// </summary>
    public int? Top { get; set; }

    /// <summary>
    /// Gets or sets whether the window should be centered on startup.
    /// </summary>
    public bool? CenterOnInitialize { get; set; }

    /// <summary>
    /// Gets or sets whether the operating system should choose the initial window size.
    /// </summary>
    public bool? UseOsDefaultSize { get; set; }

    /// <summary>
    /// Gets or sets whether the operating system should choose the initial window location.
    /// </summary>
    public bool? UseOsDefaultLocation { get; set; }

    /// <summary>
    /// Gets or sets whether the window can be resized by the user.
    /// </summary>
    public bool? Resizable { get; set; }

    /// <summary>
    /// Gets or sets whether the window should be borderless.
    /// </summary>
    public bool? Chromeless { get; set; }

    /// <summary>
    /// Gets or sets whether the window should use a transparent background.
    /// </summary>
    public bool? Transparent { get; set; }

    /// <summary>
    /// Gets or sets whether the window should stay above other windows.
    /// </summary>
    public bool? Topmost { get; set; }

    /// <summary>
    /// Gets or sets the startup URL, file path, or custom-scheme URI.
    /// </summary>
    public string? StartUrl { get; set; }

    /// <summary>
    /// Gets or sets the startup HTML content.
    /// </summary>
    public string? StartString { get; set; }

    /// <summary>
    /// Gets or sets the window icon file path.
    /// </summary>
    public string? IconFile { get; set; }
}