using System.IO;

namespace Server;

public class ConfigReader
{
    private string _configFileName;
    private StreamReader _reader;
    
    public string ServerAddress { get; private set; }
    public int ServerPort { get; private set; }


    public ConfigReader(string configFileName)
    {
        _configFileName = configFileName;
        _reader = new StreamReader(File.OpenRead(_configFileName));
        ReadConfig();
    }

    private void ReadConfig()
    {
        ServerAddress = _reader.ReadLine();
        ServerPort = int.Parse(_reader.ReadLine());
    }
    
}