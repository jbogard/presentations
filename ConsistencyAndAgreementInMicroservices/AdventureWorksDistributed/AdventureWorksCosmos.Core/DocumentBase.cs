using System;
using System.Collections.Generic;
using AdventureWorksDistributed.Core.Infrastructure;
using Newtonsoft.Json;

namespace AdventureWorksDistributed.Core
{
    public abstract class DocumentBase
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }
    }
}