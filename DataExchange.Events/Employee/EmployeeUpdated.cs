using System;
using System.Collections.Generic;
using System.Text;

namespace DataExchange.Events.Employee
{
    public class EmployeeUpdated : DataExchangeEvent
    {
        public Employee Employee { get; set; }

        public override string GetPartitionKey()
        {
            return GetEmployeeId(Employee.EmployeeId);
        }

        private string GetEmployeeId(int Id)
        {
            return "APP" + "-" + Id;
        }
    }
}
