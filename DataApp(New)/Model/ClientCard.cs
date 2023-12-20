using System;
using System.Collections.Generic;
using System.ComponentModel;

public class ClientCard : INotifyPropertyChanged
{
    public int Id { get; set; }
    private string _cardCode;
    private DateTime? _startDate;
    private DateTime? _finishDate;
    private string _lastName;
    private string _firstName;
    private string _surname;
    private string _gender;
    private DateTime? _birthday;
    private string _phoneHome;
    private string _phoneMobil;
    private string _email;
    private string _city;
    private string _street;
    private string _house;
    private string _apartment;
    private bool _isModified;

    public string CardCode
    {
        get { return _cardCode; }
        set
        {
            if (_cardCode != value)
            {
                _cardCode = value;
                OnPropertyChanged(nameof(CardCode));
            }
        }
    }

    public DateTime? StartDate
    {
        get { return _startDate; }
        set
        {
            if (_startDate != value)
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
    }

    public DateTime? FinishDate
    {
        get { return _finishDate; }
        set
        {
            if (_finishDate != value)
            {
                _finishDate = value;
                OnPropertyChanged(nameof(FinishDate));
            }
        }
    }

    public string LastName
    {
        get { return _lastName; }
        set
        {
            if (_lastName != value)
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
    }

    public string FirstName
    {
        get { return _firstName; }
        set
        {
            if (_firstName != value)
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
    }

    public string Surname
    {
        get { return _surname; }
        set
        {
            if (_surname != value)
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
    }

    public string Gender
    {
        get { return _gender; }
        set
        {
            if (_gender != value)
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
    }

    public DateTime? Birthday
    {
        get { return _birthday; }
        set
        {
            if (_birthday != value)
            {
                _birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
        }
    }

    public string PhoneHome
    {
        get { return _phoneHome; }
        set
        {
            if (_phoneHome != value)
            {
                _phoneHome = value;
                OnPropertyChanged(nameof(PhoneHome));
            }
        }
    }

    public string PhoneMobil
    {
        get { return _phoneMobil; }
        set
        {
            if (_phoneMobil != value)
            {
                _phoneMobil = value;
                OnPropertyChanged(nameof(PhoneMobil));
            }
        }
    }

    public string Email
    {
        get { return _email; }
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public string City
    {
        get { return _city; }
        set
        {
            if (_city != value)
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }
    }

    public string Street
    {
        get { return _street; }
        set
        {
            if (_street != value)
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }
    }

    public string House
    {
        get { return _house; }
        set
        {
            if (_house != value)
            {
                _house = value;
                OnPropertyChanged(nameof(House));
            }
        }
    }

    public string Apartment
    {
        get { return _apartment; }
        set
        {
            if (_apartment != value)
            {
                _apartment = value;
                OnPropertyChanged(nameof(Apartment));

            }
        }
    }

    public bool IsModified
    {
        get { return _isModified; }
        set
        {
            if (_isModified != value)
            {
                _isModified = value;
                OnPropertyChanged(nameof(IsModified));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        IsModified = true;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}
