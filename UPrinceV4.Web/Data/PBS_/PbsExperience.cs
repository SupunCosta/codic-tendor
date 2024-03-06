namespace UPrinceV4.Web.Data.PBS_;

public class PbsExperience
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LocaleCode { get; set; }
}

public class PbsExperienceDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class GetPbsExperienceDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}