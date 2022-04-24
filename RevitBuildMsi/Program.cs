using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using WixSharp;
using IniParser;
using WixSharp.CommonTasks;
using WixSharp.Controls;
using Console = System.Console;
using Dir = WixSharp.Dir;
using Directory = System.IO.Directory;
using DirectoryInfo = System.IO.DirectoryInfo;
using Files = WixSharp.Files;
using Guid = System.Guid;
using InstallDir = WixSharp.InstallDir;
using InstallScope = WixSharp.InstallScope;
using MajorUpgrade = WixSharp.MajorUpgrade;
using Platform = WixSharp.Platform;
using Project = WixSharp.Project;
using SearchOption = System.IO.SearchOption;
using Version = System.Version;
using WixEntity = WixSharp.WixEntity;
using WUI = WixSharp.WUI;


const string installationDir = @"%AppDataFolder%\Autodesk\Revit\Addins\";
const string projectName = "RevitAddinManager";
const string outputName = "RevitAddinManager";
const string outputDir = @"D:\";
const string version = "1.0.0";

var fileName = new StringBuilder().Append(outputName).Append("-").Append(version);
var project = new Project
{
    Name = projectName,
    OutDir = outputDir,
    Platform = Platform.x64,
    Description = "Project Support Developer Work With Revit API",
    UI = WUI.WixUI_InstallDir,
    Version = new Version(version),
    OutFileName = fileName.ToString(),
    InstallScope = InstallScope.perUser,
    MajorUpgrade = MajorUpgrade.Default,
    GUID = new Guid("A0176A8B-2483-4073-B6BB-4A481D9B4439"),
   // BackgroundImage = @"Installer\Resources\Icons\BackgroundImage.png",
   // BannerImage = @"Installer\Resources\Icons\BannerImage.png",
    ControlPanelInfo =
    {
        Manufacturer = "Autodesk",
        HelpLink = "https://github.com/chuongmep/RevitAddInManager/issues",
        Comments = "Project Support Developer With Revit API",
       // ProductIcon = @"Installer\Resources\Icons\ShellIcon.ico"
    },
    Dirs = new Dir[]
    {
        new InstallDir(installationDir, GenerateWixEntities())
    }
};

project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);
project.BuildMsi();

WixEntity[] GenerateWixEntities()
{
    var versionRegex = new Regex(@"\d+");
    var versionStorages = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<WixEntity>>();

    foreach (var directory in args)
    {
        var directoryInfo = new DirectoryInfo(directory);
        var fileVersion = versionRegex.Match(directoryInfo.Name).Value;
        var files = new Files($@"{directory}\*.*");
        if (versionStorages.ContainsKey(fileVersion))
            versionStorages[fileVersion].Add(files);
        else
            versionStorages.Add(fileVersion, new System.Collections.Generic.List<WixEntity> { files });

        var assemblies = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
        Console.WriteLine($"Added '{fileVersion}' version files: ");
        foreach (var assembly in assemblies) Console.WriteLine($"'{assembly}'");
    }

    return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>().ToArray();
}
