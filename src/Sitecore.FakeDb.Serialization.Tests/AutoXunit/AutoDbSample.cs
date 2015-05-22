namespace Sitecore.FakeDb.Serialization.Tests.AutoXunit
{
  using FluentAssertions;
  using Sitecore.Analytics;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.AutoXunit;
  using Xunit;

  public class AutoDbSample
  {
    private readonly SimpleCampaignRepository campaignRepository = new SimpleCampaignRepository();

    [Theory, AutoDb]
    public void ShouldCreateNewCampaign(Db db)
    {
      // arrange
      var campaign = new Campaign { Name = "My Campaign" };

      // act
      this.campaignRepository.Create(campaign);

      // assert
      db.GetItem("/sitecore/system/Marketing Control Panel/Campaigns/My Campaign").Should().NotBeNull();
    }

    [Theory, AutoDb]
    public void ShouldGetCampaign(Db db)
    {
      // arrange
      var id = ID.NewID;
      db.Add(new DbItem("My Campaign", id) { ParentID = AnalyticsIds.CampaignRoot });

      // act
      var campaign = this.campaignRepository.Get(id);

      // assert
      campaign.Name.Should().Be("My Campaign");
    }

    [Theory, AutoDb]
    public void ShouldUpdateCampaign(Db db)
    {
      // arrange
      var id = ID.NewID;
      db.Add(new DbItem("My Old Campaign", id) { ParentID = AnalyticsIds.CampaignRoot });

      var campaign = new Campaign { Id = id, Name = "My New Campaign" };

      // act
      this.campaignRepository.Update(campaign);

      // assert
      db.GetItem(id).Name.Should().Be("My New Campaign");
    }

    [Theory, AutoDb]
    public void ShoulDeleteCampaign(Db db)
    {
      // arrange
      var id = ID.NewID;
      db.Add(new DbItem("My Old Campaign", id) { ParentID = AnalyticsIds.CampaignRoot });

      // act
      this.campaignRepository.Delete(id);

      // assert
      db.GetItem(id).Should().BeNull();
    }
  }

  public class Campaign
  {
    public ID Id { get; set; }

    public string Name { get; set; }
  }

  public class SimpleCampaignRepository
  {
    private readonly Database database = Database.GetDatabase("master");

    public void Create(Campaign campaign)
    {
      var campaignsRoot = this.database.GetItem(AnalyticsIds.CampaignRoot);
      campaignsRoot.Add(campaign.Name, new TemplateID(new ID("{94FD1606-139E-46EE-86FF-BC5BF3C79804}")));
    }

    public Campaign Get(ID id)
    {
      var item = this.database.GetItem(id);
      return new Campaign { Id = id, Name = item.Name };
    }

    public void Update(Campaign campaign)
    {
      var item = this.database.GetItem(campaign.Id);
      using (new EditContext(item))
      {
        item.Name = campaign.Name;
      }
    }

    public void Delete(ID id)
    {
      this.database.GetItem(id).Delete();
    }
  }
}