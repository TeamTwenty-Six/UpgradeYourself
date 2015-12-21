using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;

namespace UpgradeYourself.Windows.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        public async Task<bool> IsConnectedToInternet()
        {
            // bool isConnected = NetworkInterface.GetIsNetworkAvailable();
            bool isConnected = true;
            if (!isConnected)
            {
                await new MessageDialog("No internet connection is avaliable.").ShowAsync();
            }
            else
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                NetworkConnectivityLevel connection = InternetConnectionProfile.GetNetworkConnectivityLevel();
                if (connection == NetworkConnectivityLevel.None || connection == NetworkConnectivityLevel.LocalAccess)
                {
                    isConnected = false;
                }
            }

            // return isConnected;
            // Returning true so we can easily test with mobile emulator
            return true;
        }
    }
}
