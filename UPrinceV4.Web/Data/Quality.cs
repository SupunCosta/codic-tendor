using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UPrinceV4.Web.Data;

public class Quality : BaseEntity
{
    //public string PbsRiskId { get; set; }
    //[JsonProperty(PropertyName = "-----changed-----")]
    public string Criteria { get; set; }

    //[NotMapped] public string Title => SequenceCode + " - " + Name.Substring(0, 1);
    public string Tolerance { get; set; }
    public string Method { get; set; }
    public string Skills { get; set; }

    [JsonIgnore] public string ProjectDefinitionId { get; set; }
    [JsonIgnore] public ProjectDefinition ProjectDefinition { get; set; }

    [NotMapped] public QualityHistoryLogDto QualityHistoryLogDto { get; set; }
}

public class QualityDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Criteria { get; set; }
    public string Tolerance { get; set; }
    public string Method { get; set; }
    public string Skills { get; set; }
    public string ProjectId { get; set; }
}

public class QualityDapperDto : BaseEntity
{
    public string PbsQualityId { get; set; }
    public string Criteria { get; set; }
    public string Tolerance { get; set; }
    public string Method { get; set; }
    public string Skills { get; set; }
    [JsonIgnore] public string ProjectDefinitionId { get; set; }
    [JsonIgnore] public ProjectDefinition ProjectDefinition { get; set; }
    public string Uid { get; set; }
}

public class QualityReadDtoDapper
{
    public string Id { get; set; }
    public string PbsQualityId { get; set; }
    public string Criteria { get; set; }
    public string Tolerance { get; set; }
    public string Method { get; set; }
    public string Skills { get; set; }
    public QualityHistoryLogDto QualityHistoryLog { get; set; }
}