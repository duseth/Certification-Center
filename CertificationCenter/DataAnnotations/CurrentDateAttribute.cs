using System;
using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.DataAnnotations {
    public class CurrentDateAttribute : ValidationAttribute {
        public CurrentDateAttribute() {
        }

        public override bool IsValid(object value) {
            if (value != null) {
                var dt = (DateTime) value;
                if (dt > DateTime.Now) {
                    return true;
                }
            }

            return false;
        }
    }
}