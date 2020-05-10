using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class Notify
    {
        String to;
        string Priority = "high";
        bool Content_available = true;
        Notification Notification;
        Data Data;
    }
    public class Data
    {
        string Body;
        string Title;
    }
    public class Notification
    {
        string Body;
        string Title;
        int Badget;
    }

    public class Account
    {
        string Token;
        bool IsActive;
    }

    public class NotifyModel
    {
        public int Id { get; set; }
        public int TokenId { get; set; }
        public string Title { get; set; }
        public string Messager { get; set; }
        public int Sender { get; set; }
        public int Reviever { get; set; }
        public int IdObject { get; set; }
        public string TypeSend { get; set; }
    }

    public class UserToken
    {
        public string Token;
        public int UserId;
        public string DeviceType;
        public int Id;

    }

    public class GetNotifyModel
    {
        public int Id { get; set; }
        public int IdObject { get; set; }
        public string Title { get; set; }
        public string Messager { get; set; }
        public string SendType { get; set; }

    }





}
