using DigitalNomadApp.DataModels;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalNomadApp.Helpers
{
    public class  OutputType
    {
        [SqlOutput("dbo.user", connectionStringSetting: "SqlConnectionString")]
        public UserDetails userDetails { get; set; }
    }
}
