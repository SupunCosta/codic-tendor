using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PMOL;

public class PmolPersonCommentCard
{
    public string Id { get; set; }
    public string Creater { get; set; }
    public string ActivityType { get; set; }
    public string ActivityName { get; set; }
    public string Comments { get; set; }
    public DateTime? Date { get; set; }
    public string IsAccept { get; set; }
    public string PmolId { get; set; }
    public string CabPersonId { get; set; }
}

public class PmolPersonCommentCardDto
{
    public string Id { get; set; }
    public string CreaterName { get; set; }
    public string Creater { get; set; }
    public string ActivityType { get; set; }
    public string ActivityName { get; set; }
    public string Comments { get; set; }
    public DateTime? Date { get; set; }
    public string IsAccept { get; set; }
    public string PmolId { get; set; }
    public string CabPersonId { get; set; }
    public string CabPersonName { get; set; }

    public List<PmolPersonCommentDto> PersonComment { get; set; }
}

public class Creater
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class PmolStatusUpdateDto
{
    public string SequenceCode { get; set; }
    public string ProjectId { get; set; }
    public string ProjectMoleculeId { get; set; }
    public string StatusId { get; set; }
}

public class PmolPerson
{
    public List<string> CabPersonId { get; set; }
    public DateTime Date { get; set; }  
    
    public string BuId { get; set; }

}