namespace UPrinceV4.Web.Data.PBS_;

public class PbsScopeOfWork
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Quantity { get; set; }
    public string MouId { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
}

public class PbsScopeOfWorkCreateDto
{
    public string ProductId { get; set; }
    public string Quantity { get; set; }
    public string Mou { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
}

public class PbsScopeOfWorkGetByIdDto
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Quantity { get; set; }
    public string MouId { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public CpcBasicunitofMeasure Mou { get; set; }
}

public class CpcBasicunitofMeasure
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class PbsScopeOfWorkRfq
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string Quantity { get; set; }
    public string MouId { get; set; }
    public string UnitPrice { get; set; }
    public string TotalPrice { get; set; }
    public string PbsSequenceId { get; set; }
    public string PbsTitle { get; set; }
}