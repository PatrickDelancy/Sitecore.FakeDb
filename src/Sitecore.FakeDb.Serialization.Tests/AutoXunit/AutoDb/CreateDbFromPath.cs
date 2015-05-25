﻿namespace Sitecore.FakeDb.Serialization.Tests.AutoXunit.AutoDb
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
    [Theory(DisplayName = "All serialized system items are loaded and can be located by path")]
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
    [Theory(DisplayName = "Content items are not loaded")]
    public void ContentItemsNotLoaded(Db db)
    {
      db.GetItem("/sitecore/content").Children.Should().BeEmpty();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "System items are not loaded")]
    public void SystemItemsNotLoaded(Db db)
    {
      db.GetItem("/sitecore/system/Marketing Control Panel/Campaigns").Should().BeNull();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "All templates are loaded")]
    public void AllTemplatesLoaded(Db db)
    {
      db.Database.GetTemplate(Constants.SomeTemplateId).Should().NotBeNull();
    }
  }

  [Trait("AutoDb", @"Load a single item: [AutoDb(""/sitecore/content/Home/Child Item"")]")]
  public class LoadSingleItem
  {
    private const string Path = "/sitecore/content/Home/Child Item";

    [AutoDb(Path)]
    [Theory(DisplayName = "Item is loaded")]
    public void ItemLoaded(Db db)
    {
      db.GetItem(Path).Should().NotBeNull();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "[TBD]Child items are loaded")]
    public void ChildItemsNotLoaded(Db db)
    {
      // TODO: Decide if the child items should be loaded.
      db.GetItem(Path).Children.Should().NotBeEmpty();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "Parent is loaded")]
    public void ParentGenerated(Db db)
    {
      db.GetItem(Path.GetParentPath()).TemplateID.Should().Be(SerializationId.SampleItemTemplate);
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "[TBD]System items are not loaded")]
    public void SystemItemsNotLoaded(Db db)
    {
      // TODO: Decide if the System items should be loaded.
      db.GetItem("/sitecore/system/Marketing Control Panel/Campaigns").Should().NotBeNull();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "All templates are loaded")]
    public void AllTemplatesLoaded(Db db)
    {
      db.Database.GetTemplate(Constants.SomeTemplateId).Should().NotBeNull();
    }
  }

  [Trait("AutoDb", @"Load a single item if there is no parent serialization: [AutoDb(""/sitecore/content/Folder Without Serialization/Item in Folder Without Serialization"")]")]
  public class LoadSingleItemWithoutParentSerialization
  {
    private const string Path = "/sitecore/content/Folder Without Serialization/Item in Folder Without Serialization";

    [AutoDb(Path)]
    [Theory(DisplayName = "Item is loaded")]
    public void ItemLoaded(Db db)
    {
      db.GetItem(Path).Should().NotBeNull();
    }

    [AutoDb(Path)]
    [Theory(DisplayName = "Parent is not loaded but auto-generated")]
    public void ParentGenerated(Db db)
    {
      db.GetItem(Path.GetParentPath()).TemplateID.Should().Be(TemplateIDs.Folder);
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