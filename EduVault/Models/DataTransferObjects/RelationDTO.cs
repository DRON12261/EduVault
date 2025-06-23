using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Xml.Linq;

namespace EduVault.Models.DataTransferObjects
{
    public class RelationDTO
    {
        public long SourceRecordId { get; set; }
        public long TargetRecordId { get; set; }
        public RelationDTO(Relation relation)
        {
            SourceRecordId = relation.SourceRecordId;
            TargetRecordId = relation.TargetRecordId;
        }
        public RelationDTO() { }
    }
}
