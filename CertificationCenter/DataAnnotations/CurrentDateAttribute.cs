using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Net.Sockets;

namespace CertificationCenter.DataAnnotations {
    /// <summary>
    /// Проверяющий элемент управления данных.
    /// </summary>
    public class CurrentDateAttribute : ValidationAttribute {
        /// <summary>
        /// Метод для проверки правильности данных.
        /// </summary>
        /// <param name="value">Объект для проверки.</param>
        /// <returns>"true" в случае правильности данных, иначе "false".</returns>
        public override bool IsValid(object value) {
            if (value != null) {
                var dt = (DateTime) value;
                if (dt > GetDate()) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Метод получения текущей даты и времени.
        /// </summary>
        /// <returns>Объект DateTime с текущим датой и временем.</returns>
        private DateTime GetDate() {
            var client = new TcpClient("time.nist.gov", 13);
            using (var streamReader = new StreamReader(client.GetStream())) {
                var response = streamReader.ReadToEnd();
                var utcDateTimeString = response.Substring(7, 17);
                return DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal);
            }
        }
    }
}