using CatStealer.Domain.Audit;
using CatStealer.Domain.CatsTagsBridge;

namespace CatStealer.Domain.Cats
{
    public class CatEntity : AuditableBaseEntity
    {
        public int Id { get; set; }

        public string CatId { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public string Image { get; set; }

        public ICollection<CatTags> CatTags { get; set; } = new List<CatTags>();

    }
}
