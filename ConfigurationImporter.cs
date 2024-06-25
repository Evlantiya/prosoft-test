using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Newtonsoft.Json;

//паттерн шаблон
public abstract class ConfigurationImporter{
    public Configuration Import(string path){
        Console.WriteLine($"Импорт конфигурации {path}");
        try{
            var configuration = ParseConfigurationFile(path);
            return configuration;
        }
        catch(Exception ex){
            Console.WriteLine($"Ошибка импорта \n{ex.Message}");
            return null;
        }
    }

    protected abstract Configuration ParseConfigurationFile(string path);
}

public class XMLConfigurationImporter : ConfigurationImporter
{
    protected override Configuration ParseConfigurationFile(string path)
    {
        Configuration cfg = new Configuration();
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);
        XmlElement? xRoot = xDoc.DocumentElement;
        if (xRoot != null)
        {
            foreach (XmlElement xnode in xRoot)
            {
                if (xnode.Name=="name"){
                    cfg.Name=xnode.InnerText;
                }
                else if(xnode.Name=="description"){
                    cfg.Description=xnode.InnerText;
                }
            }
        }
        return cfg;
    }
}

public class CSVConfigurationImporter : ConfigurationImporter
{
    protected override Configuration ParseConfigurationFile(string path)
    {
        Configuration cfg = new Configuration();
        using (StreamReader reader = new StreamReader(path)){

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                cfg.Name = values[0];
                cfg.Description = values[1];
            } 
        }
        return cfg;
    }
}

public class JSONConfigurationImporter : ConfigurationImporter
{
    protected override Configuration ParseConfigurationFile(string path)
    {
        using (StreamReader file = new StreamReader(path))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            JsonSerializer js = new JsonSerializer();
            var cfg = js.Deserialize<Configuration>(reader);
            return cfg;
        }
    }
}



