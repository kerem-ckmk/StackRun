using System;
#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

    public class PostBuildiOS : IPostprocessBuildWithReport
    {
        public int callbackOrder
        {
            get { return 999; }
        }
        public void OnPostprocessBuild(BuildReport report)
        {
            string pathToBuiltProject = report.summary.outputPath;
            // Stop processing if targe is NOT iOS
            if (report.summary.platform != BuildTarget.iOS)
                return;

            Debug.Log("Entering Custom Post Build...");

            // Initialize PbxProject
            var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);

            string targetGuid = pbxProject.GetUnityFrameworkTargetGuid();
            pbxProject.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            pbxProject.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

            pbxProject.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
            pbxProject.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);

            File.WriteAllText(projectPath, pbxProject.WriteToString());

            string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            plist.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            plist.root.values.Remove("UIApplicationExitsOnSuspend");

            File.WriteAllText(plistPath, plist.WriteToString());

            ProjectCapabilityManager projCapability =
                new ProjectCapabilityManager(projectPath, $"Unity-iPhone/{Application.productName}.entitlements", "Unity-iPhone");
            projCapability.WriteToFile();

        }
    }
#endif
