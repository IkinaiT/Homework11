using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework11
{
    interface IChangeUser
    { 
        public User ChangeTo(string surname, string name, string patronymic, string phoneNumber, string passport, User user);
    }
}
