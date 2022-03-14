using System;
using System.Collections.Generic;
using System.Text;

namespace DataExchange.Events.Employee
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime HiredDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
