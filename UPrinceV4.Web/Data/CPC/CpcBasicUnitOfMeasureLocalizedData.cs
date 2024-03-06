using System;

namespace UPrinceV4.Web.Data.CPC;

public class CpcBasicUnitOfMeasureLocalizedData
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string BasicUnitOfMeasureId { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }

    internal static void Update(CpcBasicUnitOfMeasureLocalizedData r)
    {
        throw new NotImplementedException();
    }

    internal static void SaveChanges()
    {
        throw new NotImplementedException();
    }

    internal static void Add(CpcBasicUnitOfMeasureLocalizedData r)
    {
        throw new NotImplementedException();
    }
}

public class UpdateCpcBasicUnitOfMeasureLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}

public class AddCpcBasicUnitOfMeasureLocalizedDataDto
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string BasicUnitOfMeasureId { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
    public string Name { get; set; }
    public string DisplayOrder { get; set; }
    public string LocaleCode { get; set; }
}

public class GetCpcBasicUnitOfMeasureLocalizedDataByCode
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
    public string BasicUnitOfMeasureId { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
    public string Name { get; set; }
}