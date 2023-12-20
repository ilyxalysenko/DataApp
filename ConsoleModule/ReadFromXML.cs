using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DataApp_New_.Model;
using static DataApp_New_.Model.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleModule
{
    //Сделать сохранение документа в "Документы" (общее StartConfiguration для приложух)
    class ReadFromXML
    {
        private static Configuration config = new Configuration("StartConfiguration.xml"); 
        static void Main(string[] args)
        {
            Configuration.FromUserToXml();
            ReadFromXML.LoadCardsToSql();
            Console.Read();
        }

        public static void LoadCardsToSql()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(config.GetConnection()))
                {
                    connection.Open();

                    XDocument xmlCards = XDocument.Load(config.GetPath());
                    XElement xCards = xmlCards.Element("Cards");

                    List<ClientCard> listCards = xCards?.Elements("Card")
                        .Select(xCard => new ClientCard
                        {
                            CardCode = XmlAttributeRead(xCard.Attribute("CARDCODE")),
                            StartDate = XmlAttributeDateTimeNullableRead(xCard.Attribute("STARTDATE")),
                            FinishDate = XmlAttributeDateTimeNullableRead(xCard.Attribute("FINISHDATE")),
                            LastName = XmlAttributeRead(xCard.Attribute("LASTNAME")),
                            FirstName = XmlAttributeRead(xCard.Attribute("FIRSTNAME")),
                            Surname = XmlAttributeRead(xCard.Attribute("SURNAME")),
                            Gender = XmlAttributeRead(xCard.Attribute("GENDER")),
                            Birthday = XmlAttributeDateTimeNullableRead(xCard.Attribute("BIRTHDAY")),
                            PhoneHome = XmlAttributeRead(xCard.Attribute("PHONEHOME")),
                            PhoneMobil = XmlAttributeRead(xCard.Attribute("PHONEMOBIL")),
                            Email = XmlAttributeRead(xCard.Attribute("EMAIL")),
                            City = XmlAttributeRead(xCard.Attribute("CITY")),
                            Street = XmlAttributeRead(xCard.Attribute("STREET")),
                            House = XmlAttributeRead(xCard.Attribute("HOUSE")),
                            Apartment = XmlAttributeRead(xCard.Attribute("APARTMENT"))
                        })
                        .ToList();

                    if (listCards != null)
                    {
                        foreach (ClientCard card in listCards)
                        {
                            if (card.StartDate != null)
                            {
                                string query =
                                    $"INSERT INTO ClientsCard (CARDCODE, STARTDATE, FINISHDATE, LASTNAME, FIRSTNAME, SURNAME, GENDER, BIRTHDAY, PHONEHOME, PHONEMOBIL, EMAIL, CITY, STREET, HOUSE, APARTMENT) " +
                                    $"VALUES ('{card.CardCode}', ";

                                // Проверяем, есть ли значение StartDate, чтобы избежать вставки нулевых значений
                                if (card.StartDate != null)
                                {
                                    query += $"'{((DateTime)card.StartDate).Date}', ";
                                }
                                else
                                {
                                    query += "NULL, ";
                                }

                                // Проверяем, есть ли значение FinishDate, чтобы избежать вставки нулевых значений
                                if (card.FinishDate != null)
                                {
                                    query += $"'{((DateTime)card.FinishDate).Date}', ";
                                }
                                else
                                {
                                    query += "NULL, ";
                                }

                                query += $"'{card.LastName}', '{card.FirstName}', '{card.Surname}', '{card.Gender}', ";

                                // Проверяем, есть ли значение Birthday, чтобы избежать вставки нулевых значений
                                if (card.Birthday != null)
                                {
                                    query += $"'{((DateTime)card.Birthday).Date}', ";
                                }
                                else
                                {
                                    query += "NULL, ";
                                }

                                query += $"'{card.PhoneHome}', '{card.PhoneMobil}', '{card.Email}', '{card.City}', '{card.Street}', '{card.House}', '{card.Apartment}')";

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        Console.WriteLine("Данные успешно импортированы в базу данных.");
                    }
                    else
                    {
                        Console.WriteLine("Нет данных для импорта.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при импорте данных: {ex.Message}");
            }
        }

        static string XmlAttributeRead(XAttribute xAttr) => xAttr?.Value ?? string.Empty;

        static DateTime? XmlAttributeDateTimeNullableRead(XAttribute xAttr)
        {
            string value = XmlAttributeRead(xAttr);

            if (DateTime.TryParse(value, out DateTime result))
            {
                if (result == DateTime.MinValue)
                {
                    return null;
                }

                return result;
            }

            return null;
        }
    }
}
