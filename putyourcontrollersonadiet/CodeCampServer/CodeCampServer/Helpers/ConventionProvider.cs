using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CodeCampServerLite.Helpers
{
    public class ConventionProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes, Type containerType,
            Func<object> modelAccessor, Type modelType, string propertyName)
        {
            ModelMetadata metadata = base.CreateMetadata(attributes,
                                                         containerType,
                                                         modelAccessor,
                                                         modelType,
                                                         propertyName);

            if (metadata.DisplayName == null)
                metadata.DisplayName =
                    metadata.PropertyName.ToSeparatedWords();

            return metadata;
        }
    }
}