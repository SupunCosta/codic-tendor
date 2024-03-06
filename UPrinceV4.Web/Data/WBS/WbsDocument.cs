namespace UPrinceV4.Web.Data.WBS;

public class WbsDocument
{
    public string Id { get; set; }
    public string ProjectTitle { get; set; }
    public string Product { get; set; }
    public string Wbs { get; set; }
    public string FolderId { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public string FileId { get; set; }
    public string FileDownloadUrl { get; set; }
    public string AttachmentId { get; set; }
    public string SiteId { get; set; }
    public string MailId { get; set; }
    public string WbsId { get; set; }
    public string FolderPath { get; set; }
}

public class GetWbsDocumentIdByUrl
{
    public string SiteId { get; set; }
    public string ListId { get; set; }
    public string ItemId { get; set; }
}