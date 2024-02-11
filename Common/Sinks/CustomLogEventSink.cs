
using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;

public class CustomLogEventSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        var activity = Activity.Current;

        if (activity is not null)
        {
            if (logEvent.Exception is null)
            {
                var tags = new ActivityTagsCollection();
                // {
                //     {"TraceId",logEvent.TraceId},
                //     {"SpanId",logEvent.SpanId}
                // };
                foreach (var pr in logEvent.Properties)
                {
                    tags.Add(pr.Key, pr.Value);
                }
                var actEvent = new ActivityEvent(logEvent.RenderMessage(), tags: tags);
                activity.AddEvent(actEvent);
            }
            else
            {
                var tags = new ActivityTagsCollection
                {
                    {"StackTrace", logEvent.Exception.StackTrace },
                    // {"TraceId",logEvent.TraceId},
                    // {"SpanId",logEvent.SpanId}
                };
                activity.AddEvent(new ActivityEvent(logEvent.Exception.Message,tags:tags));
            }
        }
    }
}
