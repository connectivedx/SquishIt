using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Css;
using SquishIt.Framework.Files;
using System.IO;

namespace SquishIt.WebForms.UI.WebControls
{
	public class CssFolder : CssBundle
	{
		public string FolderPath { get; set; }
		public override List<CssFile> Files
		{
			get
			{
				if (FolderPath == null)
					return new List<CssFile>();

				var files = new List<CssFile>();
				var rootPath = Context.Server.MapPath(FolderPath);

				var fileNames = Directory.GetFiles(rootPath);
				foreach (var fileName in fileNames)
				{
					files.Add(new CssFile() { Path = string.Concat(FolderPath, "/", Path.GetFileName(fileName)) });
				}

				return files;
			}
			set
			{
				throw new InvalidOperationException("CssFolder does not support manually added files");
			}
		}
	}
}
