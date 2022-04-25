using System.Text;
using RevitBuildMsi;
using WixSharp.CommonTasks;
using WixSharp.Controls;
using Console = System.Console;
using Dir = WixSharp.Dir;
using DirectoryInfo = System.IO.DirectoryInfo;
using Files = WixSharp.Files;
using Guid = System.Guid;
using InstallDir = WixSharp.InstallDir;
using InstallScope = WixSharp.InstallScope;
using MajorUpgrade = WixSharp.MajorUpgrade;
using Platform = WixSharp.Platform;
using Project = WixSharp.Project;
using Version = System.Version;
using WixEntity = WixSharp.WixEntity;
using WUI = WixSharp.WUI;

string pathConfig = @".\Resources\Config.ini";
Config config = new Config(pathConfig);
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
    BackgroundImage = GetBackGroundImage(),
    BannerImage = GetBannerImage(),
    // LicenceFile = config.LicenceFile,
    ControlPanelInfo =
    {
        Manufacturer = config.Manufacturer,
        HelpLink = config.HelpLink,
        Comments = config.Comments,
        ProductIcon = GetProductIcon(),
        HelpTelephone = config.HelpTelephone,
        Contact = config.Contact,
    },
    Dirs = new Dir[]
    {
        new InstallDir(config.InstallDir, GenerateWixEntities())
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
        Console.WriteLine($"Added '{fileVersion}' version files: ");
        foreach (var file in directory.GetFiles()) Console.WriteLine($"'{file.Name}'");
        if (versionStorages.ContainsKey(fileVersion))
            versionStorages[fileVersion].Add(files);
        else
            versionStorages.Add(fileVersion, new System.Collections.Generic.List<WixEntity> {files});
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
    if (string.IsNullOrEmpty(config.OutFileName)|| config.OutFileName.Equals("Add-in Installer"))
    {
        return new StringBuilder().Append(config.ProjectName).Append("-").Append(config.Version).ToString();
    }

    return config.OutFileName;
}

string GetBackGroundImage()
{
    if (string.IsNullOrEmpty(config.BackgroundImage))
    {
        return @".\Resources\BackgroundImage.png";
    }
    return config.BackgroundImage;
}
string GetBannerImage()
{
    if (string.IsNullOrEmpty(config.BannerImage))
    {
        return @".\Resources\BannerImage.png";
    }
    return config.BannerImage;
}
string GetProductIcon()
{
    if (string.IsNullOrEmpty(config.ProductIcon))
    {
        return @".\Resources\ShellIcon.ico";
    }
    return config.ProductIcon;
}