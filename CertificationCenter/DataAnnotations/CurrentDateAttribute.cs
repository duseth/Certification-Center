using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Net.Sockets;

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

        private void GetDate()
        {
            var client = new TcpClient("time.nist.gov", 13);
            using (var streamReader = new StreamReader(client.GetStream()))
            {
                var response = streamReader.ReadToEnd();
                var utcDateTimeString = response.Substring(7, 17);
                var localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
        }
    }
}