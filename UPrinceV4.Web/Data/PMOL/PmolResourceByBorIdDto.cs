using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolResourceByBorIdDto
{
    public string CorporateProductCatalogId { get; set; }
    public double Required { get; set; }
    public string Title { get; set; }
    public string Unit { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
}

public class ResourceByBorIdDto
{
    public IEnumerable<PmolResourceByBorIdDto> Consumable { get; set; }
    public IEnumerable<PmolResourceByBorIdDto> Material { get; set; }
    public IEnumerable<PmolResourceByBorIdDto> Tools { get; set; }
    public IEnumerable<PmolResourceByBorIdDto> Labour { get; set; }
}

public class PmolResourceCreateDto
{
    public string Id { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public double Required { get; set; }
    public double ConsumedQuantity { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
    public string Environment { get; set; }
    public string PmolId { get; set; }
    public string Type { get; set; }
    public string ResourceNumber { get; set; }
    public List<PmolTeamRoleCreateDto> TeamRoleList { get; set; }

    public string BorId { get; set; }
    public string OrganizationTeamId { get; set; }
}

public class PmolResourceCreateMobileDto
{
    public string Id { get; set; }
    public string CorporateProductCatalogId { get; set; }
    public double Required { get; set; }
    public double ConsumedQuantity { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
    public string Environment { get; set; }
    public string PmolId { get; set; }
    public string Type { get; set; }
    public string ResourceNumber { get; set; }
    public string labourId { get; set; }
    public string cabPersonId { get; set; }
    public string RoleId { get; set; }
}

public class ResourceCreateDto
{
    public IEnumerable<PmolResourceCreateDto> Consumable { get; set; }
    public IEnumerable<PmolResourceCreateDto> Material { get; set; }
    public IEnumerable<PmolResourceCreateDto> Tools { get; set; }
    public IEnumerable<PmolResourceCreateDto> Labour { get; set; }
}

public class PmolResourceReadDto
{
    public string CorporateProductCatalogId { get; set; }
    public double Required { get; set; }
    public string Title { get; set; }
    public string Unit { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
    public double ConsumedQuantity { get; set; }
    public string Id { get; set; }
    public string PmolId { get; set; }
    public string ResourceNumber { get; set; }

    public string Type { get; set; }
    public IEnumerable<PmolTeamRoleReadDto> Team { get; set; }
}

public class PmolResourceLabourDto
{
    public string CorporateProductCatalogId { get; set; }
    public double Required { get; set; }
    public string Title { get; set; }
    public string Unit { get; set; }
    public string CpcBasicUnitOfMeasureId { get; set; }
    public double ConsumedQuantity { get; set; }
    public string Id { get; set; }
    public string PmolId { get; set; }
    public string ResourceNumber { get; set; }

    public List<PmolTeamRoleReadDto> Team { get; set; }
}

public class PmolResourceReadAllDto
{
    public IEnumerable<PmolResourceReadDto> Consumable { get; set; }
    public IEnumerable<PmolResourceReadDto> Material { get; set; }
    public IEnumerable<PmolResourceReadDto> Tools { get; set; }
    public IEnumerable<PmolResourceReadDto> Labour { get; set; }
}

public class PmolTime
{
    public DateTime StartDateTimeRoundNearest { get; set; }
    public DateTime EndDateTimeRoundNearest { get; set; }
    public int dif { get; set; }
}

public class PmolBreakTime
{
    public DateTime StartDateTimeRoundNearest { get; set; }
    public DateTime EndDateTimeRoundNearest { get; set; }
    public int dif { get; set; }
    public DateTime TotalTime { get; set; }
}