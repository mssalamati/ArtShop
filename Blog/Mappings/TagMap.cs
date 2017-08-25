
using Blog.Objects;
using FluentNHibernate.Mapping;


namespace Blog.Mappings
{
  public class TagMap: ClassMap<Tag>
  {
    public TagMap()
    {
      Id(x => x.Id);
      Map(x => x.Name).Length(50).Not.Nullable();
      Map(x => x.UrlSlug).Length(50).Not.Nullable();
      Map(x => x.Description).Length(200);
      HasManyToMany(x => x.Posts).Cascade.All().Inverse().Table("PostTagMap");
    }
  }
}
