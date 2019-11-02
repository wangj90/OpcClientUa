using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Opc.Ua;
using Opc.Ua.Client;
using OpcClientUa.Configs;
using OpcClientUa.Models;
using OpcClientUa.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpcClientUa.Middleware
{
    public class OpcMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly OpcPointOptions _options;

        private readonly IConfiguration _configuration;

        private readonly IHubContext<OpcHub, IOpcClient> _hubContext;

        public OpcMiddleware(RequestDelegate next, IOptions<OpcPointOptions> options,
            IConfiguration configuration, IHubContext<OpcHub, IOpcClient> hubContext)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            this._next = next ?? throw new ArgumentNullException("next");
            this._options = options.Value;
            this._configuration = configuration ?? throw new ArgumentNullException("configuration");
            this._hubContext = hubContext ?? throw new ArgumentNullException("hubContext");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Session session = SessionConfig.GetOpcSessionAsync(_configuration.GetSection("EndpointUrl").Value).Result;
            Subscription subscription = new Subscription(session.DefaultSubscription);
            IEnumerable<MonitoredItem> items = _options.OpcPoints
                .Select(i =>
                {
                    MonitoredItem item = new MonitoredItem
                    {
                        DisplayName = i,
                        StartNodeId = "ns=2;s=" + i
                    };
                    item.Notification += (MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e) =>
                    {
                        if (e.NotificationValue is MonitoredItemNotification notification)
                        {
                            var value = notification.Value;
                            OpcItem opcItem = new OpcItem
                            {
                                ItemId = monitoredItem.DisplayName,
                                Value = value.Value,
                                Quality = value.StatusCode.ToString(),
                                TimeStamp = value.SourceTimestamp.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")
                            };
                            _hubContext.Clients.All.OpcDataUpdate(opcItem);
                        }
                    };
                    return item;
                }
             );
            subscription.AddItems(items);
            session.AddSubscription(subscription);
            subscription.Create();
            await _next(context);
        }
    }
}
