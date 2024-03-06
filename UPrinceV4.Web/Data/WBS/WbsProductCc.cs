using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WBS
{
    public class WbsProductCc
    {
        public string Id { get; set; }
        public string PersonId { get; set; }
        public string WbsProductId { get; set; }
        [NotMapped] public virtual string PersonName { get; set; }
        [NotMapped] public virtual string EmailAddress { get; set; }

    }
}
