using System;

namespace HelloServerless.Events
{
    public class LoanApplicationAccepted
    {
        public string Name { get; set; }
        public DateTime AcceptedDate { get; set; }
    }
}