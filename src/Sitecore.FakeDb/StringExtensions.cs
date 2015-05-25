namespace Sitecore.FakeDb
{
  using Sitecore.IO;

  internal static class StringExtensions
  {
    public static string GetParentName(this string path)
    {
      return FileUtil.GetFileName(FileUtil.GetParentPath(path));
    }

    public static string GetParentPath(this string path)
    {
      return FileUtil.GetParentPath(path);
    }
  }
}