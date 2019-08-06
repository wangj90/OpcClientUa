using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Opc.Ua.Client;
using OpcClientUa.Data;
using OpcClientUa.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpcClientUa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataReaderController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public DataReaderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IList<OpcItem>> GetAsync()
        {
            Session session = await OpcData.GetOpcDataAsync(_configuration.GetSection("EndpointUrl").Value);
            return new List<string>
                    {
                        "Data Type Examples.16 Bit Device.R Registers.Boolean2",
                        "Data Type Examples.16 Bit Device.R Registers.Double2",
                        "Data Type Examples.16 Bit Device.R Registers.DWord2"
                    }
                    .Select(i =>
                        {
                            var dataValue = session.ReadValue("ns=2;s=" + i);
                            return new OpcItem
                            {
                                ItemId = i,
                                Value = dataValue.Value,
                                Quality = dataValue.StatusCode.ToString(),
                                TimeStamp = dataValue.SourceTimestamp.ToString("yyyy-MM-dd HH:mm:ss")
                            };
                        })
                    .ToList();
        }
    }
}