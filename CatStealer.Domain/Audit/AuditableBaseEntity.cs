namespace CatStealer.Domain.Audit
{
    public abstract class AuditableBaseEntity
    {
        public DateTime? CreatedOn { get; set; }
    }
}
