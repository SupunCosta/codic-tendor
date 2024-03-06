using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolAssignTime
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string PmolId { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string ProjectSequenceId { get; set; }
    public string AssignTime { get; set; }

    [NotMapped]
    public string ProjectTitle { get; set; }

}