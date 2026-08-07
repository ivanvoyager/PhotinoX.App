namespace PhotinoX.App;

internal static class PathResolver
{
    internal static string ResolveContentRootPath(string? contentRootPath, string basePath)
    {
        if (string.IsNullOrWhiteSpace(contentRootPath))
        {
            return Path.GetFullPath(basePath);
        }
        if (!Path.IsPathRooted(contentRootPath))
        {
            contentRootPath = Path.Combine(Path.GetFullPath(basePath), contentRootPath);
        }
        return Path.GetFullPath(contentRootPath);
    }

    internal static string ResolveWebRootPath(string? webRootPath, string contentRootPath)
    {
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            // Default to /wwwroot if it exists.
            var wwwroot = Path.Combine(contentRootPath, "wwwroot");
            if (Directory.Exists(wwwroot))
            {
                webRootPath = wwwroot;
            }
        }
        else if (!Path.IsPathRooted(webRootPath))
        {
            webRootPath = Path.Combine(contentRootPath, webRootPath);
        }
        if (!string.IsNullOrWhiteSpace(webRootPath))
        {
            return Path.GetFullPath(webRootPath);
        }
        return contentRootPath;
    }
}
