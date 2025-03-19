using CatStealer.Domain.Audit;
using CatStealer.Domain.Tags;

namespace CatStealer.Domain.Cats
{
    public class CatEntity : AuditableBaseEntity
    {
        public int Id { get; set; }

        public string CatId { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public string Image { get; set; }

        public ICollection<TagEntity> Tags { get; set; } = new List<TagEntity>();

    }
}
