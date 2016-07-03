using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace www.BankBals.Classes {
    public class FileUploader2 {
        public IEnumerable<string> GetMessages() {
            for (int i = 0; i < 10; i++) {
                yield return "Line " + i;
                Thread.Sleep(1000);
            }
        }
    }
}




    
