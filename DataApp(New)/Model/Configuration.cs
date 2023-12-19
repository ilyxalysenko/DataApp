using System;
using System.Linq;
using System.Xml.Linq;

namespace DataApp_New_.Model
{
    public class Configuration
    {
        public string ConnectionString { get; set; }
        public string CardsXmlFilePath { get; set; }
        public string PathToStartConfiguration { get; set; } = "StartConfiguration.xml";
        public Configuration(string connectionString, string xmlFilePath) //Указать вручную
        {
            ConnectionString = connectionString;
            CardsXmlFilePath = xmlFilePath;
        }

        //вот здесь че то надо думать. Значения теряются
        public Configuration(string PathToStartConfiguration) //При создании экземпляра принимать в параметр путь к настройкам соединения
        {
            this.PathToStartConfiguration = PathToStartConfiguration;
            LoadFromFile(PathToStartConfiguration);
        }

        public static void FromUserToXml()
        {
            Console.WriteLine("Введите ConnectionString для связи с БД:");
            Console.WriteLine();
            Console.WriteLine("Пример: \nData Source=DESKTOP-HOE58T4;Initial Catalog=usersdb;Integrated Security=True;User ID=admin;Password=admin;TrustServerCertificate=True;\r\n");
            Console.WriteLine();
            string connectionString = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Теперь введите путь к файлу Cards...xml (включая название файла):");
            Console.WriteLine();
            Console.WriteLine("Пример: \nC:\\Users\\Илья\\Downloads\\Cards_20211005080948.xml");
            Console.WriteLine();
            string xmlFilePath = Console.ReadLine();

            Configuration configuration = new Configuration(connectionString, xmlFilePath);
            configuration.SaveToFile();
        }
        public void SaveToFile()
        {
            XDocument xDoc = new XDocument(
                new XElement("StartConfiguration",
                    new XElement("Configuration",
                        new XAttribute("ConnectionString", ConnectionString),
                        new XAttribute("FilePath", CardsXmlFilePath))));

            xDoc.Save("StartConfiguration.xml");
        }

        // 2 метода загрузки настроек из файла StartConfiguration.xml
        public static Configuration LoadFromFile(string PathToStartConfiguration)
        {
            try
            {
                XDocument xdoc = XDocument.Load(PathToStartConfiguration);
                var sConf = xdoc.Element("StartConfiguration")?
                    .Elements("Configuration")
                    .FirstOrDefault();

                if (sConf == null)
                {
                    Console.WriteLine("Ошибка: Не удалось получить конфигурацию.");
                    return null;
                }

                string ConnectionString = sConf.Attribute("ConnectionString")?.Value;
                string CardsXmlFilePath = sConf.Attribute("FilePath")?.Value;
                Configuration config = new Configuration(ConnectionString, CardsXmlFilePath);
                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке конфигурации: {ex.Message}");
                return null;
            }
        }

        //На этом моменте значения потеряны
        public string GetConnection()
        {
            return ConnectionString;//null
        }
        public string GetPath()
        {
            return CardsXmlFilePath;//null
        }
    }
}
