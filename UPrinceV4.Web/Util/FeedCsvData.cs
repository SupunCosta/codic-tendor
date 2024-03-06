using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Util;

public class FeedCsvData
{
    public string LoadDataToDatabase(ApplicationDbContext _context, string url, string fileName)
    {
        try
        {
            var result = "";
            var fileData = LoadDataToTwoDArray(url);
            var rowLength = fileData.GetLength(0);
            var colLength = fileData.GetLength(1);
            string localeCode = null;
            switch (fileName)
            {
                case "ProjectType.csv":
                    DeleteRelatedDataInDb(_context, "ProjectType");
                    for (var i = 1; i < colLength; i++)
                    for (var j = 0; j < rowLength; j++)
                        if (j == 0)
                        {
                            var projectType = new ProjectType
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = fileData[j, i]
                                //LocaleCode = GetLocaleCode()
                            };
                            var defaultValue = fileData[rowLength - 1, i];
                            if (defaultValue.ToLower().Equals("true"))
                                projectType.IsDefault = true;
                            else if (defaultValue.ToLower().Equals("false")) projectType.IsDefault = false;

                            _context.ProjectType.Add(projectType);
                            _context.SaveChanges();
                            //localeCode = projectType.LocaleCode;
                        }
                        else
                        {
                            var localizedData = new LocalizedData
                            {
                                Label = fileData[j, i],
                                LocaleCode = localeCode,
                                LanguageCode = fileData[j, 0]
                            };
                            _context.LocalizedData.Add(localizedData);
                            _context.SaveChanges();
                        }

                    result += "ProjectType, ";
                    break;
                case "ProjectState.csv":
                    DeleteRelatedDataInDb(_context, "ProjectState");
                    for (var i = 1; i < colLength; i++)
                    for (var j = 0; j < rowLength; j++)
                        if (j == 0)
                        {
                            var projectState = new ProjectState
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = fileData[j, i],
                                LocaleCode = GetLocaleCode()
                            };
                            var defaultValue = fileData[rowLength - 1, i];
                            if (defaultValue.ToLower().Equals("true"))
                                projectState.IsDefault = true;
                            else if (defaultValue.ToLower().Equals("false")) projectState.IsDefault = false;

                            _context.ProjectState.Add(projectState);
                            _context.SaveChanges();
                            localeCode = projectState.LocaleCode;
                        }
                        else
                        {
                            var localizedData = new LocalizedData
                            {
                                Label = fileData[j, i],
                                LocaleCode = localeCode,
                                LanguageCode = fileData[j, 0]
                            };
                            _context.LocalizedData.Add(localizedData);
                            _context.SaveChanges();
                        }

                    result += "ProjectState, ";
                    break;
                case "ProjectManagementLevel.csv":
                    DeleteRelatedDataInDb(_context, "ProjectManagementLevel");
                    for (var i = 1; i < colLength; i++)
                    for (var j = 0; j < rowLength; j++)
                        if (j == 0)
                        {
                            var projectManagementLevel = new ProjectManagementLevel
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = fileData[j, i]
                                //LocaleCode = GetLocaleCode()
                            };
                            var defaultValue = fileData[rowLength - 1, i];
                            if (defaultValue.ToLower().Equals("true"))
                                projectManagementLevel.IsDefault = true;
                            else if (defaultValue.ToLower().Equals("false")) projectManagementLevel.IsDefault = false;

                            _context.ProjectManagementLevel.Add(projectManagementLevel);
                            _context.SaveChanges();
                            //localeCode = projectManagementLevel.LocaleCode;
                        }
                        else
                        {
                            var localizedData = new LocalizedData
                            {
                                Label = fileData[j, i],
                                LocaleCode = localeCode,
                                LanguageCode = fileData[j, 0]
                            };
                            _context.LocalizedData.Add(localizedData);
                            _context.SaveChanges();
                        }

                    result += "ProjectManagementLevel, ";
                    break;
                case "ProjectTemplate.csv":
                    DeleteRelatedDataInDb(_context, "ProjectTemplate");
                    for (var i = 1; i < colLength; i++)
                    for (var j = 0; j < rowLength; j++)
                        if (j == 0)
                        {
                            var projectTemplate = new ProjectTemplate
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = fileData[j, i]
                                //LocaleCode = GetLocaleCode()
                            };
                            _context.ProjectTemplate.Add(projectTemplate);
                            _context.SaveChanges();
                            //localeCode = projectTemplate.LocaleCode;
                        }
                        else
                        {
                            var localizedData = new LocalizedData
                            {
                                Label = fileData[j, i],
                                LocaleCode = localeCode,
                                LanguageCode = fileData[j, 0]
                            };
                            _context.LocalizedData.Add(localizedData);
                            _context.SaveChanges();
                        }

                    result += "ProjectTemplate, ";
                    break;
                case "ProjectToleranceState.csv":
                    DeleteRelatedDataInDb(_context, "ProjectToleranceState");
                    for (var i = 1; i < colLength; i++)
                    for (var j = 0; j < rowLength; j++)
                        if (j == 0)
                        {
                            var projectToleranceState = new ProjectToleranceState
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = fileData[j, i]
                                //LocaleCode = GetLocaleCode()
                            };
                            var defaultValue = fileData[rowLength - 1, i];
                            if (defaultValue.ToLower().Equals("true"))
                                projectToleranceState.IsDefault = true;
                            else if (defaultValue.ToLower().Equals("false")) projectToleranceState.IsDefault = false;

                            _context.ProjectToleranceState.Add(projectToleranceState);
                            _context.SaveChanges();
                            //localeCode = projectToleranceState.LocaleCode;
                        }
                        else
                        {
                            var localizedData = new LocalizedData
                            {
                                Label = fileData[j, i],
                                LocaleCode = localeCode,
                                LanguageCode = fileData[j, 0]
                            };
                            _context.LocalizedData.Add(localizedData);
                            _context.SaveChanges();
                        }

                    result += "ProjectToleranceState, ";
                    break;
                case "TimeClockActivityType.csv":
                    DeleteRelatedDataInDb(_context, "TimeClockActivityType");
                    for (var i = 1; i < colLength; i++)
                    for (var j = 0; j < rowLength; j++)
                        if (j == 0)
                        {
                            var timeClockActivityType = new TimeClockActivityType
                            {
                                Type = fileData[j, i],
                                LocaleCode = GetLocaleCode()
                            };
                            timeClockActivityType.TypeId = timeClockActivityType.Type.ToLower() switch
                            {
                                "travel" => 0,
                                "work" => 1,
                                "unload" => 2,
                                "personal" => 3,
                                "start" => 4,
                                "stop" => 5,
                                "break" => 6,
                                _ => 0
                            };
                            _context.TimeClockActivityType.Add(timeClockActivityType);
                            _context.SaveChanges();
                            localeCode = timeClockActivityType.LocaleCode;
                        }
                        else
                        {
                            var localizedData = new LocalizedData
                            {
                                Label = fileData[j, i],
                                LocaleCode = localeCode,
                                LanguageCode = fileData[j, 0]
                            };
                            _context.LocalizedData.Add(localizedData);
                            _context.SaveChanges();
                        }

                    result += "TimeClockActivityType, ";
                    break;
                case "WorkflowState.csv":
                    DeleteRelatedDataInDb(_context, "WorkflowState");
                    for (var i = 1; i < colLength; i++)
                    for (var j = 0; j < rowLength; j++)
                        if (j == 0)
                        {
                            var workflowState = new WorkflowState
                            {
                                State = fileData[j, i],
                                LocaleCode = GetLocaleCode()
                            };
                            _context.WorkflowState.Add(workflowState);
                            _context.SaveChanges();
                            localeCode = workflowState.LocaleCode;
                        }
                        else
                        {
                            var localizedData = new LocalizedData
                            {
                                Label = fileData[j, i],
                                LocaleCode = localeCode,
                                LanguageCode = fileData[j, 0]
                            };
                            _context.LocalizedData.Add(localizedData);
                            _context.SaveChanges();
                        }

                    result += "WorkflowState, ";
                    break;
                case "Country.csv":
                    for (var i = 1; i < colLength; i++)
                    for (var j = 0; j < rowLength; j++)
                        if (j == 0)
                        {
                            var country = new Country
                            {
                                Id = Guid.NewGuid().ToString(),
                                CountryName = fileData[j, i],
                                LocaleCode = GetLocaleCode()
                            };
                            _context.Country.Add(country);
                            _context.SaveChanges();
                            localeCode = country.LocaleCode;
                        }
                        else
                        {
                            var localizedData = new LocalizedData
                            {
                                Label = fileData[j, i],
                                LocaleCode = localeCode,
                                LanguageCode = fileData[j, 0]
                            };
                            _context.LocalizedData.Add(localizedData);
                            _context.SaveChanges();
                        }

                    result += "Country, ";
                    break;
                case "Salutation.csv":
                    //for (var i = 1; i < colLength; i++)
                    //{
                    //    for (var j = 0; j < rowLength; j++)
                    //    {
                    //        if (j == 0)
                    //        {
                    //            var salutation = new CabSalutation()
                    //            {
                    //                Id = Guid.NewGuid().ToString(),
                    //                SalutationName = fileData[j, i],
                    //                LocaleCode = GetLocaleCode()
                    //            };
                    //            _context.CabSalutation.Add(salutation);
                    //            _context.SaveChanges();
                    //            localeCode = salutation.LocaleCode;
                    //        }
                    //        else
                    //        {
                    //            var localizedData = new LocalizedData()
                    //            {
                    //                Label = fileData[j, i],
                    //                LocaleCode = localeCode,
                    //                LanguageCode = fileData[j, 0]
                    //            };
                    //            _context.LocalizedData.Add(localizedData);
                    //            _context.SaveChanges();
                    //        }
                    //    }
                    //}
                    //result += "Salutation, ";
                    break;
            }

            result += "files have been added.";
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private string[,] LoadDataToTwoDArray(string url)
    {
        var req = (HttpWebRequest)WebRequest.Create(url);
        var resp = (HttpWebResponse)req.GetResponse();

        var line = "";
        var lines = new List<string>();
        var sr = new StreamReader(resp.GetResponseStream());
        while ((line = sr.ReadLine()) != null) lines.Add(line);

        var results = sr.ReadToEnd();
        sr.Close();

        if (lines != null && lines.Count > 0)
        {
            var columnLength = lines[0].Split(',').Length;
            var data = new string[columnLength, lines.Count];
            for (var i = 0; i < lines.Count; i++)
            {
                var columns = lines[i].Split(',');
                for (var j = 0; j < columns.Length; j++) data[j, i] = columns[j];
            }

            return data;
        }

        return null;
    }

    private void DeleteRelatedDataInDb(ApplicationDbContext _context, string tableName)
    {
        //switch (tableName)
        //{
        //    case "ProjectType":
        //        var projectTypes = _context.ProjectType.ToList();
        //        foreach (ProjectType pt in projectTypes)
        //        {
        //            var relatedLocalizedData = _context.LocalizedData.Where(ld => ld.LocaleCode == pt.LocaleCode).ToList();
        //            pt.IsDeleted = true;
        //            _context.ProjectType.Update(pt);
        //            _context.SaveChanges();
        //        }
        //        break;
        //    case "ProjectState":
        //        var projectStates = _context.ProjectState.ToList();
        //        foreach (ProjectState ps in projectStates)
        //        {
        //            var relatedLocalizedData = _context.LocalizedData.Where(ld => ld.LocaleCode == ps.LocaleCode).ToList();
        //            ps.IsDeleted = true;
        //            _context.ProjectState.Update(ps);
        //            _context.SaveChanges();
        //        }
        //        break;
        //    case "ProjectManagementLevel":
        //        var projectManagementLevels = _context.ProjectManagementLevel.ToList();
        //        foreach (ProjectManagementLevel pm in projectManagementLevels)
        //        {
        //            var relatedLocalizedData = _context.LocalizedData.Where(ld => ld.LocaleCode == pm.LocaleCode).ToList();
        //            pm.IsDeleted = true;
        //            _context.ProjectManagementLevel.Update(pm);
        //            _context.SaveChanges();
        //        }
        //        break;
        //    case "ProjectTemplate":
        //        var projectTemplates = _context.ProjectTemplate.ToList();
        //        foreach (ProjectTemplate pt in projectTemplates)
        //        {
        //            var relatedLocalizedData = _context.LocalizedData.Where(ld => ld.LocaleCode == pt.LocaleCode).ToList();
        //            pt.IsDeleted = true;
        //            _context.ProjectTemplate.Update(pt);
        //            _context.SaveChanges();
        //        }
        //        break;
        //    case "ProjectToleranceState":
        //        var projectToleranceStates = _context.ProjectToleranceState.ToList();
        //        foreach (ProjectToleranceState pts in projectToleranceStates)
        //        {
        //            var relatedLocalizedData = _context.LocalizedData.Where(ld => ld.LocaleCode == pts.LocaleCode).ToList();
        //            pts.IsDeleted = true;
        //            _context.ProjectToleranceState.Update(pts);
        //            _context.SaveChanges();
        //        }
        //        break;
        //    case "TimeClockActivityType":
        //        var timeClockActivityTypes = _context.TimeClockActivityType.ToList();
        //        foreach (TimeClockActivityType tcp in timeClockActivityTypes)
        //        {
        //            var relatedLocalizedData = _context.LocalizedData.Where(ld => ld.LocaleCode == tcp.LocaleCode).ToList();
        //            tcp.IsDeleted = true;
        //            _context.TimeClockActivityType.Update(tcp);
        //            _context.SaveChanges();
        //        }
        //        break;
        //    case "WorkflowState":
        //        var workflowStates = _context.WorkflowState.ToList();
        //        foreach (WorkflowState ws in workflowStates)
        //        {
        //            var relatedLocalizedData = _context.LocalizedData.Where(ld => ld.LocaleCode == ws.LocaleCode).ToList();
        //            ws.IsDeleted = true;
        //            _context.WorkflowState.Update(ws);
        //            _context.SaveChanges();
        //        }
        //        break;
        //    default:
        //        break;
        //}           
    }

    private string GetLocaleCode()
    {
        var localeCode = Guid.NewGuid().ToString();
        return localeCode;
    }
}