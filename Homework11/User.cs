using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework11
{
    class User
    {
        public string Surname { get; }
        public string Name { get; }
        public string Patronymic { get; }
        public long PhoneNumber { get; }
        public string Passport { get; }
        public string ChangedDT { get; }
        public string ChangedEmplpoyee { get; }


        //public string GetInformation(Employee employee)
        //{
        //    string passport = (employee is Consultant) ? "**** ******" : Passport;
        //    string info = Surname + Name + Patronymic + PhoneNumber + passport;
        //    return info;
        //}

        public User(string surname, string name, string patronymic, string phone, string passport, string changedDT, string changedEmployee)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            PhoneNumber = long.Parse(phone);
            Passport = passport;
            ChangedDT = changedDT;
            ChangedEmplpoyee = changedEmployee;
        }
    }
}
