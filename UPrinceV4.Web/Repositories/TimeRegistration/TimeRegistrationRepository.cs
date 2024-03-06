using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.GD.Vehicle;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.GD;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.TimeRegistration;

public class TimeRegistrationRepository : ITimeRegistrationRepository
{
    public async Task<List<PmolCpcData>> GetLabourPmolVehicalesPositions(
        TimeRegistrationParameter TimeRegistrationParameter)
    {
        var pmolList = new List<PmolCpcData>();
        var results = new List<PmolCpcData>();


        var projectManager =
            TimeRegistrationParameter.IPmolRepository.ProjectPm(TimeRegistrationParameter.TenantProvider.GetTenant()
                .ConnectionString);

        foreach (var project in projectManager)
        {
            await using var connection = new SqlConnection(project.ProjectConnectionString);


            pmolList = connection.Query<PmolCpcData>(@"SELECT
                                  PMol.ProjectMoleculeId
                                 ,PmolTeamRole.CabPersonId
                                 ,PMol.Name
                                 ,PMol.Id
                                 ,PMol.ExecutionEndTime
                                 ,PMol.ExecutionStartTime
                                 ,PMol.ExecutionDate
                                 ,PMol.Title
                                 ,PMol.ProjectSequenceCode
                                 ,pmol.TypeId
                                FROM dbo.PMolPlannedWorkLabour
                                LEFT OUTER JOIN dbo.PMol
                                  ON PMolPlannedWorkLabour.PmolId = PMol.Id
                                LEFT OUTER JOIN dbo.PmolTeamRole
                                  ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id
                                WHERE PMolPlannedWorkLabour.IsDeleted = 0
                                AND PmolTeamRole.IsDeleted = 0
                                AND PmolTeamRole.CabPersonId  = @cabPersonId 
                                AND ExecutionDate = @ExecutionDate 
                                AND TypeId = '3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1'
                                GROUP BY PMol.ProjectMoleculeId
                                        ,PmolTeamRole.CabPersonId
                                        ,PMol.Name
                                        ,PMol.Id
                                        ,PMol.ExecutionEndTime
                                        ,PMol.ExecutionStartTime
                                        ,PMol.ExecutionDate
                                        ,PMol.Title
                                        ,PMol.ProjectSequenceCode
                                        ,pmol.TypeId
                                ORDER BY PMol.ProjectMoleculeId DESC",
                new
                {
                    cabPersonId = TimeRegistrationParameter.GetLabourPmolVehicalesPositionsDto.PersonId,
                    TimeRegistrationParameter.GetLabourPmolVehicalesPositionsDto.ExecutionDate
                }).ToList();

            var vehicalList = connection.Query<PmolVehicle>(
                @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber,PMolPlannedWorkTools.PmolId , CpcVehicleTrackingNo.ResourceId, CpcVehicleTrackingNo.TrackingNo FROM dbo.PMolPlannedWorkTools 
                                                                                    LEFT OUTER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id
                                                                                    LEFT OUTER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id 
                                                                                    LEFT OUTER JOIN CpcVehicleTrackingNo ON CpcVehicleTrackingNo.CpcId = CorporateProductCatalog.Id
                                                                                    WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMolPlannedWorkTools.IsDeleted = 0");


            foreach (var pmol in pmolList)
            {
                // foreach (var pmol in mm.Pmol)
                // {
                //     pmol.ProjectManager = manager.FullName;
                //     pmol.ProjectSequenceCode = manager.SequenceCode;
                //     DateTime sDate = Convert.ToDateTime(pmol.ExecutionStartTime);
                //     DateTime eDate = Convert.ToDateTime(pmol.ExecutionEndTime);
                //     pmol.ExecutionStartTime = sDate.ToString("HH:MM");
                //     pmol.ExecutionEndTime = eDate.ToString("HH:MM");
                // }
                pmol.PmolVehical = vehicalList.Where(x => x.PmolId == pmol.Id).ToList();

                foreach (var vehicle in pmol.PmolVehical)
                    if (vehicle.ResourceId != null)
                    {
                        var gdParameter = new GDParameter
                        {
                            VehiclePositionDto = new VehiclePositionDto
                            {
                                Date = pmol.ExecutionDate,
                                ResourceId = vehicle.ResourceId
                            }
                        };

                        //var position = await TimeRegistrationParameter.IGDRepository.GetVehiclePosition(gdParameter);
                        var status = await TimeRegistrationParameter.IGDRepository.GetVehicleStatus(gdParameter);

                        var statusList = new List<VtsMapData>();
                        foreach (var item in status)
                        {
                            if (item.StartLocation != null)
                            {
                                var sItem = new VtsMapData
                                {
                                    Latitude = item.StartLocation.Latitude,
                                    Longitude = item.StartLocation.Longitude,
                                    City = item.StartLocation.Address.ShortAddressLine
                                };

                                statusList.Add(sItem);
                            }

                            if (item.StopLocation != null)
                            {
                                var sItem = new VtsMapData
                                {
                                    Latitude = item.StopLocation.Latitude,
                                    Longitude = item.StopLocation.Longitude,
                                    City = item.StopLocation.Address.ShortAddressLine
                                };

                                statusList.Add(sItem);
                            }
                        }

                        vehicle.Positions = statusList;
                    }
            }

            results.AddRange(pmolList);
        }

        // var dDate = "2023-02-17";
        // var gdParameter1 = new GDParameter()
        // {
        //     VehiclePositionDto = new VehiclePositionDto()
        //     {
        //         Date = DateTime.ParseExact(dDate,"yyyy-MM-dd",CultureInfo.CurrentCulture),
        //         ResourceId = "4785aa97-d316-43d8-9c13-82c2cc27f7b9"
        //     }
        // };
        // var position1 = await TimeRegistrationParameter.IGDRepository.GetVehiclePosition(gdParameter1);


        return results;
    }

    public async Task<List<VtsData>> GetVtsDataByPerson(TimeRegistrationParameter TimeRegistrationParameter)
    {
        try
        {
            //var result = await GetLabourPmolVehicalesPositions(TimeRegistrationParameter);
            var pmolList = new List<PmolCpcData>();
            var results = new List<PmolCpcData>();

            var connectionString = ConnectionString.MapConnectionString(
                TimeRegistrationParameter.ContractingUnitSequenceId,
                TimeRegistrationParameter.ProjectSequenceId, TimeRegistrationParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            pmolList = connection.Query<PmolCpcData>(@"SELECT
                                  PMol.ProjectMoleculeId
                                 ,PmolTeamRole.CabPersonId
                                 ,PMol.Name
                                 ,PMol.Id
                                 ,PMol.ExecutionEndTime
                                 ,PMol.ExecutionStartTime
                                 ,PMol.ExecutionDate
                                 ,PMol.Title
                                 ,PMol.ProjectSequenceCode
                                 ,pmol.TypeId
                                FROM dbo.PMolPlannedWorkLabour
                                LEFT OUTER JOIN dbo.PMol
                                  ON PMolPlannedWorkLabour.PmolId = PMol.Id
                                LEFT OUTER JOIN dbo.PmolTeamRole
                                  ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id
                                WHERE PMolPlannedWorkLabour.IsDeleted = 0
                                AND PmolTeamRole.IsDeleted = 0
                                AND PmolTeamRole.CabPersonId  = @cabPersonId 
                                AND ExecutionDate = @ExecutionDate 
                                AND TypeId = '3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1'
                                GROUP BY PMol.ProjectMoleculeId
                                        ,PmolTeamRole.CabPersonId
                                        ,PMol.Name
                                        ,PMol.Id
                                        ,PMol.ExecutionEndTime
                                        ,PMol.ExecutionStartTime
                                        ,PMol.ExecutionDate
                                        ,PMol.Title
                                        ,PMol.ProjectSequenceCode
                                        ,pmol.TypeId
                                ORDER BY PMol.ProjectMoleculeId DESC",
                new
                {
                    cabPersonId = TimeRegistrationParameter.GetLabourPmolVehicalesPositionsDto.PersonId,
                    TimeRegistrationParameter.GetLabourPmolVehicalesPositionsDto.ExecutionDate
                }).ToList();

            var vehicalList = connection.Query<PmolVehicle>(
                @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber,PMolPlannedWorkTools.PmolId , CpcVehicleTrackingNo.ResourceId, CpcVehicleTrackingNo.TrackingNo FROM dbo.PMolPlannedWorkTools 
                                                                                    LEFT OUTER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id
                                                                                    LEFT OUTER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id 
                                                                                    LEFT OUTER JOIN CpcVehicleTrackingNo ON CpcVehicleTrackingNo.CpcId = CorporateProductCatalog.Id
                                                                                    WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMolPlannedWorkTools.IsDeleted = 0");


            foreach (var pmol in pmolList)
            {
                // foreach (var pmol in mm.Pmol)
                // {
                //     pmol.ProjectManager = manager.FullName;
                //     pmol.ProjectSequenceCode = manager.SequenceCode;
                //     DateTime sDate = Convert.ToDateTime(pmol.ExecutionStartTime);
                //     DateTime eDate = Convert.ToDateTime(pmol.ExecutionEndTime);
                //     pmol.ExecutionStartTime = sDate.ToString("HH:MM");
                //     pmol.ExecutionEndTime = eDate.ToString("HH:MM");
                // }
                pmol.PmolVehical = vehicalList.Where(x => x.PmolId == pmol.Id).ToList();

                foreach (var vehicle in pmol.PmolVehical)
                    if (vehicle.ResourceId != null)
                    {
                        var gdParameter = new GDParameter
                        {
                            VehiclePositionDto = new VehiclePositionDto
                            {
                                Date = pmol.ExecutionDate,
                                ResourceId = vehicle.ResourceId
                            }
                        };

                        //var position = await TimeRegistrationParameter.IGDRepository.GetVehiclePosition(gdParameter);
                        var status = await TimeRegistrationParameter.IGDRepository.GetVehicleStatus(gdParameter);

                        //vehicle.Positions = position;
                        vehicle.VehicleStatus = status;
                    }
            }

            results.AddRange(pmolList);

            var vtsData = new List<VtsData>();

            foreach (var i in results)
            foreach (var n in i.PmolVehical)
                //var speedList = n.Positions.Where(e => e.Speed == 0).OrderBy(e => e.RtcDateTime).ToList();
            foreach (var r in n.VehicleStatus)
            {
                var vts = new VtsData
                {
                    CoperateProductCatalogId = n.CoperateProductCatalogId,
                    Title = r.StateName
                };

                var sPosition = new Position
                {
                    Lat = r.StartLocation.Latitude.ToString(CultureInfo.InvariantCulture),
                    Lon = r.StartLocation.Longitude.ToString(CultureInfo.InvariantCulture)
                };

                var ePosition = new Position
                {
                    Lat = r.StopLocation.Latitude.ToString(CultureInfo.InvariantCulture),
                    Lon = r.StopLocation.Longitude.ToString(CultureInfo.InvariantCulture)
                };

                vts.StartTime = r.Start.DateTime;
                vts.StartPoint = sPosition;
                vts.EndTime = r.Stop.DateTime;
                vts.Destination = ePosition;

                vtsData.Add(vts);
            }

            // if (!start)
            // {
            //     if (lastSpeed < r.Speed)
            //     {
            //         vts.CoperateProductCatalogId = n.CoperateProductCatalogId;
            //         vts.Title = n.Title;
            //         var sPosition = new Position()
            //         {
            //             Lat = r.Latitude.ToString(CultureInfo.InvariantCulture),
            //             Lon = r.Longitude.ToString(CultureInfo.InvariantCulture)
            //         };
            //     
            //         vts.StartTime = r.RtcDateTime.UtcDateTime;
            //         vts.StartPoint = sPosition;
            //         vts.Speed1 = r.Speed;
            //
            //         start = true;
            //         lastSpeed = r.Speed;
            //     }
            // }
            // if(start)
            // {
            //     if (r.Speed == 0)
            //     {
            //         var ePosition = new Position()
            //         {
            //             Lat = r.Latitude.ToString(CultureInfo.InvariantCulture),
            //             Lon = r.Longitude.ToString(CultureInfo.InvariantCulture)
            //         };
            //
            //         var sPosition = new Position()
            //         {
            //             Lat = vts.StartPoint.Lat,
            //             Lon = vts.StartPoint.Lon
            //         };
            //         var mVts = new VtsData()
            //         {
            //             CoperateProductCatalogId = vts.CoperateProductCatalogId,
            //             Title = vts.Title,
            //             StartTime = vts.StartTime,
            //             StartPoint = sPosition,
            //             EndTime = r.RtcDateTime.UtcDateTime,
            //             Destination = ePosition,
            //             Speed2 = r.Speed
            //         };
            //         vtsData.Add(mVts);
            //
            //         start = false;
            //     }
            // }
            return vtsData;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Vehicle>> GetVehicles(TimeRegistrationParameter TimeRegistrationParameter)
    {
        var gdParameter = new GDParameter();

        return await TimeRegistrationParameter.IGDRepository.GetVehicles(gdParameter);
    }
}