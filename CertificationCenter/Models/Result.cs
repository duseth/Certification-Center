using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificationCenter.Models
{
    public class Result
    {
        public string Id { get; set; }
        public string UserId { get; set;}
        public string CertificationId { get; set;}
        public User User { get; set; }
        public Certification Certification { get; set; }

    }
}
