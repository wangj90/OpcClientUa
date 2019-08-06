using System.Net;
using System.Threading.Tasks;
using Opc.Ua;

namespace OpcClientUa.Configs
{
    public static class ApplicationConfig
    {
        public static async Task<ApplicationConfiguration> Load(string applicationName)
        {
            var configuration = new ApplicationConfiguration
            {
                ApplicationName = applicationName,
                ApplicationUri = Utils.Format(@"urn:{0}:{1}", Dns.GetHostName(), applicationName),
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier
                    {
                        StoreType = CertificateStoreType.X509Store,
                        StorePath = @"CurrentUser\My",
                        SubjectName = Utils.Format(@"CN={0}, DC={1}", applicationName, Dns.GetHostName())
                    },
                    TrustedIssuerCertificates = new CertificateTrustList
                    {
                        StoreType = CertificateStoreType.Directory,
                        StorePath = Utils.Format(@"%LocalApplicationData%/{0}/pki/issuer", applicationName)
                        
                    },
                    TrustedPeerCertificates = new CertificateTrustList
                    {
                        StoreType = CertificateStoreType.Directory,
                        StorePath = Utils.Format(@"%LocalApplicationData%/{0}/pki/trusted", applicationName)
                    },
                    RejectedCertificateStore = new CertificateTrustList
                    {
                        StoreType = CertificateStoreType.Directory,
                        StorePath = Utils.Format(@"%LocalApplicationData%/{0}/pki/rejected", applicationName)
                    },
                    AutoAcceptUntrustedCertificates = true,
                    AddAppCertToTrustedStore = true
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas
                {
                    OperationTimeout = 600000
                },
                ClientConfiguration = new ClientConfiguration
                {
                    DefaultSessionTimeout = 600000
                },
                TraceConfiguration = new TraceConfiguration
                {
                    OutputFilePath = Utils.Format(@"%LocalApplicationData%/Logs/{0}.log",applicationName),
                    DeleteOnLoad = true
                }
            };
            configuration.TraceConfiguration.ApplySettings();
            await configuration.Validate(ApplicationType.Client);
            return configuration;
        }
    }
}