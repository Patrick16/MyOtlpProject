using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Logs;

public class LogProcessor : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord data)
    {
        base.OnEnd(data);
        var activity = Activity.Current;
        
        if (activity is not null)
        {
            if (data.Exception is null)
            {
                var tags = new ActivityTagsCollection
                {
                    {"TraceId",data.TraceId},
                    {"SpanId",data.SpanId}
                };
                foreach (var pr in data.Attributes)
                {
                    tags.Add(pr.Key, pr.Value);
                }
                var actEvent = new ActivityEvent(data.Body.ToString(), tags: tags);
                activity.AddEvent(actEvent);
            }
            else
            {
                var tags = new ActivityTagsCollection
                {
                    {"StackTrace", data.Exception.StackTrace },
                    {"TraceId",data.TraceId},
                    {"SpanId",data.SpanId}
                };
                activity.AddEvent(new ActivityEvent(data.Exception.Message,tags:tags));
            }
        }
    }
}