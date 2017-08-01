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

	public class ReceiveOrderDto
	{
		public string ordeType;
		public string streetName;
		public string streetNumber;
		public string cityName;
		public string countryCode;
		public float latitude;
		public float longitude;
		public int fixedDurationInSec;
		public string timeWindowFrom;
		public string timeWindowTill;
	}
}
