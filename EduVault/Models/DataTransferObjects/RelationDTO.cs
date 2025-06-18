using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Xml.Linq;

namespace EduVault.Models.DataTransferObjects
{
    public class RelationDTO
    {
        public long Id { get; set; }
        public long SourceRecordId { get; set; }
        public long TargetRecordId { get; set; }
        public RelationDTO(Relation relation)
        {
            Id = relation.Id;
            SourceRecordId = relation.SourceRecord;
            TargetRecordId = relation.TargetRecord;
        }
        public RelationDTO() { }
    }
}
