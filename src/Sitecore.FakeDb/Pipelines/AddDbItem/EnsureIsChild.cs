namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  public class EnsureIsChild
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var dataStorage = args.DataStorage;

      var parent = dataStorage.GetFakeItem(item.ParentID);
      if (parent == null)
      {
        return;
      }

      if (!parent.Children.Contains(item))
      {
        parent.Children.Add(item);
      }
    }
  }
}