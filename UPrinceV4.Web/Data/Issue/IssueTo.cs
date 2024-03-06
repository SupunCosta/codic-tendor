using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.Issue
{
    public class IssueTo
    {
        public string Id { get; set; }
        public string PersonId { get; set; }
        public string IssueId { get; set; }
        public bool IsDefault { get; set; }
        [NotMapped] public virtual string PersonName { get; set; }
        [NotMapped] public virtual string EmailAddress { get; set; }
    }
}
