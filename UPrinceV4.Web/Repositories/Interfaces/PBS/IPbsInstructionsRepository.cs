using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Repositories.Interfaces.PBS;

public interface IPbsInstructionsRepository
{
    Task<IEnumerable<PbsInstruction>> GetPbsInstructionist(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<Instructions> GetPbsInstructionById(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<IEnumerable<PbsInstructionLoadDto>> GetPbsInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<IEnumerable<PbsInstructionLoadDto>> GetPictureOfInstallationInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<IEnumerable<PbsInstructionLoadDto>> GetPipingInstrumentationDiagramByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<IEnumerable<PbsInstructionLoadDto>> GetWeldingProceduresSpecificationsPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<IEnumerable<PbsInstructionLoadDto>> GetIsoMetricDrawingsPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<IEnumerable<PbsInstructionLoadDto>> GetAllPbsInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<PbsInstructionLoadAllPmolDto> GetAllPbsInstructionsByPbsProductAllTypes(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<IEnumerable<PbsInstructionLoadDto>> GetAllInstructionsByPbsProductIdAll(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);


    Task<IEnumerable<PbsInstructionLoadDto>> GetHealthSafetyEnvironmenInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<Instructions> AddPbsInstruction(PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);
    Task<bool> DeletePbsInstruction(PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<PbsInstructionsDropdown> GetPbsInstructionDropdownData(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    //IEnumerable<RiskReadDto> Filter(RiskRepositoryParameter riskRepositoryParameter);
    Task<string> UploadImage(PbsInstructionsRepositoryParameter personRepositoryParameter);

    Task<IEnumerable<Instructions>> InstructionsFilter(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<string> CreatePbsInstruction(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<bool> DeleteInstruction(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);

    Task<IEnumerable<PbsInstructionLoadDto>> ReadAllInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter);
}

public class PbsInstructionsRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public List<string> IdList { get; set; } // should pass sequence code list as id
    public Instructions PbsInstruction { get; set; }
    public string PbsInstructionId { get; set; } // should pass sequence code as id
    public PbsInstructionFamilyRepositoryParameter PbsInstructionFamilyRepositoryParameter { get; set; }
    public IPbsInstructionFamilyRepository IPbsInstructionFamilyRepository { get; set; }
    public string Lang { get; set; }
    public RiskFilterModel RiskFilterModel { get; set; }
    public IFormCollection Image { get; set; }
    public string FolderName { get; set; }
    public string PbsProductId { get; set; }
    public string InstructionType { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }

    public PbsInstructionFilter Filter { get; set; }

    public CreateInstructionDto CreateInstruction { get; set; }
}

public class PbsInstructionsDropdown
{
    public IEnumerable<PbsInstructionFamilyDapper> PbsInstructionFamily { get; set; }
}

public enum InstructionType
{
    TechnicalInstructions = 100,
    SafetyInstructions = 200,
    EnvironmentalInstructions = 300,
    HealthInstructions = 400
}