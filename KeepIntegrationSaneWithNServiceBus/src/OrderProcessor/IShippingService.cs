using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace OrderProcessor
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IShippingService" in both code and config file together.
    [ServiceContract]
    public interface IShippingService
    {
        [OperationContract]
        bool ProcessOrderForShipping(int orderId);
    }
}
