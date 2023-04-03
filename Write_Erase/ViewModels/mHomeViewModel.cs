using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Write_Erase.Services;
using Write_Erase.Views;

namespace Write_Erase.ViewModels
{
    public class mHomeViewModel : BindableBase
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;

        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageButton { get; set; }

        public mHomeViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }
        public AsyncCommand SignInCommand => new(async () =>
        {
            await Task.Run(async () =>
            {
                if (await _userService.AuthorizationAsync(Username, Password))
                {
                    ErrorMessageButton = string.Empty;
                    await Application.Current.Dispatcher.InvokeAsync(async () => _pageService.ChangePage(new ViewItems()));
                }
                else
                {
                    ErrorMessageButton = "Неверный логин или пароль";
                }
            });
        }, bool () =>
        {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Пустые поля";
                    ErrorMessageButton = string.Empty;
                }
                else
                {
                    ErrorMessage = string.Empty;
                }
                if (ErrorMessage.Equals(string.Empty))
                    return true; return false;
        }); 
        public DelegateCommand SignInLaterCommand => new(() => _pageService.ChangePage(new ViewItems()));
        public DelegateCommand RegistrationCommand => new(() => _pageService.ChangePage(new RegistrationPage()));
    }
}
