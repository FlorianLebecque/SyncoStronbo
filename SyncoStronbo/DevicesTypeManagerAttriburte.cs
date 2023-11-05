using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncoStronbo {
    internal class DevicesTypeManagerAttriburte: Attribute {

        public string id;
        public DevicesTypeManagerAttriburte(string id) {
            this.id = id;
        }
    }
}
