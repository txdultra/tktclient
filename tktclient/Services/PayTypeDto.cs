using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Services
{
    public class PayTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Sort { get; set; }

        public int ClientType { get; set; }

        public string ClientTypeName { get; set; }

        public string Note { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEnabled { get; set; }

        public int CompanyID { get; set; }

        public bool IsCanUseToReturn { get; set; }

        public bool IsOnline { get; set; }
    }
}
