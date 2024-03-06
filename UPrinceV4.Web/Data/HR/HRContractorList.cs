using System;

namespace UPrinceV4.Web.Data.HR
{
    public class HRContractorList
    {
        public string Id { get; set; }
        public string HRId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ContractTypeId { get; set; }
        public string Url { get; set; }
    }
}
