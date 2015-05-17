namespace Sitecore.FakeDb.Serialization.Tests.AutoXunit.AutoDb
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.AutoXunit;
  using Xunit;

  [Trait("AutoDb", "Load all serialized items: [AutoDb]")]
  public class LoadAllSerializedItems
  {
    [AutoDb]
    [Theory(DisplayName = @"Context database is ""master""")]
    public void DatabaseMaster(Db db)
    {
      db.Database.Name.Should().Be("master");
    }

    [AutoDb]
    [Theory(DisplayName = "All serialized content items are loaded")]
    public void ContentItemsLoaded(Db db)
    {
      db.GetItem("/sitecore/content/Home/Child Item/Grandchild Item").Should().NotBeNull();
    }

    [AutoDb]
    [Theory(DisplayName = "All serialized system items are loaded and can be located by id")]
    public void SystemItemsLoadedById(Db db)
    {
      db.GetItem(Constants.CampaignsId).Should().NotBeNull();
    }


    [AutoDb]
    [Theory(DisplayName = "All serialized system items are loaded and can be located by path", Skip = "To be implemented.")]
    public void SystemItemsLoadedByPath(Db db)
    {
      db.GetItem("/sitecore/system/Marketing Control Panel/Campaigns").Should().NotBeNull();
    }

    [AutoDb]
    [Theory(DisplayName = "All serialized templates are loaded")]
    public void TemplatesLoaded(Db db)
    {
      db.Database.GetTemplate(Constants.SomeTemplateId).Should().NotBeNull();
    }
  }

  [Trait("AutoDb", @"Load all serialized templates: [AutoDb(""/sitecore/templates"")]")]
  public class LoadAllTemplates
  {
    private const string Path = "/sitecore/templates";

    [AutoDb(Path)]
    [Theory(DisplayName = "Content items are not loaded", Skip = "To be implemented.")]
    public void ContentItemsNotLoaded(Db db)
    {
      db.GetItem("/sitecore/content").Children.Should().BeEmpty();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "System items are not loaded", Skip = "To be implemented.")]
    public void SystemItemsNotLoaded(Db db)
    {
      db.GetItem("/sitecore/system").Children.Should().BeEmpty();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "All templates are loaded")]
    public void AllTemplatesLoaded(Db db)
    {
      db.Database.GetTemplate(Constants.SomeTemplateId).Should().NotBeNull();
    }
  }

  [Trait("AutoDb", @"Load single item: [AutoDb(""/sitecore/content/Home/Child Item"")]")]
  public class LoadSingleItem
  {
    private const string Path = "/sitecore/content/Home/Child Item";

    [AutoDb(Path)]
    [Theory(DisplayName = "The item is loaded")]
    public void ItemLoaded(Db db)
    {
      db.GetItem(Path).Should().NotBeNull();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "Children are not loaded", Skip = "To be implemented.")]
    public void ChildrenNotLoaded(Db db)
    {
      db.GetItem(Path).Children.Should().BeEmpty();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "Parent is not loaded but auto-generated")]
    public void ParentGenerated(Db db)
    {
      db.GetItem(Path).Parent.Versions.Count.Should().Be(1);
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "All templates are loaded")]
    public void AllTemplatesLoaded(Db db)
    {
      db.Database.GetTemplate(Constants.SomeTemplateId).Should().NotBeNull();
    }
  }

  public static class Constants
  {
    public static readonly ID CampaignsId = new ID("{EC095310-746F-4C1B-A73F-941863564DC2}");

    public static readonly ID SomeTemplateId = new ID("{F6A72DBF-558F-40E5-8033-EE4ACF027FE2}");
  }
}