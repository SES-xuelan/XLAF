using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XLAF.Public;
using System.IO;

public class MakeSpriteSheetWithTexturePacker:EditorWindow
{
	private static string configFile = Application.dataPath + "/XLAF/Editor/TexturePackerConfig.json";
	private static string outputPath = Application.dataPath + "/Resources/ImageSheets/";

	[MenuItem ("XLAF/TexturePacker/Select TexturePacker.exe")]
	static void SelectTexturePackerFile ()
	{
		string data = "";
		if (File.Exists (configFile)) {
			data = ModUtils.ReadFile (configFile);
		} else {
			FileStream fs = File.Create (configFile);
			fs.Close ();
			fs.Dispose ();
		}
		string path = EditorUtility.OpenFilePanel ("Select TexturePacker.exe", data, "exe");
		if (!string.IsNullOrEmpty (path)) {
			ModUtils.WriteToFile (configFile, path, true);
		}
		Debug.Log ("New TexturePacker File is   " + path);
	}

	[MenuItem ("XLAF/TexturePacker/MakeSheets")]
	static void MakeSheets ()
	{
		string sourcePath = Application.dataPath + "/../Resources-Images/";
		if (!Directory.Exists (sourcePath)) {
			Debug.LogError (sourcePath + " is not exists, abort!");
			return;
		}
		if (!File.Exists (configFile)) {
			Debug.LogError ("Please select TexturePacker.exe first!");
			return;
		}
		if (Directory.Exists (outputPath)) {
			Directory.Delete (outputPath, true);
		}
		string data = ModUtils.ReadFile (configFile);
		DirectoryInfo folder = new DirectoryInfo (sourcePath);
		FileSystemInfo[] files = folder.GetFileSystemInfos ();
		int length = files.Length;
		for (int i = 0; i < length; i++) {
			if (files [i] is DirectoryInfo) {
				MakeSheet (files [i], data);
			}
		}
		Debug.Log ("Make sheets succeed!");
	}

	private static void MakeSheet (FileSystemInfo file, string texturePackerExePath)
	{
		string cmd = string.Format (
			             "--sheet {0}.png --data {1}.tpsheet --format unity-texture2d {2}",
			             outputPath + file.Name,
			             outputPath + file.Name,
			             file.FullName
		             );
		processCommand (texturePackerExePath, cmd);
	}

	///////////////// cmd ////////////////////////

	private static void processCommand (string command, string argument)
	{
		System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo (command);
		start.Arguments = argument;
		start.CreateNoWindow = false;
		start.ErrorDialog = true;
		start.UseShellExecute = true;

		if (start.UseShellExecute) {
			start.RedirectStandardOutput = false;
			start.RedirectStandardError = false;
			start.RedirectStandardInput = false;
		} else {
			start.RedirectStandardOutput = true;
			start.RedirectStandardError = true;
			start.RedirectStandardInput = true;
			start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
			start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
		}

		System.Diagnostics.Process p = System.Diagnostics.Process.Start (start);

		if (!start.UseShellExecute) {
			Debug.Log (p.StandardOutput);
			Debug.Log (p.StandardError);
		}

		p.WaitForExit ();
		p.Close ();
	}

}
