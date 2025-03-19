using CatStealer.Domain.Audit;

namespace CatStealer.Domain.Cats
{
    public class CatEntity : AuditableBaseEntity
    {
        public int Id { get; set; }
    }
}
