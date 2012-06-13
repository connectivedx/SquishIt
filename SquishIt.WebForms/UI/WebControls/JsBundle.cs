using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Files;

namespace SquishIt.WebForms.UI.WebControls
{
	[ParseChildren(typeof(JsFile), DefaultProperty="Files", ChildrenAsProperties=true)]
	[PersistChildren(false)]
	public class JsBundle : WebControl
	{
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
		public virtual List<JsFile> Files { get; set; }

		public JsBundle()
		{
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (Files == null || Files.Count == 0) return;
			if (string.IsNullOrEmpty(OutputFile)) throw new Exception("OutputFile was not set. Can't write bundle without a destination!");
			
			var bundle = SquishIt.Framework.Bundle.JavaScript();
			
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

            if (UseAbsolutePath) bundle.WithOutputBaseHref(Context.Request.Url.GetLeftPart(UriPartial.Authority));

			writer.Write(bundle.Render(OutputFile));
		}
	}
}
