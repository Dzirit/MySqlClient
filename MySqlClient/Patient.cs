using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlClient
{
    class Patient
    {

        public Guid PublicIdGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string DateOfRegistration { get; set; }
        public string Sex { get; set; }
        public string Institute { get; set; }
        public string Department { get; set; }
        public string Doctor { get; set; }
        public string Id { get; set; }
        public string Node { get; set; }
    }
}
