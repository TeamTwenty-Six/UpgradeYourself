namespace UpgradeYourself.Windows.ViewModels
{
    using GalaSoft.MvvmLight;
    using Parse;
    using System;
    using System.Threading.Tasks;
    using UpgradeYourself.Common;
    using UpgradeYourself.Models.Models;

    public class LoginPageViewModel : MainPageViewModel
    {
        public LoginPageViewModel()
        {
            this.User = new UserProfile();
        }

        public UserProfile User { get; set; }

        public bool ValidateInput()
        {
            if (!InputValidator.ValidateRegularTextInput(this.User.Username))
            {
                return false;
            }

            if (!InputValidator.ValidatePasswordTextInput(this.User.Password))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Login()
        {
            if (await this.IsConnectedToInternet())
            {
                try
                {
                    await ParseUser.LogInAsync(this.User.Username, this.User.Password);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
