using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.VisualPlan;

public class ResourceMatrix
{
    public List<string> Week { get; set; }
    public Dictionary<string, CpcDataDto> Resource { get; set; }
}

public class GetResourceMatrixDto
{
    public string Type { get; set; }
    public GetResourceMatrixFilter Filter { get; set; }
    public List<string> BusinessUnit { get; set; }

}

public class GetResourceMatrixFilter
{
    public string ViewMode { get; set; }
}
public class PbsDate
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Type { get; set; }
    
}

public class ResourceMatrixPmolData
{
    public DateTime ExecutionDate { get; set; }
    public string Label { get; set; }
    public string Title { get; set; }
    public string CpcId { get; set; }
    public int RequiredQuantity { get; set; }
}

public class ResourceMatrixPbsData
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Label { get; set; }
    public string Title { get; set; }
    public string CpcId { get; set; }
    public int Quantity { get; set; }
}

public class CpcDataDto
{
    public List<ChildData> ParentData { get; set; }
    public Dictionary<string, List<ChildData>> Childs { get; set; }
}

public class ChildData
{
    public string Hours { get; set; }
    public string Percentage { get; set; }
}

public class QuarterDto
{
    public string Quarter { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } 
}

public class PreferredCpcDto
{
    public string Id { get; set; }
    public string CpcId { get; set; }
    public int TotalDays { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class HoursWithPercentage
{
    public List<string> HoursString { get; set; }
    public List<double> HoursDouble { get; set; }
    public List<double> Percentage { get; set; }
}

public class ChildrenData
{
    public string Pearson { get; set; }
    public List<string> Hours { get; set; }
    public List<double> HoursCal { get; set; }
    public List<double> Percentage { get; set; }
}
public class ParentData
{
    public List<string> Hours { get; set; }
    public List<double> HoursCal { get; set; }
}

public class ManagerProjectData 
{
    public string Project { get; set; }
    public ParentData Parent { get; set; }
    public List<ChildrenData> Children { get; set; }
}
public class ProjectManagerData
{
    public string ProjectManager { get; set; }
    public ParentData Parent { get; set; }
    public List<ManagerProjectData> Children { get; set; }
}
public class UnAssigned
{
    public ParentData Parent { get; set; }
    public List<ChildrenData> Children { get; set; }
}
public class Resource
{
    public List<ProjectManagerData> ProjectManager { get; set; }
    public List<UnAssigned> UnAssigned { get; set; }
    public List<UnAssigned> Absent { get; set; }
}

public class LabourMatrix
{
    public List<string> Week { get; set; }
    public List<string> Days { get; set; }
    public Resource Resource { get; set; }
    public ParentData Parent { get; set; }
}

public class HoursData
{
    public List<double> HoursDouble { get; set; }
    public List<string> HoursString { get; set; }
}

public class OrganizationMatrix
{
    public List<string> Week { get; set; }
    public List<string> Days { get; set; }
    public List<OrganizationMatrixCu> Cu { get; set; }
}

public class OrganizationMatrixCu
{
    public string CuTitle { get; set; }
    public List<OrganizationMatrixBu> Bu { get; set; }
    public List<OrganizationMatrixTeam> Team { get; set; }
}
public class OrganizationMatrixBu
{
    public string BuTitle { get; set; }
    public List<OrganizationMatrixTeam> Team { get; set; }
}
public class OrganizationMatrixTeam
{
    public string TeamTitle { get; set; }
    public List<OrganizationMatrixPerson> Person { get; set; }
}
public class OrganizationMatrixPerson
{
    public string PersonId { get; set; }
    public string PersonName { get; set; }
    public List<List<string>> Project { get; set; }
}
public class WeekData
{
    public int Number { get; set; }
    public int Year { get; set; }
    public string Name { get; set; }
}


