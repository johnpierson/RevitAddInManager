using IniParser;

namespace RevitBuildMsi;

public struct Config
{
    public string Company { get; private set; }

    public string Contact { get; private set; }

    public string BackgroundImage { get; private set; }

    public string BannerImage { get; private set; }
    public string ProductIcon { get; private set; }
    public string LicenceFile { get; private set; }

    public string Guid { get; private set; }

    public string OutputDir { get; private set; }
    public string ProjectName { get; private set; }
    public string Version { get; private set; }
    public string Comments { get; private set; }
    public string HelpLink { get; private set; }
    public string Manufacturer { get; private set; }
    public string HelpTelephone { get; private set; }
    public string Description { get; private set; }
    public string InstallDir { get; private set; }
    public string DirContentFiles { get; private set; }
    public string OutFileName { get; private set; }
    public string AutodeskProduct { get; private set; }
    
    public Config(string filename)
    {
        try
        {
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(filename);
            var config = data["config"];

            Company = config["Company"];
            Contact = config["Contact"];
            BackgroundImage = config["BackgroundImage"];
            LicenceFile = config["LicenceFile"];
            Guid = config["Guid"];
            BannerImage = config["BannerImage"];
            ProductIcon = config["ProductIcon"];
            Version = config["Version"];
            OutputDir = config["OutputDir"];
            ProjectName = config["ProjectName"];
            Comments = config["Comments"];
            HelpLink = config["HelpLink"];
            Manufacturer = config["Manufacturer"];
            HelpTelephone = config["HelpTelephone"];
            Description = config["Description"];
            InstallDir = config["InstallDir"];
            DirContentFiles = config["DirContentFiles"];
            OutFileName = config["OutFileName"];
            AutodeskProduct = config["AutodeskProduct"];
        }
        catch (Exception e)
        {
            throw new Exception($"Unable to read ini file, please refer to example_config.ini format. Error message: {e.Message}");
        }
    }
} 