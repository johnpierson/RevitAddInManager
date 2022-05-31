using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace RevitAddinManager.Model;

public static class UriResolveWPF
{
    public static void LoadViewFromUri(this FrameworkElement userControl, string baseUri)
    {
    
        try
        {
    
            Uri resourceLocater = new Uri(baseUri, UriKind.Relative);
            PackagePart exprCa = (PackagePart)typeof(Application).GetMethod("GetResourceOrContentPart", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] {
                resourceLocater });
            Stream stream = exprCa.GetStream();
            var uri = new Uri((Uri)typeof(BaseUriHelper).GetProperty("PackAppBaseUri", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null), resourceLocater);
            var parserContext = new ParserContext
            {
    
                BaseUri = uri
            };
            typeof(XamlReader).GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] {
                stream, parserContext, userControl, true });
        }
        catch (Exception)
        {
    
            //log
        }
    }
}