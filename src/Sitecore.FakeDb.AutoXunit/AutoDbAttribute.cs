namespace Sitecore.FakeDb.AutoXunit
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using Sitecore.Data.Serialization.ObjectModel;
  using Sitecore.FakeDb.Serialization;
  using Xunit.Sdk;

  public class AutoDbAttribute : DataAttribute
  {
    private readonly string path;

    public AutoDbAttribute()
    {
    }

    public AutoDbAttribute(string path)
    {
      this.path = path;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
      const string SerializationFolderName = "master";

      var db = new Db();

      var templates = LoadSerializedTemplates(SerializationFolderName);
      Add(db, templates);

      if (this.path != "/sitecore/templates")
      {
        var content = LoadSerializedItems(SerializationFolderName, "/sitecore/content");
        Add(db, content);

        var system = LoadSerializedItems(SerializationFolderName, "/sitecore/system");
        Add(db, system);
      }

      return new List<object[]> { new object[] { db } };
    }

    private static IEnumerable<KeyValuePair<SyncItem, FileInfo>> GetSerializationInfo(string serializationFolderName, string root, bool recursive)
    {
      var serializationFolder = Deserializer.GetSerializationFolder(serializationFolderName);
      var truePath = ShortenedPathsDictionary.FindTruePath(serializationFolder, root);

      var folder =
        new DirectoryInfo(
          Path.Combine(
            serializationFolder.FullName.Trim(new[] { Path.DirectorySeparatorChar }),
            truePath.Replace('/', Path.DirectorySeparatorChar).Trim(new[] { Path.DirectorySeparatorChar })));

      return folder.Deserialize(recursive);
    }

    private static IEnumerable<DsDbTemplate> LoadSerializedTemplates(string serializationFolderName)
    {
      return GetSerializationInfo(serializationFolderName, "/sitecore/templates", true)
        .Where(si => si.Key.TemplateID == TemplateIDs.Template.ToString())
        .Select(si => new DsDbTemplate(serializationFolderName, si.Key, si.Value));
    }

    private static IEnumerable<DsDbItem> LoadSerializedItems(string serializationFolderName, string path)
    {
      return GetSerializationInfo(serializationFolderName, path, true)
        .Where(si => si.Key.TemplateID != TemplateIDs.Template.ToString())
        .Select(si => new DsDbItem(serializationFolderName, si.Key, si.Value, true));
    }

    private static void Add(Db db, IEnumerable<DbItem> items)
    {
      foreach (var item in items)
      {
        db.Add(item);
      }
    }
  }
}