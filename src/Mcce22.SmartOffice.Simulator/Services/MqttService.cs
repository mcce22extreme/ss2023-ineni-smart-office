using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mcce22.SmartOffice.Simulator.Messages;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Oocx.ReadX509CertificateFromPem;

namespace Mcce22.SmartOffice.Simulator.Services
{
    public interface IMqttService
    {
        event EventHandler<MessageReceivedArgs> MessageReceived;

        Task Connect();

        Task PublishMessage(DataIngressMessage message);
    }

    public class MqttService : IMqttService
    {
        private static readonly MqttFactory Factory = new MqttFactory();

        private readonly IMqttClient _mqttClient;
        private readonly AppSettings _appSettings;

        public event EventHandler<MessageReceivedArgs> MessageReceived;

        public MqttService(AppSettings appSettings)
        {
            _appSettings = appSettings;

            _mqttClient = Factory.CreateMqttClient();            
        }

        public async Task Connect()
        {
            if (!_mqttClient.IsConnected)
            {
                var deviceCertPEMString = await  File.ReadAllTextAsync(@$"certificates\certificate.pem.crt");
                var devicePrivateCertPEMString = await File.ReadAllTextAsync(@$"certificates\private.pem.key");
                var certificateAuthorityCertPEMString = await File.ReadAllTextAsync(@$"certificates\AmazonRootCA1.pem");

                //Converting from PEM to X509 certs in C# is hard
                //Load the CA certificate
                //https://gist.github.com/ChrisTowles/f8a5358a29aebcc23316605dd869e839
                var certBytes = Encoding.UTF8.GetBytes(certificateAuthorityCertPEMString);
                var signingcert = new X509Certificate2(certBytes);

                //Load the device certificate
                //Use Oocx.ReadX509CertificateFromPem to load cert from pem
                var reader = new CertificateFromPemReader();
                var deviceCertificate = reader.LoadCertificateWithPrivateKeyFromStrings(deviceCertPEMString, devicePrivateCertPEMString);

                // Certificate based authentication
                var certs = new List<X509Certificate>
                {
                    signingcert,
                    deviceCertificate
                };

                //Set things up for our MQTTNet client
                var tlsOptions = new MqttClientOptionsBuilderTlsParameters
                {
                    Certificates = certs,
                    SslProtocol = SslProtocols.Tls12,
                    UseTls = true,
                    AllowUntrustedCertificates = true,
                    IgnoreCertificateChainErrors = true,
                    IgnoreCertificateRevocationErrors = true
                };

                var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_appSettings.EndpointAddress, _appSettings.EndpointPort)
                .WithTls(tlsOptions)
                .Build();

                _mqttClient.ApplicationMessageReceivedAsync += OnMqttMessageReceived;

                await _mqttClient.ConnectAsync(options, CancellationToken.None);

                await _mqttClient.SubscribeAsync(Topics.DEVICE_ACTIVATED);
            }
        }

        private Task OnMqttMessageReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            var payload = arg.ApplicationMessage.ConvertPayloadToString();

            var message = JsonConvert.DeserializeObject<DeviceActivatedMessage>(payload);

            MessageReceived?.Invoke(this, new MessageReceivedArgs(message));

            return Task.CompletedTask;
        }

        public async Task PublishMessage(DataIngressMessage message)
        {
            if (_mqttClient.IsConnected)
            {
                var msg = new MqttApplicationMessageBuilder()
                    .WithTopic(Topics.DATA_INGRESS)
                    .WithPayload(JsonConvert.SerializeObject(message))
                    .Build();

                await _mqttClient.PublishAsync(msg, CancellationToken.None);
            }
        }
    }

    public class MqttMessage
    {
        public string Topic { get; set; }
    }

    public class MessageReceivedArgs : EventArgs
    {
        public DeviceActivatedMessage Message { get; }

        public MessageReceivedArgs(DeviceActivatedMessage message)
        {
            Message = message;
        }
    }
}
