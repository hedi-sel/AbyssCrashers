using System.Collections.Generic;
using System.IO;
using Godot;

public static class DirectoryExplorerExtensions
{
    public static IEnumerable<T> GetResources<T>(this DirAccess directory, string allowedExtension = "tscn")
        where T : Resource
    {
        directory.ListDirBegin();
        var file = directory.GetNext();
        while (file != "")
        {
            file = Path.Join(directory.GetCurrentDir(), file);
            if (file.GetExtension() == allowedExtension)
            {
                yield return ResourceLoader.Load<T>(file);
            }

            file = directory.GetNext();
        }
    }
}