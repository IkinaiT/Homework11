using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Homework11
{
    class Consultant : Employee, IChangeUser
    {

        public User ChangeTo(string surname, string name, string patronymic, string phoneNumber, string passport, User user)
        {
            {
                string changedData = "";
                if (user.Surname != surname)
                    changedData += "Фамилия";

                if (user.Name != name)
                    changedData += " имя";

                if (user.Patronymic != patronymic)
                    changedData += " отчество";

                if (user.PhoneNumber.ToString() != phoneNumber)
                    changedData += " номер телефона";

                if (user.Passport != passport)
                    changedData += " паспорт";

                return new User(surname, name, patronymic, phoneNumber, passport, DateTime.Now.ToString(), 
                                this.Surname + " " + this.Name + " " + this.Patronymic + ", консультант, " + changedData);
            }
        }

        public Consultant(string surname, string name, string patronymic)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
        }

        
        
    }
}
