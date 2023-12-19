using System.Data;
using System.Xml.Linq;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Xml;
using System;
using DataApp_New_.Model;
using System.Linq;
public class ModuleXmlToSql
{
    
    public static void ConfigFromConsoleToXml() // Метод загрузки настроек в XML
        //Параметры запуска сохранятся в XML
    {

        //Считываем с консоли
        Console.WriteLine($"Введите ConnectionString, по которому будет установлена связь с БД");
        Console.WriteLine("Пример: Data Source=DESKTOP-HOE58T4;Initial Catalog=usersdb;Integrated Security=True;User ID=admin;Password=admin;");
        string str = new ConnectionString(Console.ReadLine()).ToString();
        Console.WriteLine($"Теперь введите путь к файлу Cards...xml (Включая название файла)");
        Console.WriteLine(@"Пример: C:\Users\Илья\Downloads\Cards_20211005080948.xml");
        string xmlFilePath = Console.ReadLine();

        XDocument xDoc = new XDocument(       //Write to XML
            new XElement("StartConfiguration",
            new XElement("Configuration",
                new XAttribute
        ("ConnectionString", $"{str}")),                         //Обозначаем строку подключения как атрибут конфигурации
                new XAttribute("FilePath", $"{xmlFilePath}")));  //Обозначаем путь к файлу как атрибут конфигурации
        xDoc.Save("StartConfiguration.xml");
    }
    public static void LoadToSql()
    {
        string xmlFilePath;
        string str;

        
        XDocument xdoc = XDocument.Load("StartConfiguration.xml");

        var sConf = xdoc.Element("StartConfiguration")?
            .Elements("Configuration")
            .FirstOrDefault();

        if (sConf != null)
        {
            str = sConf.Attribute("ConnectionString")?.Value;
            xmlFilePath = sConf.Attribute("FilePath")?.Value;
        }
        else
        {
            Console.WriteLine("Ошибка: Не удалось получить конфигурацию.");
            return;
        }
        // Загружаем XML-документ
        XmlDocument xCards = new XmlDocument();
        try
        {
            xCards.Load(xmlFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
            return;
        }
        
        // Импорт данных из XML в базу данных
        ImportXmlToDatabase(xmlFilePath, str);
    }
    static void ImportXmlToDatabase(string path, string str)
    {
        
        try
        {
            // Создаем подключение к базе данных
            using (SqlConnection connection = new SqlConnection(str))
            {
                connection.Open();

                try
                {
                    // Операции с базой данных
                    

                    XDocument xmlCards = XDocument.Load("Cards_20211005080948.xml"); //load
                    Console.WriteLine("Doc has loaded");

                    XElement xCards = xmlCards.Element("Cards");      //root
                    Console.WriteLine("xCards has rooted");

                    List<ClientCard> listCards = new List<ClientCard>(); // Создаем список КлиентКарт

                    // Проходим по каждому элементу card
                    foreach (XElement xCard in xCards.Elements("Card")) // для каждого элемента Карта
                    {
                        listCards.Add(new ClientCard() //в листКарт добавить новую КлиентКарту
                        {
                            CardCode = XAttrRead(xCard.Attribute("CARDCODE")), //код карты экземпляра = аттрибуту из хмл
                            StartDate = XAttrReadDateTime(xCard.Attribute("STARTDATE")), // -/-
                            FinishDate = XAttrReadNullableDateTime(xCard.Attribute("FINISHDATE")),
                            LastName = XAttrRead(xCard.Attribute("LASTNAME")),
                            FirstName = XAttrRead(xCard.Attribute("FIRSTNAME")),
                            Surname = XAttrRead(xCard.Attribute("SURNAME")),
                            Gender = XAttrRead(xCard.Attribute("GENDER")),
                            Birthday = XAttrReadDateTime(xCard.Attribute("BIRTHDAY")),
                            PhoneHome = XAttrRead(xCard.Attribute("PHONEHOME")),
                            PhoneMobil = XAttrRead(xCard.Attribute("PHONEMOBIL")),
                            Email = XAttrRead(xCard.Attribute("EMAIL")),
                            City = XAttrRead(xCard.Attribute("CITY")),
                            Street = XAttrRead(xCard.Attribute("STREET")),
                            House = XAttrRead(xCard.Attribute("HOUSE")),
                            Apartment = XAttrRead(xCard.Attribute("APARTMENT"))
                        });
                        string XAttrRead(XAttribute xAttr)
                        {
                            return xAttr?.Value ?? string.Empty;
                        } // метод считать хмл атрибут стринг

                        DateTime XAttrReadDateTime(XAttribute xAttr)
                        {
                            string value = XAttrRead(xAttr);

                            if (DateTime.TryParse(value, out DateTime result))
                            {
                                return result;
                            }
                            else
                            {
                                DateTime? nullableDateTime = null;
                                // Обработка ситуации, когда значение не распознается как дата и время
                                // Здесь можно выбрать другое значение по умолчанию или сгенерировать исключение
                                return (DateTime)nullableDateTime;
                            }
                        } // метод считать хмл атрибут датавремя

                        DateTime? XAttrReadNullableDateTime(XAttribute xAttr)
                        {
                            string value = XAttrRead(xAttr);
                            if (DateTime.TryParse(value, out DateTime result))
                            {
                                return result;
                            }
                            return null;
                        }
                        // метод считать хмл атрибут если пустое датавремя


                    }

                    foreach (ClientCard card in listCards) //для каждой карты клинты из списка
                    {
                        // Выполняем SQL-запрос для вставки данных в базу данных
                        string query =
                            $"INSERT INTO ClientsCard (CARDCODE, " +
                                                     $"STARTDATE, " +
                                                     $"FINISHDATE, " +
                                                     $"LASTNAME, " +
                                                     $"FIRSTNAME, " +
                                                     $"SURNAME, " +
                                                     $"GENDER, " +
                                                     $"BIRTHDAY, " +
                                                     $"PHONEHOME, " +
                                                     $"PHONEMOBIL, " +
                                                     $"EMAIL, " +
                                                     $"CITY, " +
                                                     $"STREET, " +
                                                     $"HOUSE, " +
                                                     $"APARTMENT) " +

                            $"VALUES ('{card.CardCode}' " +
                                    $"'{card.StartDate}' " +
                                    $"'{card.FinishDate}' " +
                                    $"'{card.LastName}' " +
                                    $"'{card.FirstName}' " +
                                    $"'{card.Surname}' " +
                                    $"'{card.Gender}' " +
                                    $"'{card.Birthday}' " +
                                    $"'{card.PhoneHome}' " +
                                    $"'{card.PhoneMobil}' " +
                                    $"'{card.Email}' " +
                                    $"'{card.City}' " +
                                    $"'{card.Street}' " +
                                    $"'{card.House}' " +
                                    $"'{card.Apartment}')";

                        using (SqlCommand command = new SqlCommand(query, connection)) //Загружаем в SQL DB
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    Console.WriteLine("Данные успешно импортированы в базу данных.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при импорте данных: {ex.Message}");
                }
                finally
                {
                    // Закрытие соединения в любом случае
                    connection.Close();
                }


            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при импорте данных: {ex.Message}");
        }
        Console.ReadLine();
    }
}
