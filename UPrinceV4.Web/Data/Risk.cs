namespace UPrinceV4.Web.Data;

public class Risk : BaseEntity
{
    public string RiskDetails { get; set; }
    public string RiskTypeId { get; set; }
    public virtual RiskType RiskType { get; set; }

    public string PersonId { get; set; }

    // public virtual CabPerson Person { get; set; }
    public string RiskStatusId { get; set; }

    public virtual RiskStatus RiskStatus { get; set; }
    // [JsonIgnore]
    //public string ProjectDefinitionId { get; set; }
    //[JsonIgnore]
    //public ProjectDefinition ProjectDefinition { get; set; }
}

public class RiskDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string RiskDetails { get; set; }
    public string RiskTypeId { get; set; }
    public string CabPersonId { get; set; }
    public string RiskStatusId { get; set; }
    public string ProjectId { get; set; }
}

public class RiskPersonDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
}

public class RiskReadDtoDapper
{
    public RiskReadDtoDapper()
    {
        RiskType = new RiskTypeDto();
        RiskStatus = new RiskStatusDto();
    }

    public string Id { get; set; }
    public string PbsRiskId { get; set; }
    public string SequenceCode { get; set; }
    public string Title { get; set; }
    public string HeaderTitle { get; set; }
    public string Name { get; set; }
    public string RiskDetails { get; set; }
    public string PersonId { get; set; }
    public RiskTypeDto RiskType { get; set; }
    public RiskPersonDto Person { get; set; }
    public RiskStatusDto RiskStatus { get; set; }

    public RiskHistoryLogDto RiskHistoryLog { get; set; }
}

public class RiskReadDto
{
    public RiskReadDto()
    {
        RiskType = new RiskType();
        RiskStatus = new RiskStatus();
    }

    public string Id { get; set; }
    public string SequenceCode { get; set; }
    public string Title { get; set; }
    public string HeaderTitle { get; set; }
    public string Name { get; set; }
    public string RiskDetails { get; set; }
    public RiskType RiskType { get; set; }
    public RiskPersonDto Person { get; set; }
    public RiskStatus RiskStatus { get; set; }
    public RiskHistoryLogDto historyLog { get; set; }
}

public class RiskReadDapperDto : BaseEntity
{
    public RiskReadDapperDto()
    {
        RiskType = new RiskTypeDapperDto();
        RiskStatus = new RiskStatusDapperDto();
    }

    public string PbsRiskId { get; set; }
    public string RiskDetails { get; set; }
    public string CabPersonId { get; set; }
    public RiskTypeDapperDto RiskType { get; set; }
    public RiskPersonDto Person { get; set; }
    public RiskStatusDapperDto RiskStatus { get; set; }
}