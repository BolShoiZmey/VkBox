using System;
using System.Collections.Generic;
using Catel.Collections;

namespace VKbox.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class MainWindowViewModel : ViewModelBase
    {
        //private ObservableCollection<string> something = new ObservableCollection<String> { "A", "B", "C", "D", "E", "F" };     
        //public ObservableCollection<String> Something // Must be property or DP to be bound!
        //{
        //    get { return something; }
        //    set
        //    {
        //        if (Equals(value, something)) return;
        //        something = value;
        //        RaisePropertyChanged("Something");
        //    }
        //}
        public ObservableCollection<User> Users { get; set; }
        public Command SyncCommand { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Status { get; set; }
     
        public MainWindowViewModel()
        {
            SyncCommand=new Command(() =>
            {
                //Users = new ObservableCollection<User>();
                var vk = new Vk(Name,Pass);
                vk.VkLogin();    
                vk.GetFriends();
                //Users.Clear();
                //Users = vk.Friends;
                Status = vk.ReadyCheck() ? "Online!" : "Offline :(";
            });
        }
       
    }
}
