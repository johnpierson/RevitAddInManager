using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using RevitBuildMsi;

namespace RevitAddinManager.Model;

public class ConfigMsi
{
    private  string MsiBuilderExe => DefaultSetting.exeRun;

    public ConfigMsi()
    {
        if (!File.Exists(MsiBuilderExe))
        {
            MessageBoxResult result2 = MessageBox.Show("Please Installer MsiBuilderToolkit !", Resource.AppName, MessageBoxButton.OKCancel,
                MessageBoxImage.Exclamation);
            if (result2 == MessageBoxResult.OK) Process.Start(DefaultSetting.MsiBuilderToolkitPath);
        }
    }
    public void SetConfiguration()
    {
        string directoryName = Path.GetDirectoryName(MsiBuilderExe);
        if (directoryName != null)
        {
            string configPath = Path.Combine(directoryName, "Resources", "config.ini");
            if(File.Exists(configPath))
            {
                //TODO
            }
            else
            {
                throw new FileNotFoundException($"File config.ini not found at :\n {configPath} !");
            }
        }
    }
    public void BuildMsi()
    {
        var proc = new Process();
        proc.StartInfo.FileName = MsiBuilderExe;
        proc.Start();
        proc.WaitForExit();
        MessageBox.Show("Completed !");
    }
}