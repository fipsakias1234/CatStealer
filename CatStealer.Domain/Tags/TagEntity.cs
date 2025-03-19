using CatStealer.Domain.Audit;
using CatStealer.Domain.CatsTagsBridge;

namespace CatStealer.Domain.Tags
{
    public class TagEntity : AuditableBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CatTags> CatTags { get; set; } = new List<CatTags>();

    }
}
