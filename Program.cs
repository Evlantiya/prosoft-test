using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;


Dictionary<string, ConfigurationImporter> extensionMapForImport = new Dictionary<string, ConfigurationImporter>{
    {".xml", new XMLConfigurationImporter()},
    {".json", new JSONConfigurationImporter()},
    {".csv", new CSVConfigurationImporter()},
};

//настроить путь выходных файлов сборки, cba
var XMLcfgPath = @"../../../Data/test-xml.xml";
var JSONcfgPath = @"../../../Data/test-json.json";
var CSVcfgPath = @"../../../Data/test-csv.csv";


var cfgFileList = new List<string>(){XMLcfgPath,JSONcfgPath,CSVcfgPath};

List<Configuration> configurationList = new List<Configuration>();

foreach(var path in cfgFileList){
    configurationList.Add(ImportConfig(path));
}

foreach (var cfg in configurationList){
    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    Console.WriteLine($"Name: {cfg.Name}\nDescription: {cfg.Description}");
}

Configuration ImportConfig(string path)
{
    var cfgExtension = Path.GetExtension(path);
    var configurationImporter = extensionMapForImport.GetValueOrDefault(cfgExtension);
    if(configurationImporter == null){
        Console.WriteLine($"Формат {cfgExtension} не поддерживается");
        return null;
    }
    else{
        return configurationImporter.Import(path);
    }

}