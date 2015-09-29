using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GmailSystem
{
    public class BusinessLayer
    {
        Global g1 = new Global();
        public string UserLogIn(string Email, string Password)
        {
            if (g1.CheckEmailIdExistsOrNot(Email)>0)
            {
                g1 = new Global();
                if ((g1.ReturnPasswordByEmail(Email)).Equals(Password))
                    return "Password matches";
                else
                    return "Incorrect password";
                
            }
            else
                return "EmailId doesn't exist.";
        }

    }
}