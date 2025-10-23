using System.IO;

namespace FFDrop.DomainServices;

internal static class ProgramFinder
{
    public static bool TryFindProgramPath(string programName, out string programPath)
    {
        var paths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? Array.Empty<string>();

        List<string> checkPaths =
        [
            AppContext.BaseDirectory, .. paths
        ];
        
        foreach (var path in checkPaths)
        {
            var fullPath = Path.Combine(path, programName);
            if (File.Exists(fullPath))
            {
                programPath = fullPath;
                return true;
            }
        }
        programPath = string.Empty;
        return false;
    }
}
