using System;

namespace UPrinceV4.Web.Data.Translations;

public class Language
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string Lang { get; set; }
    public string Code { get; set; }

    internal static void Update(Language r)
    {
        throw new NotImplementedException();
    }

    internal static void SaveChanges()
    {
        throw new NotImplementedException();
    }
}

public class AddLanguageDto
{
    public string Country { get; set; }
    public string Lang { get; set; }
    public string Code { get; set; }
}

public class UpdateLanguageDto
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string Lang { get; set; }
    public string Code { get; set; }
}

public class SelectLanguageIdDto
{
    public int Id { get; set; }
}