
using Blog.Objects;
using FluentNHibernate.Mapping;


namespace Blog.Mappings
{
  public class CategoryMap : ClassMap<Category>
  {
    public CategoryMap()
    {
      Id(x => x.Id);
      Map(x => x.Name).Length(50).Not.Nullable();
      Map(x => x.UrlSlug).Length(50).Not.Nullable();
      HasMany(x => x.Posts).Inverse().Cascade.All().KeyColumn("Category");
    }
  }
}
