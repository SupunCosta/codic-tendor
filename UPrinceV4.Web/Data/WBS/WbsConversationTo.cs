using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WBS
{
    public class WbsConversationTo
    {
        public string Id { get; set; }
        public string PersonId { get; set; }
        public string WbsConversationId { get; set; }
        [NotMapped] public string PersonName { get; set; }
        [NotMapped]public virtual string EmailAddress { get; set; }
    }
}
