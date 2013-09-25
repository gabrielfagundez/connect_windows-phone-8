using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    instance.user = null;
                }
                return instance;
            }
        }

        public void SetLoggedUser(UserData u)
        {
            this.user = u;
        }

        public UserData GetLoggedUser()
        {
            return this.user;
        }

    }
}
