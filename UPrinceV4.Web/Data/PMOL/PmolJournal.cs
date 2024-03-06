using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolJournal
{
    public string Id { get; set; }
    public string DoneWork { get; set; }
    public string EncounteredProblem { get; set; }
    public string LessonsLearned { get; set; }
    public string ReportedThings { get; set; }
    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }
}

public class PmolJournalCreateDto
{
    public string Id { get; set; }
    public string DoneWork { get; set; }
    public string EncounteredProblem { get; set; }
    public string LessonsLearned { get; set; }
    public string ReportedThings { get; set; }
    public string PmolId { get; set; }
    public string WhatsLeftToDo { get; set; }

    public IEnumerable<PmolJournalPictureCreateDto> PictureList { get; set; }
}