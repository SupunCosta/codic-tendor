using System;

namespace UPrinceV4.Web.Data.StandardMails;

public class StandardMailHeader
{
    public string Id { get; set; }
    public string MailHeader { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string SequenceId { get; set; }
    public string RequestToWrittenInTender { get; set; }
    public string MeasuringStateRecieved { get; set; }
    public string Reminder1 { get; set; }
    public string Reminder1TimeFrameTender { get; set; }
    public string Reminder2 { get; set; }
    public string Reminder2TimeFrameTender { get; set; }
    public string Reminder3 { get; set; }
    public string Reminder3TimeFrameTender { get; set; }
    public string TenderWon { get; set; }
    public string TenderLost { get; set; }
    public string OutStandingComments { get; set; }
    public string Createdby { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string Modifiedby { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsDefault { get; set; }
    public string Reminder4 { get; set; }
    public string Reminder4TimeFrameTender { get; set; }
    public string AcceptTender { get; set; }
    public string SubscribeTender { get; set; }
    public string DownloadTender { get; set; }

}

public class StandardMailHeaderDto
{
    public string Id { get; set; }
    public string MailHeader { get; set; }
    public string Name { get; set; }

    public string Title { get; set; }
    public string SequenceId { get; set; }
    public string RequestToWrittenInTender { get; set; }
    public string MeasuringStateRecieved { get; set; }
    public string Reminder1 { get; set; }
    public string Reminder1TimeFrameTender { get; set; }
    public string Reminder2 { get; set; }
    public string Reminder2TimeFrameTender { get; set; }
    public string Reminder3 { get; set; }
    public string Reminder3TimeFrameTender { get; set; }
    public string TenderWon { get; set; }
    public string TenderLost { get; set; }
    public string OutStandingComments { get; set; }
    public History History { get; set; }
    public bool IsDefault { get; set; }
    public string Reminder4 { get; set; }
    public string Reminder4TimeFrameTender { get; set; }
    public string AcceptTender { get; set; }
    public string SubscribeTender { get; set; }
    public string DownloadTender { get; set; }
}

public class StandardMailFilter
{
    public string Title { get; set; }
    public Sorter Sorter { get; set; }
}

public class History
{
    public string Createdby { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string Modifiedby { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public class StandardMailDto
{
    public string Value { get; set; }
    public string Label { get; set; }
}