namespace Sitecore.FakeDb.Serialization.Tests.AutoXunit
{
  using FluentAssertions;
  using Sitecore.FakeDb.AutoXunit;
  using Xunit;

  public class AutoDbTest
  {
    [Theory]
    [AutoDb]
    public void ShouldDeserializeItems(Db db)
    {
      // act
      var item = db.GetItem("/sitecore/content/home");

      // assert
      item.Should().NotBeNull();
      item["Title"].Should().Be("Sitecore");
    }
  }
}