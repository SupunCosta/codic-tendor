using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PBS_
{
    public class PbsCbcResources
    {
        public string Id { get; set; }
        public string PbsId { get; set; }
        public string LotId { get; set; }
        public string ArticleNo { get; set; }
        public string Quantity { get; set; }
        public string ConsumedQuantity { get; set; }
        public string InvoicedQuantity { get; set; }
        public bool IsIgnore { get; set; }


    }

    public class GetPbsCbcResourcesDto
    {
        public string Id { get; set; }
        public string PbsId { get; set; }
        public string LotId { get; set; }
        public string ArticleNo { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public string Title { get; set; }
        public string CbcQuantity { get; set; }
        public string ConsumedQuantity { get; set; }
        public string InvoicedQuantity { get; set; }

    }
    
    public class AwardedLotDataDto
    {
        public string Id { get; set; }
        public string LotId { get; set; }
        public string Title { get; set; }

    }
    
    public class AwardedLotDataResult
    {
        public string Id { get; set; }
        public string LotId { get; set; }
        public string CompanyId { get; set; }
        public string Title { get; set; }
        public string ArticleNo { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }


    }
    
    public class PbsLotDataDto
    {
        public string PbsLotId { get; set; }
        public List<GetPbsCbcResourcesDto> Cbc { get; set; }


    }
}
