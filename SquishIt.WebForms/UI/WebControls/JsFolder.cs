using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Files;
using System.IO;

namespace SquishIt.WebForms.UI.WebControls
{
	public class JsFolder : JsBundle
	{
		public string FolderPath { get; set; }
		public override List<JsFile> Files
		{
			get
			{
				if (FolderPath == null)
					return new List<JsFile>();

				var files = new List<JsFile>();
				var rootPath = Context.Server.MapPath(FolderPath);

				var fileNames = Directory.GetFiles(rootPath);
				string extension;
				foreach (var fileName in fileNames)
				{
					// ignore debug preprocessor files eg .debug.js files from coffeescript processor in debug mode
					if (fileName.Contains("squishit.debug")) continue;

					extension = Path.GetExtension(fileName);
					if (extension.Equals(".js", StringComparison.OrdinalIgnoreCase) || extension.Equals(".coffee", StringComparison.OrdinalIgnoreCase))
						files.Add(new JsFile() { Path = string.Concat(FolderPath, "/", Path.GetFileName(fileName)) });
				}

				return files;
			}
			set
			{
				throw new InvalidOperationException("JsFolder does not support manually added files");
			}
		}
	}
}
