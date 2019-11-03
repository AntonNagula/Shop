using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Auto.ModelsApp;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using DomainCore.Interfaces;
namespace Auto.Hubs
{
    //public class ChatHub : Hub
    //{
    //    static List<User> Users = new List<User>();
    //    IService Service;
    //    public ChatHub(IService Unit)
    //    {
    //        Service = Unit;
    //    }
    //    // Отправка сообщений
    //    public void Send(string name, string message)
    //    {
    //        var user = Users.FirstOrDefault(x=>x.Name==name);
    //        if (user!=null)
    //        {
    //            Clients.Client(user.ConnectionId).addMessage(name, message);
    //        }
    //        //Service.CreateMessage(user.IdSpeach,name,message);
    //    }

    //    // Подключение нового пользователя
    //    public void Connect(int chatid, string userName)
    //    {
    //        var id = Context.ConnectionId;


    //        if (!Users.Any(x => x.ConnectionId == id))
    //        {
    //            Users.Add(new User { ConnectionId = id, Name = userName, IdSpeach=chatid });

    //            // Посылаем сообщение текущему пользователю
    //            Clients.Caller.onConnected(id, userName, Users);

    //            // Посылаем сообщение всем пользователям, кроме текущего
    //            Clients.AllExcept(id).onNewUserConnected(id, userName);
    //        }
    //    }

    //    // Отключение пользователя
    //    public override Task OnDisconnected(bool stopCalled)
    //    {
    //        var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
    //        if (item != null)
    //        {
    //            Users.Remove(item);
    //            var id = Context.ConnectionId;
    //            Clients.All.onUserDisconnected(id, item.Name);
    //        }

    //        return base.OnDisconnected(stopCalled);
    //    }
    //}




    public class ChatHub : Hub
    {
        static List<User> Users = new List<User>();
        
        // Отправка сообщений
        public void Send(string name, string message)
        {            
            Clients.All.addMessage(name, message);
        }

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;


            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new User { ConnectionId = id, Name = userName });

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName);

                // Посылаем сообщение всем пользователям, кроме текущего
                //Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }

        // Отключение пользователя
        public override Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}