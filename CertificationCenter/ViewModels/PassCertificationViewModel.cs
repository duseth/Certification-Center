using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificationCenter.ViewModels
{
    public class PassCertificationViewModel
    {
        public string IdCertification { get; set; }
        public  string IdUser { get; set; }
        public string Name { get; set; }
        public List<string> Questions { get; set; }
        public List<AnswerViewModel> Answer { get; set; }
    }
}
