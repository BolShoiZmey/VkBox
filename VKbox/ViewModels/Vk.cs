using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xNet;
using static System.String;

namespace VKbox.ViewModels
{
    class Vk
    {
        private const string ClientId = "5627888";
        private const string RedirectId = "https://oauth.vk.com/blank.html";
        private const string SecretKey = "bxhLTbrE2Adjw8ZsQpXT";
        public string Token;
        private readonly HttpRequest _request;
        public ObservableCollection<User> Friends = new ObservableCollection<User>();
        private readonly string _name;
        private readonly string _pass;

        public Vk(string name, string pass)
        {
            _name = name;
            _pass = pass;
            _request = new HttpRequest
            {
                UserAgent = Http.ChromeUserAgent(),
                Cookies = new CookieDictionary()
            };
        }
        public string RequestToken()
        {
            var response =
                _request.Get(
                    $"https://oauth.vk.com/authorize?client_id={ClientId}&redirect_uri={RedirectId}&scope=friends,messages,offline&response_type=token&v=5.53");
            return _request.Address.ToString().Substring("token=", "&");
        }
        public void VkLogin()
        {

            //var response =
            //  _request.Get(
            //     "vk.com").ToString();
            //var rp = new RequestParams
            //{
            //    ["act"] = "login",
            //    ["role"] = "al_frame",
            //    ["ip_h"] = response.Substring("ip_h\" value=\"", "\""),
            //    ["lg_h"] = response.Substring("lg_h\" value=\"", "\""),
            //    ["_origin"] = "https://vk.com",
            //    ["captcha_sid"] = "",
            //    ["captcha_key"] = "",
            //    ["email"] = _name,
            //    ["pass"] = _pass,
            //    ["expire"] = "0"
            //};
            //var result = _request.Post("https://login.vk.com/", rp).ToString();
            //var testdrive = _request.Get("https://vk.com/").ToString();
            /////////////////////////////////////////////////////// - без Api, всё работает.

            var reqParams = new RequestParams();
            var response =
                _request.Get(
                    $"https://oauth.vk.com/authorize?client_id={ClientId}&redirect_uri={RedirectId}&scope=friends,messages,offline&response_type=token&v=5.53");
            var respTxt = response.ToString();
            reqParams["ip_h"] = respTxt.Substring("ip_h\" value=\"", "\"");
            reqParams["lg_h"] = respTxt.Substring("lg_h\" value=\"", "\"");
            reqParams["_origin"] = respTxt.Substring("_origin\" value=\"", "\"");
            reqParams["to"] = respTxt.Substring("to\" value=\"", "\"");
            reqParams["expire"] = "0";
            reqParams["email"] = _name;
            reqParams["pass"] = _pass;
            var result = _request.Post($"https://login.vk.com/?act=login&soft=1", reqParams).ToString();
            Token = _request.Address.ToString().Substring("token=", "&");
        }
        public bool ReadyCheck()
        {
            return !IsNullOrEmpty(Token);
        }
        public void GetFriends()
        {
            var resp =
                _request.Get(
                    $"https://api.vk.com/method/friends.get.xml?fields=nickname,domain&access_token={Token}&count=10")
                    .ToString();
          var usersString = resp.Substrings("<user>", "</user>");
            foreach (var userString in usersString)
            {
                var id = userString.Substring("<uid>", "</uid>");
                var name = userString.Substring("<first_name>", "</first_name>");
                var lastName = userString.Substring("<last_name>", "</last_name>");
                Friends.Add(new User(id, name, lastName));
            }
        }
        public string[] GetHistory(string id)
        {
            var resp =
                _request.Get(
                    $"https://api.vk.com/method/messages.getHistory.xml?access_token={Token}&count=15&user_id={id}")
                    .ToString();
            return resp.Substrings("<body>", "</body>");
        }
    }
}
