namespace UPrinceV4.Web.Data.PBS_;

public class PbsSkill
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string LocaleCode { get; set; }
    public string ParentId { get; set; }
}

public class PbsSkillDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class GetPbsSkillDto
{
    public string Id { get; set; }
    public string Title { get; set; }
}