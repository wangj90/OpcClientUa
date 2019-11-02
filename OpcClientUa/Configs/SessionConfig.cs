using Microsoft.Extensions.Configuration;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using OpcClientUa.Configs;
using System;
using System.Threading.Tasks;

namespace OpcClientUa.Configs
{
    public class SessionConfig
    {
        private const string ApplicationName = "UA Client";
        public static async Task<Session> GetOpcSessionAsync(string EndpointUrl)
        {
            var config = await ApplicationConfig.Load(ApplicationName);
            var application = new ApplicationInstance
            {
                ApplicationName = ApplicationName,
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = config
            };

            var haveAppCertificate = await application.CheckApplicationInstanceCertificate(false, 0);
            if (!haveAppCertificate)
            {
                throw new Exception("应用程序实例证书无效!");
            }
            if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                config.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
            }
            config.ApplicationUri = Utils.GetApplicationUriFromCertificate(config.SecurityConfiguration.ApplicationCertificate.Certificate);

            var selectedEndpoint = CoreClientUtils.SelectEndpoint(EndpointUrl, true, 15000);

            var endpointConfiguration = EndpointConfiguration.Create(config);
            var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);
            return await Session.Create(config, endpoint, false, ApplicationName, 60000, new UserIdentity(new AnonymousIdentityToken()), null);
        }
    }
}
