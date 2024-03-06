namespace UPrinceV4.Web.Data.PMOL
{
    public class PmolCbcResources
    {
        public string Id { get; set; }
        public string PmolId { get; set; }
        public string LotId { get; set; }
        public string ArticleNo { get; set; }
        public string Quantity { get; set; }
        public string ConsumedQuantity { get; set; }



    }
    public class GetPmolCbcResourcesDto
    {
        public string Id { get; set; }
        public string PmolId { get; set; }
        public string LotId { get; set; }
        public string ArticleNo { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public string Title { get; set; }
        public string CbcQuantity { get; set; }
        public string ConsumedQuantity { get; set; }

    }


}
