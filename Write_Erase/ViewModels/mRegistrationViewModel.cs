using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Write_Erase.Services;
using Write_Erase.Views;

namespace Write_Erase.ViewModels
{
    public class mRegistrationViewModel : IDataErrorInfo
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;

        public string Surname
        {
            get => _surname;
            set
            {
                _surname= value;
                if (_surname == null || _surname.Equals(""))
                {
                    errors["Surname"] = "Фамилия не должна быть пустой";
                    Debug.WriteLine(errors["Surname"]);

                }
                else if (Char.IsLower(_surname[0]))
                {
                    errors["Surname"] = "Фамилия должна начинаться с заглавной буквы";
                    Debug.WriteLine(errors["Surname"]);
                }
                else
                {
                    errors["Surname"] = null;
                }
            }
        }

        public string Name{ get; set; }
        public string Patronymic{ get; set; }
        public string Login{ get; set; }
        public string Password{ get; set; }
        public string ReturnPassword{ get; set; }

        Dictionary<string, string> errors = new Dictionary<string, string>();

        private string _surname;

        public string Error => throw new NotImplementedException();
        public string this[string columnName] => errors.ContainsKey(columnName) ? errors[columnName] : null;

        public bool IsValid => errors.Values.Any(x => x == null);

        public mRegistrationViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }

        public DelegateCommand RegistrationCommand => new(() =>
        {
            _userService.RegistrationAsync(Surname, Name, Patronymic, Login, Password);
            _pageService.ChangePage(new HomePage());
            MessageBox.Show("Вы успешно зарегистрировались!", "Внимание");

        });

        public DelegateCommand BackCommand => new(() => _pageService.ChangePage(new HomePage()));

    }
}
