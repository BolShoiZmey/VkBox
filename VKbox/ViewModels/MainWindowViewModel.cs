using System;
using System.Collections.Generic;
using System.Linq;
using Catel.Collections;
using VKbox.Models;

namespace VKbox.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class MainWindowViewModel : ViewModelBase
    {
        public Settings Settings { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public Command SyncCommand { get; set; }
        public string Status { get; set; }
     
        public MainWindowViewModel(Settings settings)
        {
            Settings = settings;
            Users = new ObservableCollection<User>();
            SyncCommand =new Command(() =>
            {            
                var vk = new Vk(settings.Login,settings.Pass);
                vk.VkLogin();    
                vk.GetFriends();
                Users.Clear();
                Users.AddRange(vk.Friends);
                Status = vk.ReadyCheck() ? "Online!" : "Offline :(";
            });
        }
       
    }
}
