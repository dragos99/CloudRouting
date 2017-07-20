using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp
{
    public class ReceiveManagerLoginDto
    {
        public string customerKey;
    }

    public class ReceiveDriverLoginDto
    {
        public string customerKey;
        public string driverId;
    }

    public class SendDriverDto
    {
        public string driverId;
    }
}
