using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IEmailService
    {
        public void SendResetPasswordMail(string userId, string mail);

        public void SendPassword(string mail, string password);
    }
}
