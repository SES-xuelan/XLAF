using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using XLAF.Public;
using System.Text.RegularExpressions;

public class DocMaker
{
	public class Function
	{
		public class Parameter
		{
			public string type = "";
			public string name = "";
			public string explain = "";
			public bool isVarieParameter = false;
		}

		public string name = "";
		public string returnType = "";
		public string description = "";
		public List<Parameter> parameters = new List<Parameter> ();
		public string returns = "";
		public string basicFacts = "";

	}

	public class Variable
	{
		public string name = "";
		public string description = "";
		public string value = "";
	}

	public class DocFileInfo
	{
		//all comments and functions
		public List<string> fileContent;
		//doc sub folder
		public string docFolder = "";
		//class name
		public string mainClassName = "";
		// main class description
		public string mainClassDescription = "";
		//main class description for index.html
		public string mainClassDescriptionForIndex = "";
		//index.html left menu link
		public string leftPadLinkedStringForIndexHtml{ get { return string.Format (DocTemplates.modulelistIndex, docFolder + "/" + mainClassName + ".html"); } }
		//xxxx.html(class name) left menu link
		public string leftPadLinkedStringForClasses{ get { return string.Format (DocTemplates.modulelistClass, docFolder + "/" + mainClassName + ".html", mainClassName); } }
		//xxxx.html(class name) left menu with no link
		public string leftPadNotLinkedStringForClasses{ get { return string.Format (DocTemplates.modulelistClassNoLink, mainClassName); } }
		//functions info
		public List<Function> functions = new List<Function> ();
		//variables
		public List<Variable> variables = new List<Variable> ();

		public override string ToString ()
		{
			return string.Format ("[DocFileInfo: docFolder={0}, mainClassName={1}, mainClassDescription={2}]", docFolder, mainClassName, mainClassDescription);
		}
	}


	public static readonly string DOC_PATH = Application.dataPath + "/XLAF/Docs/";
	public static List<DocFileInfo> allDocs;

	[MenuItem ("XLAF/DocTools/GenDocs")]
	static void GenDocs ()
	{
		allDocs = new List<DocFileInfo> ();
		if (Directory.Exists (DOC_PATH)) {
			Directory.Delete (DOC_PATH, true);
		}
		Directory.CreateDirectory (DOC_PATH);
		
		string scriptPath = Application.dataPath + "/XLAF/Scripts/Public/";
		// C Sharp 3rd-party file
		GetInfoFromFile (new FileInfo (Application.dataPath + "/XLAF/3rd-party/iTweenHashBuilder.cs"), "3rd-party");
		DirectoryInfo folder = new DirectoryInfo (scriptPath);
		FileSystemInfo[] files = folder.GetFileSystemInfos ();
		for (int i = 0; i < files.Length; i++) {
			if (files [i].Name.EndsWith (".cs")) {
				GetInfoFromFile (files [i], "Public");
			}
		}
		GenCSS ();
		GenIndex ();
		GenFiles ();
		AssetDatabase.Refresh ();
		Debug.Log ("Gen documents finished!");
	}

	private static List<string> GetCommentData (string filePathName)
	{     
		List<string> list = new List<string> (); 
		StreamReader sr = new StreamReader (filePathName);
		string line;
		bool needCode = false;//need add code
		while ((line = sr.ReadLine ()) != null) {
			line = line.Trim ();
			if (line.StartsWith ("///")) {
				list.Add (line);
				needCode = true;
			} else if (needCode) {
				list.Add (line);
				needCode = false;
			}
		}
		sr.Close ();
		sr.Dispose ();
		return list;
	}

	static void GetInfoFromFile (FileSystemInfo file, string docFolder)
	{
		if (!Directory.Exists (DOC_PATH + docFolder)) {
			Directory.CreateDirectory (DOC_PATH + docFolder);
		}
		Debug.Log ("Gen file =>" + file.Name);
		//get all comments
		List<string> data = GetCommentData (file.FullName);
		DocFileInfo dfi = new DocFileInfo ();
		dfi.fileContent = data;
		dfi.docFolder = docFolder;
		//get class info
		bool start = false;
		int functionsBeginLines = 0;
		for (int i = 0; i < data.Count; i++) {
			string s = data [i];
			if (!start && s.Contains ("public class ")) {
				dfi.mainClassName = s.Replace ("public class", "").Trim ();
				if (dfi.mainClassName.Contains (":")) {
					dfi.mainClassName = dfi.mainClassName.Split (':') [0].Trim ();
				}
				functionsBeginLines = i + 1;
				break;
			}
			if (s.Contains ("</summary>")) {
				start = false;
			}
			if (start) {
				string tmpStr = dfi.mainClassDescription + s.Replace ("///", "").Trim () + "\n";
				dfi.mainClassDescription = HandleSpecialMark (tmpStr, "../");
				dfi.mainClassDescriptionForIndex = HandleSpecialMark (tmpStr, "");
			}
			if (s.Contains ("<summary>") && dfi.mainClassDescription.Equals ("")) {
				start = true;
			} 
		}
		//get functions info (only public functions)
		start = false;
		string commentString = "";
		for (int i = functionsBeginLines; i < data.Count; i++) {
			string s = data [i];
			if (s.Trim ().StartsWith ("public ")) {
				start = false;
				if (s.Contains ("{")) {
					dfi.variables.Add (GenVariable (commentString, s));
				} else if (s.Contains (" class ")) {
					//inner class
				} else if (s.Contains ("enum")) {
					//do nothing
				} else {
					int index = i + 1;
					while (!s.Contains (")")) {
						s = s + data [index];
						index++;
					}
					Function f = GenFunction (commentString, s);
					// returnType is empty means ctor
					if (!f.returnType.Trim ().Equals (""))
						dfi.functions.Add (f);
				}
				commentString = "";
			}
			if (s.Contains ("<summary>")) {
				commentString = "";
				start = true;
			}
			if (start) {
				commentString += s.Replace ("///", "").Trim () + "\n";
			}
		}

		allDocs.Add (dfi);
	}

	private static Variable GenVariable (string comments, string vari)
	{
		Variable v = new Variable ();
		MatchCollection matches = Regex.Matches (comments, @"(?s)<summary>(.+)</summary>", RegexOptions.IgnoreCase);
		foreach (Match match in matches) {
			v.description = match.Groups [1].Value.Trim ();
			v.description = HandleSpecialMark (v.description, "../");
//			Debug.Log ("variable.description=>" + v.description);
		}
		matches = Regex.Matches (comments, @"(?s)<value>(.+)</value>", RegexOptions.IgnoreCase);
		foreach (Match match in matches) {
			v.value = match.Groups [1].Value.Trim ();
//			Debug.Log ("variable.value=>" + v.value);
		}
		vari = vari.Replace ("static", "");
		vari = vari.Replace ("public", "").Trim ();
		matches = Regex.Matches (vari.Split ('{') [0], @"(.+) (.+)", RegexOptions.IgnoreCase);
		foreach (Match match in matches) {
			string name = match.Groups [2].Value.Trim ();
//			Debug.Log ("variable.name=>" + name);
			v.name = name;
			break;
		}
		return v;
	}

	private static Function GenFunction (string comments, string func)
	{
		Function f = new Function ();
		f.basicFacts = func.Trim ();
//		Debug.Log ("GenFunction=>\n" + comments + "\n" + func);
		//functionDescription
		MatchCollection matches = Regex.Matches (comments, @"(?s)<summary>(.+)</summary>", RegexOptions.IgnoreCase);
		foreach (Match match in matches) {
			f.description = match.Groups [1].Value.Trim ();
			f.description = HandleSpecialMark (f.description, "../");

//			Debug.Log ("function.description=>" + f.description);
		}
		//function name & return type
		func = func.Replace ("static", "");
		func = func.Replace ("public", "").Trim ();
		matches = Regex.Matches (func.Split ('(') [0], @"(.+) (.+)", RegexOptions.IgnoreCase);
		foreach (Match match in matches) {
			string returnType = match.Groups [1].Value.Trim ();
			string name = match.Groups [2].Value.Trim ();
//			Debug.Log ("function.returnType=>" + returnType);
//			Debug.Log ("function.name=>" + name);
			f.name = name;
			f.returnType = returnType;
			break;
		}
		//parameters info
		string bracketsContent = func.Split ('(') [1].Replace (")", "").Trim ();
//		Debug.Log ("bracketsContent=>" + bracketsContent);
		if (!string.IsNullOrEmpty (bracketsContent)) {
			//do: Action<bool,string>  => Action<bool|string>
			if (bracketsContent.Contains ("<")) {
				MatchCollection matches1 = Regex.Matches (bracketsContent, @"<(.+?)>", RegexOptions.IgnoreCase);
				foreach (Match match in matches1) {
					string m = match.Groups [1].Value.Trim ();
					bracketsContent = bracketsContent.Replace (m, m.Replace (",", "|"));
				}
			}
			if (bracketsContent.Contains (",")) {
				//multi params
				foreach (string s in bracketsContent.Split(',')) {
					string[] strs = s.Trim ().Split (' ');
					Function.Parameter p = new Function.Parameter ();
					if (strs.Length >= 3) {
						p.isVarieParameter = true;
						p.type = strs [0] + " " + strs [1].Trim ();
						p.name = strs [2].Trim ();
					} else {
						p.isVarieParameter = false;
						p.type = strs [0].Trim ();
						p.name = strs [1].Trim ();
					}
					f.parameters.Add (p);
//					Debug.Log (string.Format ("isVarieParameter=>{0} | type=>{1} | name=>{2}", p.isVarieParameter, p.type, p.name));
				}
			} else {
				//only one param
				string[] strs = bracketsContent.Split (' ');
				Function.Parameter p = new Function.Parameter ();
				if (strs.Length >= 3) {
					p.isVarieParameter = true;
					p.type = strs [0] + " " + strs [1].Trim ();
					p.name = strs [2].Trim ();
				} else {
					p.isVarieParameter = false;
					p.type = strs [0].Trim ();
					p.name = strs [1].Trim ();
				}
				f.parameters.Add (p);
//				Debug.Log (string.Format ("isVarieParameter=>{0} | type=>{1} | name=>{2}", p.isVarieParameter, p.type, p.name));
			}
		}
		//parameters explain
		matches = Regex.Matches (comments, @"(?s)<param name=""(.+?)"">(.+?)</param>", RegexOptions.IgnoreCase);
		foreach (Match match in matches) {
			string name = match.Groups [1].Value.Trim ();
			string explain = match.Groups [2].Value.Trim ();
//			Debug.Log ("p.name=>" + name);
//			Debug.Log ("p.explain=>" + explain);
			foreach (Function.Parameter p in f.parameters) {
				if (p.name == name) {
					p.explain = explain;
					break;
				}
			}
		}
		//returns
		matches = Regex.Matches (comments, @"(?s)<returns>(.+)</returns>", RegexOptions.IgnoreCase);
		foreach (Match match in matches) {
			f.returns = match.Groups [1].Value.Trim ();
			f.returns = HandleSpecialMark (f.returns, "../");
//			Debug.Log ("returns " + f.returns);
		}

		return f;
	}

	/// <summary>
	///  Gens css file
	/// </summary>
	private static void GenCSS ()
	{
		ModUtils.WriteToFile (DOC_PATH + "css.css", DocTemplates.cssfile);
	}

	/// <summary>
	/// Gens index.html
	/// </summary>
	private static void GenIndex ()
	{
		string classeslis = "";
		string modulelists = "";
		foreach (DocFileInfo dfi in allDocs) {
			string classesli = string.Format (DocTemplates.classesli, dfi.docFolder + "/" + dfi.mainClassName + ".html", dfi.mainClassName);
			string modulelist = string.Format (DocTemplates.modulelistIndex, dfi.docFolder + "/" + dfi.mainClassName + ".html", dfi.mainClassName, dfi.mainClassDescriptionForIndex);
			classeslis += classesli;
			modulelists += modulelist;
		}
		ModUtils.WriteToFile (DOC_PATH + "index.html", string.Format (DocTemplates.indexhtml,
			DocConfig.DOCUMENT_NAME,
			DocConfig.DOCUMENT_SUBTITLE,
			classeslis,
			DocConfig.DOCUMENT_NAME,
			modulelists
		));
	}

	/// <summary>
	/// Gens the files.
	/// </summary>
	private static void GenFiles ()
	{
		foreach (DocFileInfo dfi in allDocs) {
			//left menu
			string classeslis = "";
			foreach (DocFileInfo menuDfi in allDocs) {
				if (dfi.mainClassName.Equals (menuDfi.mainClassName)) {
					classeslis += menuDfi.leftPadNotLinkedStringForClasses + "\n";
				} else {
					classeslis += menuDfi.leftPadLinkedStringForClasses + "\n";
				}
			}
			//functions 
			string functions = "";
			string functionsInfo = "";
			foreach (Function f in dfi.functions) {
				functions += string.Format (DocTemplates.functionList, f.name, Escape (f.basicFacts), f.description) + "\n";
				string parameters = "";
				foreach (Function.Parameter p in f.parameters) {
					parameters += string.Format (DocTemplates.parameter, Escape (p.type), p.name, p.explain) + "\n";
				}
				functionsInfo += string.Format (DocTemplates.functionInfo, f.name, f.name, f.description, parameters, Escape (f.returnType).Equals ("void") ? "none" : f.returnType, f.returns);
			}
			//properties / variables
			string variables = "";
			foreach (Variable v in dfi.variables) {
				variables += string.Format (DocTemplates.variableList, v.name, v.description) + "\n";
			}
			//write to file
			ModUtils.WriteToFile (DOC_PATH + dfi.docFolder + "/" + dfi.mainClassName + ".html", string.Format (DocTemplates.classTemplate,
				DocConfig.DOCUMENT_NAME,
				DocConfig.DOCUMENT_SUBTITLE,
				classeslis,
				dfi.mainClassName,
				dfi.mainClassDescription,
				variables,
				functions,
				functionsInfo
			));

		}

	}

	private static string Escape (string htmlText)
	{
		return htmlText.Replace ("<", "&lt;").Replace (">", "&gt;");
	}

	private static string HandleSpecialMark (string s, string parent)
	{
		string ret = s;
		MatchCollection matches = Regex.Matches (s, @"(?s)(<see cref=""(.+)""/>)", RegexOptions.IgnoreCase);
		foreach (Match match in matches) {
			string fullMatch = match.Groups [1].Value.Trim ();
			string url = match.Groups [2].Value.Replace ("XLAF.", "").Replace (".", "/");
			ret = ret.Replace (fullMatch, string.Format ("<a href=\"{0}{1}.html\">{2}</a>", parent, url, url.Split ('/') [1]));
		}

		ret = ret.Replace ("<para></para>", "<br />");
		ret = ret.Replace ("<c>", "<code>").Replace ("</c>", "</code>");
		return ret;
	}
}
