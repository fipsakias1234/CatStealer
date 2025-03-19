using CatStealer.Domain.Audit;
using CatStealer.Domain.Cats;

namespace CatStealer.Domain.Tags
{
    public class TagEntity : AuditableBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CatEntity> Cats { get; set; } = new List<CatEntity>();

    }
}
