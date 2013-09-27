using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect.Classes;

namespace Connect.Classes
{
    public class LoggedUser
    {
        private static LoggedUser instance;

        private LoggedUser() { }
        private UserData user;

        public static LoggedUser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoggedUser();
                    Session s = new Session();
                    if (s.Contains("Id"))
                    {
                        instance.user = new UserData();
                        instance.user.Email = s.GetStringObject("Email");
                        instance.user.Id = s.GetStringObject("Id");
                        instance.user.FacebookId = s.GetStringObject("FacebookId");
                        instance.user.LinkedInId = s.GetStringObject("LinkedInId");
                        instance.user.Name = s.GetStringObject("Name");
                        instance.user.Password = s.GetStringObject("Password");
                    }
                    else
                    {
                        instance.user = null;
                    }
                }
                return instance;
            }
        }

        public void SetLoggedUser(UserData u)
        {
            this.user = u;
            Session session = new Session();
            session.SaveStringObject("Email", u.Email);
            session.SaveStringObject("FacebookId", u.FacebookId);
            session.SaveStringObject("Id", u.Id);
            session.SaveStringObject("LinkedInId", u.LinkedInId);
            session.SaveStringObject("Name", u.Name);
            session.SaveStringObject("Password", u.Password);
        }

        public UserData GetLoggedUser()
        {
            return this.user;
        }

        public void LogOut()
        {            
            this.user = null;
            Session session = new Session();
            session.RemoveStringObject("Email");
            session.RemoveStringObject("FacebookId");
            session.RemoveStringObject("Id");
            session.RemoveStringObject("LinkedInId");
            session.RemoveStringObject("Name");
            session.RemoveStringObject("Password");            
        }
    }
}
