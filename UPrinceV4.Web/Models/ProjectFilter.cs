using System;
using System.Collections.Generic;
using System.Linq;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Models;

public class ProjectFilterClass
{
    public IEnumerable<ProjectDefinition> filterByTitle(string title, IEnumerable<ProjectDefinition> projects)
    {
        projects = projects.Where(p => (p.Name + p.SequenceCode).ToLower().Contains(title.ToLower().Replace("'","''")));
        return projects;
    }

    //public IEnumerable<ProjectDefinition> filterByType(string typeId, IEnumerable<ProjectDefinition> projects)
    //{
    //    projects = projects.Where(p => p.ProjectType.Id == typeId);
    //    return projects;
    //}

    //public IEnumerable<ProjectDefinition> filterByManagementLevel(string managementLevelId, IEnumerable<ProjectDefinition> projects)
    //{
    //    projects = projects.Where(p => p.ProjectManagementLevel.Id == managementLevelId);
    //    return projects;
    //}

    //public IEnumerable<ProjectDefinition> filterByTemplate(string templateId, IEnumerable<ProjectDefinition> projects)
    //{
    //   // projects = projects.Where(p => p.ProjectTemplateId == templateId);
    //    return projects;
    //}

    //public IEnumerable<ProjectDefinition> filterByToleranceState(string toleranceStateId, IEnumerable<ProjectDefinition> projects)
    //{
    //    projects = projects.Where(p => p.ProjectToleranceState.Id == toleranceStateId);
    //    return projects;
    //}

    public IEnumerable<Shift> filterShiftByRange(DateTime endDate, DateTime startDate, IEnumerable<Shift> shifts)
    {
        if (endDate.Date == startDate.Date)
            shifts = shifts.Where(s => s.StartDateTime >= startDate && s.StartDateTime <= startDate.AddHours(24));
        else
            shifts = shifts.Where(s => s.StartDateTime >= startDate && s.StartDateTime <= endDate.AddHours(24));

        return shifts;
    }

    public IEnumerable<Shift> filterShiftByStartDate(DateTime startDate, IEnumerable<Shift> shifts)
    {
        shifts = shifts.Where(s => s.StartDateTime >= startDate && s.StartDateTime <= startDate.AddHours(24));
        return shifts;
    }

    public IEnumerable<Shift> filterShiftByUserName(string userName, IEnumerable<Shift> shifts)
    {
        shifts = shifts.Where(s =>
            s.User.Company != null
                ? (s.User.Person.FullName + s.User.Company.Name).ToLower().Contains(userName.ToLower().Replace("'","''"))
                : s.User.Person.FullName.ToLower().Contains(userName.ToLower().Replace("'","''")));
        return shifts;
    }

    public IEnumerable<Shift> filterShiftByStatus(int statusId, IEnumerable<Shift> shifts)
    {
        shifts = shifts.Where(s => s.WorkflowStateId == statusId);
        return shifts;
    }

    public IEnumerable<QRCode> filterQRByType(int type, IEnumerable<QRCode> qr)
    {
        qr = qr.Where(q => q.Type == type);
        return qr;
    }

    public IEnumerable<QRCode> filterQRByProjectId(string projectId, IEnumerable<QRCode> qr)
    {
        qr = qr.Where(q => q.ProjectId == projectId);
        return qr;
    }

    public IEnumerable<QRCode> filterQRByVehicleNo(string vehicleNo, IEnumerable<QRCode> qr)
    {
        qr = qr.Where(q => q.VehicleNo != null && q.VehicleNo.ToLower().Contains(vehicleNo.ToLower().Replace("'","''")));
        return qr;
    }

    public IEnumerable<QRCode> filterQRByLocation(string location, IEnumerable<QRCode> qr)
    {
        qr = qr.Where(q => q.Location != null && q.Location.ToLower().Contains(location.ToLower().Replace("'","''")));
        return qr;
    }

    public IEnumerable<QRCode> filterQRByDate(DateTime date, IEnumerable<QRCode> qr)
    {
        //qr = qr.Where(q => q.Date.Date==date.Date);
        qr = qr.Where(q => q.Date >= date && q.Date <= date.AddHours(24));
        return qr;
    }

    public IEnumerable<CorporateProductCatalog> filterCpcByTitle(string title,
        IEnumerable<CorporateProductCatalog> cpc)
    {
        cpc = cpc.Where(c => c.ResourceTitle.ToLower().Contains(title.ToLower().Replace("'","''")));
        return cpc;
    }

    public IEnumerable<CorporateProductCatalog> filterCpcByType(string typeId,
        IEnumerable<CorporateProductCatalog> cpc)
    {
        cpc = cpc.Where(c => c.ResourceTypeId == typeId);
        return cpc;
    }

    public IEnumerable<CorporateProductCatalog> filterCpcByFamily(string familyId,
        IEnumerable<CorporateProductCatalog> cpc)
    {
        cpc = cpc.Where(c => c.ResourceFamilyId == familyId);
        return cpc;
    }

    public IEnumerable<CorporateProductCatalog> filterCpcByResourceNumber(string resourceNumber,
        IEnumerable<CorporateProductCatalog> cpc)
    {
        cpc = cpc.Where(c => (c.ResourceNumber + c.ResourceTitle).ToLower().Contains(resourceNumber.ToLower().Replace("'","''")));
        return cpc;
    }

    public IEnumerable<CorporateProductCatalog> filterCpcByStatus(int status,
        IEnumerable<CorporateProductCatalog> cpc)
    {
        cpc = cpc.Where(c => c.Status == status);
        return cpc;
    }

    public IEnumerable<PbsProduct> filterPbsByTitle(string title, IEnumerable<PbsProduct> pbs)
    {
        pbs = pbs.Where(p => (p.ProductId + p.Name).ToLower().Contains(title.ToLower().Replace("'","''")));
        return pbs;
    }

    public IEnumerable<PbsProduct> filterPbsByType(string typeId, IEnumerable<PbsProduct> pbs)
    {
        pbs = pbs.Where(p => p.PbsProductItemTypeId == typeId);
        return pbs;
    }

    public IEnumerable<PbsProduct> filterPbsByProductStatus(string productStatusId, IEnumerable<PbsProduct> pbs)
    {
        pbs = pbs.Where(p => p.PbsProductStatusId == productStatusId);
        return pbs;
    }

    public IEnumerable<PbsProduct> filterPbsByToleranceState(string toleranceStatusId, IEnumerable<PbsProduct> pbs)
    {
        pbs = pbs.Where(p => p.PbsToleranceStateId == toleranceStatusId);
        return pbs;
    }

    public IEnumerable<PbsProduct> filterPbsByScope(string scope, IEnumerable<PbsProduct> pbs)
    {
        pbs = pbs.Where(p => p.Scope == scope);
        return pbs;
    }
}