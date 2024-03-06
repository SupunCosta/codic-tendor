using System;

namespace UPrinceV4.Web.Data.Stock;

public class StockType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
}

public class StockTypeDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class StockDropdownAddDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string StatusId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public string Label { get; set; }
}

public class Databases
{
    public string DatabaseName { get; set; }
    public string ProjectConnectionString { get; set; }
}

public class DatabasesException
{
    public string DatabaseName { get; set; }

    public Exception Exception { get; set; }
}