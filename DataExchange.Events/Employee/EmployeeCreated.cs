using System;
using System.Collections.Generic;
using System.Text;

namespace DataExchange.Events.Employee
{
    public class EmployeeCreated : DataExchangeEvent
    {
        public Employee Employee { get; set; }

        public override string GetPartitionKey()
        {
            return GetEmployeeId(Employee.EmployeeId);
        }

        private string GetEmployeeId(int Id = 1)
        {
            return "APP" + "-" + Id;
        }
    }
}
