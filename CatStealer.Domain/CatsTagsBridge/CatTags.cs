using CatStealer.Domain.Cats;
using CatStealer.Domain.Tags;

namespace CatStealer.Domain.CatsTagsBridge
{
    public class CatTags
    {
        public int CatsId { get; set; }

        public CatEntity Cat { get; set; }

        public int TagsId { get; set; }

        public TagEntity Tag { get; set; }
    }
}
