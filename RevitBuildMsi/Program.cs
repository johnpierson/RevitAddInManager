using WixSharp;
using IniParser;

public struct Config
{
    public string Company { get; private set; }

    public string FullAppName { get; private set; }

    public string AppName { get; private set; }

    public string ShortCutName { get; private set; }

    public string ExecutableName { get; private set; }

    public string Manufacturer { get; private set; }

    public string Contact { get; private set; }

    public string BackgroungImage { get; private set; }

    public string LicenceFile { get; private set; }

    public string Guid { get; private set; }

    public string OutFileName { get; private set; }

    public string BannerImage { get; private set; }

    public bool PreserveTempFiles { get; private set; }

    public string ProductIcon { get; private set; }

    public Config(string filename)
    {
        try
        {
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(filename);
            var config = data["config"];

            Company = config["Company"];
            FullAppName = config["FullAppName"];
            AppName = config["AppName"];
            ShortCutName = config["ShortCutName"];
            ExecutableName = config["ExecutableName"];
            Manufacturer = config["Manufacturer"];
            Contact = config["Contact"];
            BackgroungImage = config["BackgroungImage"];
            LicenceFile = config["LicenceFile"];
            Guid = config["Guid"];
            OutFileName = config["OutFileName"];
            BannerImage = config["BannerImage"];
            PreserveTempFiles = config["PreserveTempFiles"].ToString() == true.ToString();
            ProductIcon = config["ProductIcon"];
        }
        catch (Exception e)
        {
            throw new Exception(string.Format(
                "Unable to read ini file, please refer to example_config.ini format. Error message: {0}", e.Message));
        }
    }
}

class Program
{
    private static readonly string configIniFilePath = GetArtifactsPath("config.ini");

    public static string GetArtifactsPath(string artifactFileName)
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "artifacts", artifactFileName);
    }

    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Wrong command line arguments. Usage installer.exe path_to_binaries output_path version");
            return;
        }

        var pathToBinaries = args[0];
        var outputPath = args[1];
        var version = args[2];

        if (!System.IO.File.Exists(configIniFilePath))
        {
            Console.WriteLine("Can not find {0} file", configIniFilePath);
            return;
        }

        var config = new Config(configIniFilePath);

        var desktopIcon = new Feature("Desktop icon", "Add icon to Desktop");
        var addToStarupMenu = new Feature("Add to Startup Menu", "Add application to Startup Menu");

        Func<ExeFileShortcut> uninstallShortcut = () =>
            new ExeFileShortcut(string.Format("Uninstall {0}", config.FullAppName), "[System64Folder]msiexec.exe",
                "/x [ProductCode]");

        var appFolder = string.Format(@"{0}\{1}", config.Company, config.AppName);
        var project = new ManagedProject(
            config.FullAppName,
            new Dir(new Id("APPLICATIONFOLDER"),
                appFolder,
                new Files(string.Format(@"{0}\*.*", args[0])),
                uninstallShortcut()),
            new Dir(string.Format(@"%ProgramMenu%\{0}\{1}", config.Company, config.AppName),
                uninstallShortcut(),
                new ExeFileShortcut(addToStarupMenu, config.AppName,
                    string.Format("[APPLICATIONFOLDER]{0}", config.ExecutableName), "")
                {
                    WorkingDirectory = "[APPLICATIONFOLDER]"
                }),
            new Dir(@"%Desktop%",
                new ExeFileShortcut(desktopIcon, config.FullAppName,
                    string.Format("[APPLICATIONFOLDER]{0}", config.ExecutableName), "")
                {
                    WorkingDirectory = "[APPLICATIONFOLDER]"
                }),
            new Property("ApplicationFolderName", appFolder),
            new Property("WixAppFolder", "WixPerMachineFolder"),
            new Property("ALLUSERS", "1")
        );

        project.Version = new Version(version);
        project.ControlPanelInfo.Manufacturer = config.Manufacturer;
        project.ControlPanelInfo.Contact = config.Contact;
        project.UI = WUI.WixUI_Advanced;
        project.ValidateBackgroundImage = false;

        if (!string.IsNullOrWhiteSpace(config.BackgroungImage))
        {
            project.BackgroundImage = GetArtifactsPath(config.BackgroungImage);
        }

        if (!string.IsNullOrWhiteSpace(config.BannerImage))
        {
            project.BannerImage = GetArtifactsPath(config.BannerImage);
        }

        project.OutFileName = config.OutFileName;
        project.OutDir = outputPath;
        project.ControlPanelInfo.ProductIcon = GetArtifactsPath(config.ProductIcon);

        project.PreserveTempFiles = config.PreserveTempFiles;
        project.LicenceFile = GetArtifactsPath(config.LicenceFile);
        project.GUID = new Guid(config.Guid);
        project.BuildMsi();
    }
}