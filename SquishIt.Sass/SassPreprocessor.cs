using System.Linq;
using System.Text.RegularExpressions;
using SquishIt.Framework.Base;
using SquishIt.Framework;
using System.Collections.Generic;
using System.IO;

namespace SquishIt.Sass
{
	public class SassPreprocessor : Preprocessor
	{
		/* 
		 * SassPreprocessor has been patched by KF/ISITE to cache SASS compile results
		 * This drastically speeds up page load times when running SASS in debug mode.
		 * 
		 * NOTE: this cache has the following limitations:
		 * - Relatively naive dependency tracking (@import of only one level is supported)
		*/

		static readonly Regex IsSass = new Regex(@"\.sass$", RegexOptions.Compiled);
		static readonly Regex ScssImports = new Regex("@import.+\"(.*)\";", RegexOptions.Compiled);

		readonly bool _cacheEnabled;

		public SassPreprocessor(bool enableCaching)
		{
			_cacheEnabled = enableCaching;
		}

		public override IProcessResult Process(string filePath, string content)
		{
			var cacheKey = "SASSCACHE-" + filePath;

			string result;
			var cache = new BundleCache();

			if (_cacheEnabled)
			{
				result = cache.GetContent(cacheKey);

				if (result != null) return new ProcessResult(result, GetImportDependencies(filePath, content));
			}
			
			var dependencies = GetImportDependencies(filePath, content);
			var compiler = new SassCompiler("");
			var sassMode = IsSass.IsMatch(filePath) ? SassCompiler.SassMode.Sass : SassCompiler.SassMode.Scss;

			result = compiler.CompileSass(content, sassMode);

			if (_cacheEnabled)
			{
				var dependentPaths = new List<string>(dependencies);
				dependentPaths.Add(filePath);

				cache.Add(cacheKey, result, dependentPaths);
			}
				

			return new ProcessResult(result, dependencies);
		}

		public string[] GetImportDependencies(string filePath, string content)
		{
			var possibleFilePaths = new List<string>();
			var rootPath = Path.GetDirectoryName(filePath);

			var matches = ScssImports.Matches(content);

			foreach (Match match in matches)
			{
				possibleFilePaths.Add(Path.Combine(rootPath, match.Groups[1] + Path.GetExtension(filePath)));
				possibleFilePaths.Add(Path.Combine(rootPath, "_" + match.Groups[1] + Path.GetExtension(filePath)));
			}

			return possibleFilePaths.Where(File.Exists).ToArray();
		}

		public override string[] Extensions
		{
			get { return new[] { ".sass", ".scss" }; }
		}
	}
}
