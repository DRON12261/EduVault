﻿using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduVault.DBClasses
{
	public class AccessRights
	{
		private long _id;
		private IRecipient _recipient;
		private Record _record;
		private AccessRightsType _ARType;

		public long Id { get { return _id; } }
		[NotMapped]
		public IRecipient Recipient { get { return _recipient; } set { _recipient = value; } }
		public Record Record { get { return _record; } set { _record = value; } }
		public AccessRightsType ARType { get { return _ARType; } set { _ARType = value; } }
		AccessRights()
		{

		}
		~AccessRights()
		{

		}
	}
}
