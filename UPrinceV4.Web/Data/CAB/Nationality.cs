namespace UPrinceV4.Web.Data.CAB;

public class Nationality
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string NationalityId { get; set; }
    public string DisplayOrder { get; set; }
}

public class NationalityDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class NationalityFilter
{
    public string Name { get; set; }
}