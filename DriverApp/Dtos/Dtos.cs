using DriverApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Dtos
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

	public class ReceiveOrdersDto
	{
		public List<Order> orders;
	}


	public class ReceiveTriggerRequestDto
	{
		public string driverId;
	}


	public class SendDriverDto
	{
		public string driverId;
	}

}
