﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace XmlGuy
{
	public class XmlDocumentWithout
	{
		public XmlElement RootElement { get; private set; }

		public XmlElement Begin(string rootElementName)
		{
			RootElement = new XmlElement { Name = rootElementName };

			return RootElement;
		}

        public bool Validate()
        {
            var doc = new System.Xml.XmlDocument();
            try
            {
                doc.LoadXml(this.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(bool pretty = false)
		{
			var sb = new StringBuilder();
			//sb.Add(@"<?xml version=""1.0"" encoding=""utf-8""?>", pretty);

			var currentDepth = -1; // start at the same indenting as the <?xml..?> decalration

			Action<IXmlElement> recurse = null;
			recurse = e =>
			{
				currentDepth++;

				if (e is CDataElement)
				{
					sb.Add("<![CDATA[ " + e.Value + " ]]>", pretty, currentDepth--);
					return;
				}

				var xe = e as XmlElement;
				if (xe == null)
					return;

				if (xe.Value == null && xe.Children.Count == 0)
				{
					sb.Add("<" + xe.Name + GetAttributes(xe) + " />", pretty, currentDepth);
				}
				else
				{
					// we keep text values on the same line, so only make it pretty if we don't have a value
					sb.Add("<" + xe.Name + GetAttributes(xe) + ">", pretty && xe.Value == null, currentDepth);

					if (xe.Value != null)
						sb.Add(e.Value);
					else
						foreach (var child in xe.Children)
							recurse(child);

					sb.Add("</" + xe.Name + ">", pretty, xe.Value != null ? 0 : currentDepth);
				}

				currentDepth--;
			};

			recurse(RootElement);

			return pretty ? sb.ToString().Trim() : sb.ToString().Replace("\t", "");
		}

		private static string GetAttributes(XmlElement e)
		{
		    return e.Attributes.Aggregate(" ", (current, attr) => current + (attr.Key + "=\"" + attr.Value + "\" "));
		}
	}
}
