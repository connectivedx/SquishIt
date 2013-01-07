using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Files;

namespace SquishIt.WebForms.UI.WebControls
{
	[ParseChildren(typeof(CssFile), DefaultProperty="Files", ChildrenAsProperties=true)]
	[PersistChildren(false)]
	public class CssBundle : WebControl
	{
		public string MediaType { get; set; }
		public string OutputFile { get; set; }
		public bool UseAbsolutePath { get; set; }
		public bool Debug
		{
			get
			{
				if (_debug.HasValue == false)
					return System.Web.HttpContext.Current.IsDebuggingEnabled;
				return _debug.Value;
			}
			set { _debug = value; }
		}
		bool? _debug;

		public bool ProcessImports { get; set; }
		public virtual List<CssFile> Files { get; set; }

		public CssBundle()
		{
			ProcessImports = false;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (Files == null || Files.Count == 0) return;
			if (string.IsNullOrEmpty(OutputFile)) throw new Exception("OutputFile was not set. Can't write bundle without a destination!");

			var bundle = SquishIt.Framework.Bundle.Css();

			if (Debug) 
				bundle.ForceDebug();
			else 
				bundle.ForceRelease();
			
			foreach (var file in Files)
			{
				if (string.IsNullOrWhiteSpace(file.RemotePath) == false)
					bundle.AddRemote(file.Path, file.RemotePath);
				else
					bundle.Add(file.Path);
			}

			if (!string.IsNullOrEmpty(MediaType)) bundle.WithAttribute("media", MediaType);

			if (ProcessImports) bundle.ProcessImports();

			if (UseAbsolutePath) bundle.WithOutputBaseHref(Context.Request.Url.GetLeftPart(UriPartial.Authority));

			writer.Write(bundle.Render(OutputFile));
		}
	}
}
