namespace UPrinceV4.Web.Data.PdfToExcel
{
    public class ContractorsTotalPriceErrors
    {
        public string Id { get; set; }
        public string ArticleNo { get; set; }
        public string LotId { get; set; }
        public string ContractorPdfId { get; set; }
        public string CompanyId { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public string Quantity { get; set; }
        public float? UnitPrice { get; set; }
        public float? TotalPrice { get; set; }
        public float? CorrectTotalPrice { get; set; }
    }
}
