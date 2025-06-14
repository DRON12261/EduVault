using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Xml.Linq;

namespace EduVault.Models.DataTransferObjects
{
    public class AccessRightsDTO
    {
        public long Id { get; set; }
        public long RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientType { get; set; }
        public long RightTypeId { get; set; }
        public string RightTypeName { get; set; }
        public AccessRightsDTO(AccessRights accessRights)
        {
            Id = accessRights.Id;
            RecipientId = accessRights.Recipient.Id;
            RightTypeId = accessRights.ARType.Id;
        }
        public AccessRightsDTO() { }
    }
}
