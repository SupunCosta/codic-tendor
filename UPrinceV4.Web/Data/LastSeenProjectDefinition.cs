using System;

namespace UPrinceV4.Web.Data;

public class LastSeenProjectDefinition
{
    public int Id { get; set; }
    public string ProjectId { get; set; }
    public string ViewedUserId { get; set; }
    public DateTime ViewTime { get; set; }
}