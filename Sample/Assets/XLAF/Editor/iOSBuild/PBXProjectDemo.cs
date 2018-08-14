using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
        using UnityEditor.iOS.Xcode;
        using UnityEditor.XCodeEditor;
#endif
#endif
using System.Collections;

public class PBXProjectDemo
{
    /// <summary>
    /// after build will call this function
    /// </summary>
    /// <param name="buildTarget">Build target.</param>
    /// <param name="pathToBuiltProject">Path to built project.</param>
    [PostProcessBuildAttribute(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
#if UNITY_IOS
        // BuildTarget need set to iOS
        if (buildTarget != BuildTarget.iOS)
            return;

        // init
        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);
        string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        // add flag
        pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
        // turn off Bitcode
        pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");

        // add framwrok
        pbxProject.AddFrameworkToProject(targetGuid, "Security.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "CoreGraphics.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "ImageIO.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "CoreData.framework", false);

        //add lib
        AddLibToProject(pbxProject, targetGuid, "libsqlite3.tbd");
        AddLibToProject(pbxProject, targetGuid, "libc++.tbd");
        AddLibToProject(pbxProject, targetGuid, "libz.tbd");

        // apply
        File.WriteAllText(projectPath, pbxProject.WriteToString());

        // modify Info.plist
        var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // insert URL Scheme into Info.plsit
        var array = plist.root.CreateArray("CFBundleURLTypes");
        //insert dict
        var urlDict = array.AddDict();
        urlDict.SetString("CFBundleTypeRole", "Editor");
        //insert array
        var urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
        urlInnerArray.AddString("blablabla");
        //apply settings
        plist.WriteToFile(plistPath);

        //insert code
        //read UnityAppController.mm 
        string unityAppControllerPath = pathToBuiltProject + "/Classes/UnityAppController.mm";
        XClass UnityAppController = new XClass(unityAppControllerPath);

        //add single line code below some codes
        UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", "#import <UMSocialCore/UMSocialCore.h>");

        string newCode = "\n" +
                         "    [[UMSocialManager defaultManager] openLog:YES];\n" +
                         "    [UMSocialGlobal shareInstance].type = @\"u3d\";\n" +
                         "    [[UMSocialManager defaultManager] setUmSocialAppkey:@\"" + "\"];\n" +
                         "    [[UMSocialManager defaultManager] setPlaform:UMSocialPlatformType_WechatSession appKey:@\"" + "\" appSecret:@\"" + "\" redirectURL:@\"http://mobile.umeng.com/social\"];\n" +
                         "    \n";
        //add multi lines code below some codes
        UnityAppController.WriteBelow("// if you wont use keyboard you may comment it out at save some memory", newCode);
#endif
    }
#if UNITY_IOS
    /// <summary>
    /// Adds the lib to project.
    /// </summary>
    /// <param name="inst">Inst.</param>
    /// <param name="targetGuid">Target GUID.</param>
    /// <param name="lib">Lib.</param>
    static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
    {
        string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }
#endif
}