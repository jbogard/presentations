using Raven.Client.Document;

namespace Polyglot.Orders.Processor 
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://nservicebus.com/GenericHost.aspx
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
	    public void Init()
	    {
            DocumentStore = new DocumentStore
            {
                DefaultDatabase = "Polyglot.UI.Orders",
                Url = "http://localhost:8080"
            };
            DocumentStore.Initialize();
	    }

	    public static DocumentStore DocumentStore { get; set; }
    }
}