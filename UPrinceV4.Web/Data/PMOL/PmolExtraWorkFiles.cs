using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolExtraWorkFiles
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }

    public string Length { get; set; }

    //Type = 1 -> image
    //Type = 2->audio
    public string Type { get; set; }
    [ForeignKey("PmolExtraWork")] public string PmolExtraWorkId { get; set; }
    public virtual PmolExtraWork PmolExtraWork { get; set; }
}

public class PmolExtraWorkFilesCreateDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public string Type { get; set; }
    public string PmolExtraWorkId { get; set; }
}

public class PmolExtraWorkFilesReadDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string PmolExtraWorkId { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public string Length { get; set; }
}