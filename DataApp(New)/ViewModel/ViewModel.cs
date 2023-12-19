using System;
using System.Linq;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DataApp_New_.Model;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Data.Common;
using System.Windows.Controls;
using System.Xml.Linq;


namespace DataApp_New_.ViewModel
{
    public class ViewModel
    {

        private Configuration config = new Configuration("StartConfiguration.xml"); //!!! Инициализируем поле configConnectionString
        

        private ObservableCollection<ClientCard> _cards; //Объявление приватного поля

        private bool _dataChanged;
        public bool DataChanged //(да/нет) Данные изменены. - Публичное поле класса ViewModel
        {
            get { return _dataChanged; }
            set
            {
                if (_dataChanged != value)
                {
                    _dataChanged = value;
                    OnPropertyChanged(nameof(DataChanged));
                }
            }
        }



        public ObservableCollection<ClientCard> Cards //Объявление свойства Cards с геттером и сеттером для доступа к _cards и оповещения об изменениях.
        {
            get { return _cards; }
            set
            {
                if (_cards != value)
                {
                    _cards = value;
                    OnPropertyChanged(nameof(Cards));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged; //

        protected virtual void OnPropertyChanged(string propertyName) //Метод для вызова события PropertyChanged, уведомляющий об изменении свойств.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ViewModel() //Конструктор класса ViewModel, инициализирующий поле configConnectionString и вызывающий метод DownloadDataFromSQL
        {
            
            Cards = new ObservableCollection<ClientCard>();
            DownloadDataFromSQL();
        }


        public void DownloadDataFromSQL()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(config.GetConnection()))//!!!
                {
                    connection.Open();

                    string query = "SELECT * FROM dbo.ClientsCard";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    Cards.Add(new ClientCard
                                    {
                                        CardCode = reader["CardCode"].ToString(),
                                        StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? (DateTime?)null : (DateTime)reader["StartDate"],
                                        FinishDate = reader.IsDBNull(reader.GetOrdinal("FinishDate")) ? (DateTime?)null : (DateTime)reader["FinishDate"],
                                        LastName = reader["LastName"] as string,
                                        FirstName = reader["FirstName"] as string,
                                        Surname = reader["Surname"] as string,
                                        Gender = reader["Gender"] as string,
                                        Birthday = reader.IsDBNull(reader.GetOrdinal("Birthday")) ? (DateTime?)null : (DateTime)reader["Birthday"],
                                        PhoneHome = reader["PhoneHome"] as string,
                                        PhoneMobil = reader["PhoneMobil"] as string,
                                        Email = reader["Email"] as string,
                                        City = reader["City"] as string,
                                        Street = reader["Street"] as string,
                                        House = reader["House"] as string,
                                        Apartment = reader["Apartment"] as string
                                    });
                                }
                                catch (InvalidCastException ex)
                                {
                                    Debug.WriteLine($"Ошибка преобразования типа данных. Имя поля: {reader.GetName(reader.GetOrdinal("Birthday"))}. Ошибка: {ex}");
                                    // Пробросим исключение дальше
                                    throw;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при загрузке данных из базы данных: {ex}");
                MessageBox.Show($"Ошибка при загрузке данных из базы данных: {ex.Message}");
            }
        } //Метод для загрузки данных из базы данных SQL Server в коллекцию Cards
        public void SaveChangesToDatabase()
        {
            using (SqlConnection connection = new SqlConnection(config.GetConnection()))
            {
                connection.Open();

                foreach (ClientCard modifiedCard in Cards.Where(c => c.IsModified))
                {
                    if (string.IsNullOrEmpty(modifiedCard.CardCode)) // Если CardCode пустой, это новый клиент
                    {
                        AddNewClientToDatabase(modifiedCard, connection);
                    }
                    else
                    {
                        UpdateClientInDatabase(modifiedCard, connection);
                    }
                }

                DataChanged = false;
            }
        }

        public void UpdateClientInDatabase(ClientCard modifiedCard, SqlConnection connection) //обновляет после отдельного добавления клинта (в др окне) (Еще в разработке)
        {
            string query =
                "UPDATE dbo.ClientsCard SET " +
                "StartDate = @StartDate, " +
                "FinishDate = @FinishDate, " +
                "LastName = @LastName, " +
                "FirstName = @FirstName, " +
                "Surname = @Surname, " +
                "Gender = @Gender, " +
                "Birthday = @Birthday, " +
                "PhoneHome = @PhoneHome, " +
                "PhoneMobil = @PhoneMobil, " +
                "Email = @Email, " +
                "City = @City, " +
                "Street = @Street, " +
                "House = @House, " +
                "Apartment = @Apartment " +
                "WHERE CardCode = @CardCode";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartDate", modifiedCard.StartDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FinishDate", modifiedCard.FinishDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastName", modifiedCard.LastName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FirstName", modifiedCard.FirstName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Surname", modifiedCard.Surname ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Gender", modifiedCard.Gender ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Birthday", modifiedCard.Birthday ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneHome", modifiedCard.PhoneHome ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneMobil", modifiedCard.PhoneMobil ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", modifiedCard.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@City", modifiedCard.City ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Street", modifiedCard.Street ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@House", modifiedCard.House ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Apartment", modifiedCard.Apartment ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CardCode", modifiedCard.CardCode ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public void AddNewClientToDatabase(ClientCard newCard, SqlConnection connection) //Метод описывающий добавление нового клиента в БД в новом окне (Не доделанно)
        {
            string query =
                "INSERT INTO dbo.ClientsCard (StartDate, FinishDate, LastName, FirstName, Surname, Gender, Birthday, " +
                "PhoneHome, PhoneMobil, Email, City, Street, House, Apartment, CardCode) " +
                "VALUES (@StartDate, @FinishDate, @LastName, @FirstName, @Surname, @Gender, @Birthday, " +
                "@PhoneHome, @PhoneMobil, @Email, @City, @Street, @House, @Apartment, @CardCode)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartDate", newCard.StartDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FinishDate", newCard.FinishDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastName", newCard.LastName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FirstName", newCard.FirstName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Surname", newCard.Surname ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Gender", newCard.Gender ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Birthday", newCard.Birthday ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneHome", newCard.PhoneHome ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneMobil", newCard.PhoneMobil ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", newCard.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@City", newCard.City ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Street", newCard.Street ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@House", newCard.House ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Apartment", newCard.Apartment ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CardCode", newCard.CardCode ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
        }
    }
}
