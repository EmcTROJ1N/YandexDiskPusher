using System.Threading.Tasks.Dataflow;

namespace YaDiskObserver;

public static class SysFuncs
{
    public static string GetDateTime() =>
        DateTime.Today.ToShortDateString().Replace("/", "_");

    public static string GetParentDirectory(string path) =>
        string.Join("/", path.Split(new char[] { '/', '\\' }).SkipLast(1));
}