using System;

namespace CertificationCenter.Models {
    public class Certification {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DatetimeStart { get; set; }
        public DateTime DatetimeEnd { get; set; }
        public bool IsActive { get; set; }
    }
}