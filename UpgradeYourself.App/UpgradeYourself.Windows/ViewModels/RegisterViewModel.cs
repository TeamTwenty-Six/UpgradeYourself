namespace UpgradeYourself.Windows.ViewModels
{
    using Parse;
    using System;
    using System.Threading.Tasks;

    public class RegisterViewModel : MainPageViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public async Task<bool> Register()
        {
            if (await this.IsConnectedToInternet())
            {
                try
                {
                    var user = new ParseUser()
                    {
                        Username = this.Username,
                        Password = this.Password
                    };

                    await user.SignUpAsync();
                    await ParseUser.LogInAsync(this.Username, this.Password);
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
