namespace UPrinceV4.Web.Data.BMLot
{
    public class ContractorTeamList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string RoleId { get; set; }
        public bool? InvitationMail { get; set; } = false;
        public string CabPersonId { get; set; }
        public string CabPersonName { get; set; }
        public string RoleName { get; set; }
        public string LotContractorId { get; set; }       
    }
}
