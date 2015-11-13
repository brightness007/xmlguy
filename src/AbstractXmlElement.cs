using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlGuy
{
	public abstract class AbstractXmlElement : IXmlElement
	{
	    protected AbstractXmlElement(XmlElement parent = null)
		{
			Parent = parent;
		}

		protected XmlElement Parent { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}
