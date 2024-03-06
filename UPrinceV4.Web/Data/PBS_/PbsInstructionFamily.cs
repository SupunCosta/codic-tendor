namespace UPrinceV4.Web.Data.PBS_;

public class PbsInstructionFamily
{
    public string Id { get; set; }
    public string Family { get; set; }
    public string LocaleCode { get; set; }
    public string Type { get; set; }
}

public class PbsInstructionFamilyLoadDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class PbsInstructionFamilyDapper
{
    public string Key { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }
}