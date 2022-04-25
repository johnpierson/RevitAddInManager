using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using WixSharp;
using IniParser;
using RevitBuildMsi;
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

string pathconfig = @"D:\API\RevitAddInManager\RevitBuildMsi\bin\Debug\Config.ini";
Config config = new Config(pathconfig);
string installationDir = @"%AppDataFolder%\Autodesk\Revit\Addins\";
var project = new Project
{
    Name = config.ProjectName,
    OutDir = config.OutputDir,
    Platform = Platform.x64,
    Description = config.Description,
    UI = WUI.WixUI_InstallDir,
    Version = new Version(config.Version),
    OutFileName = GetFileName(),
    InstallScope = InstallScope.perUser,
    MajorUpgrade = MajorUpgrade.Default,
    GUID = GetGuid(),
    // BackgroundImage = @"Installer\Resources\Icons\BackgroundImage.png",
    // BannerImage = @"Installer\Resources\Icons\BannerImage.png",
    ControlPanelInfo =
    {
        Manufacturer = config.Manufacturer,
        HelpLink = config.HelpLink,
        Comments = config.Comments,
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
    var versionStorages =
        new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<WixEntity>>();

    var directoryInfo = new DirectoryInfo(config.DirContentFiles);
    foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
    {
        var fileVersion = directory.Name;
        var files = new Files($@"{directory.FullName}\*.*");
        if (versionStorages.ContainsKey(fileVersion))
            versionStorages[fileVersion].Add(files);
        else
            versionStorages.Add(fileVersion, new System.Collections.Generic.List<WixEntity> {files});

        var assemblies = Directory.GetFiles(config.DirContentFiles, "*", SearchOption.AllDirectories);
        Console.WriteLine($"Added '{fileVersion}' version files: ");
        foreach (var assembly in assemblies) Console.WriteLine($"'{assembly}'");
    }
    return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>().ToArray();
}

Guid GetGuid()
{
    string guid = config.Guid;
    if (string.IsNullOrEmpty(guid))
    {
        return new Guid();
    }

    return new Guid(guid);
}

string GetFileName()
{
    if (string.IsNullOrEmpty(config.OutFileName))
    {
        return new StringBuilder().Append(config.ProjectName).Append("-").Append(config.Version).ToString();
    }

    return config.OutFileName;
}