using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.WBS
{
    public class WbsConversation
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string ItemId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? DateTime { get; set; }
        public string FromPersonId { get; set; }


    }

    public class WbsConversationDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string ItemId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? DateTime { get; set; }
        public string From { get; set; }
        public string FromPersonId { get; set; }
        public string FromPersonName { get; set; }
        public List<WbsConversationAttachments> Attachments { get; set; }
        public List<WbsConversationCc> Cc { get; set; }
        public List<WbsConversationTo> ToPerson { get; set; }

    }
    
    public class GetConversationDto
    {
        public string Task { get; set; }
        public List<WbsConversationDto> Conversations { get; set; }

    }
}
