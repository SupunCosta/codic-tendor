namespace UPrinceV4.Web.Data.HR
{
    public class HRContractTypes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TypeId { get; set; }
        public string LanguageCode { get; set; }
    }
    
    public class GetHRContractTypes
    {
        public string Key { get; set; }
        public string Text { get; set; }
        
    }
}
