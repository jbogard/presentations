using System.Diagnostics;
using OpenTelemetry;

public class CopyBaggageToTagsProcessor : BaseProcessor<Activity>
{
    public override void OnEnd(Activity data)
    {
        foreach (var (key, value) in data.Baggage)
        {
            data.AddTag(key, value);
        }
    }
}
