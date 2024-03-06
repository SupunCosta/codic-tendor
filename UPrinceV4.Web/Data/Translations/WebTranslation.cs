using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.Translations;

public class WebTranslation
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    [ForeignKey("Language")] public int LanguageId { get; set; }
    public virtual Language Language { get; set; }
}

public class UpdateWebTranslationDto
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public int LanguageId { get; set; }
}

public class AddWebTranslationDto
{
    public string Key { get; set; }
    public string Value { get; set; }
    public int LanguageId { get; set; }
}