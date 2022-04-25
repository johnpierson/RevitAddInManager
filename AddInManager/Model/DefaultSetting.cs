﻿namespace RevitAddinManager.Model;

/// <summary>
/// All setting name default for addin
/// </summary>
public static class DefaultSetting
{
    public static string FileName = "ExternalTool";

    public static string FormatExAddin = ".addin";
    public static string FormatDisable = ".disable";
    public static string AdskPath = "Autodesk\\Revit\\Addins";

    public static string IniName = "revit.ini";

    public static string TempFolderName = "RevitAddins";

    public static string AimInternalName = "AimInternal.ini";
    public static string exeRun = @"C:\Program Files\ChuongMep\RevitBuildMsi\RevitBuildMsi.exe";
    public static string MsiBuilderToolkitPath = @"https://github.com/chuongmep/MsiBuilderToolkit";
}