using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolJournalPicture
{
    public string Id { get; set; }
    public string Link { get; set; }
    public string Type { get; set; }
    [ForeignKey("PmolJournal")] public string PmolJournalId { get; set; }
    public virtual PmolJournal PmolJournal { get; set; }
}

public class PmolJournalPictureCreateDto
{
    public string Id { get; set; }
    public string Link { get; set; }
    public string Type { get; set; }
}