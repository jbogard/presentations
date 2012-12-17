using System;
using Neo4jClient;

namespace Polyglot.OrderAnalytics 
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://nservicebus.com/GenericHost.aspx
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public static GraphClient GraphClient { get; set; }
	    
        public void Init()
	    {
            GraphClient = new GraphClient(new Uri("http://localhost:7474/db/data"));
            GraphClient.Connect();
        }
    }
}