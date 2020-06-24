using OpcClientUa.Models;
using System.Collections.Generic;

namespace OpcClientUa.Middleware
{
    public class OpcPointOptions
    {
        public IList<OpcSetting> OpcPoints { get; set; }
    }
}