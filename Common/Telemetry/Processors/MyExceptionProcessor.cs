﻿using System.Diagnostics;
using System.Linq;
using OpenTelemetry;

namespace Common.Telemetry.Processors
{
    public class MyExceptionProcessor : BaseProcessor<Activity>
    {
        public override void OnEnd(Activity data)
        {
            base.OnEnd(data);

            var activityEventList = data.Events.Where(e => e.Name == "exception");

            var index = 1;

            foreach (var activityEvent in activityEventList)
            {
                var trace = activityEvent.Tags.FirstOrDefault(e => e.Key == "exception.stacktrace").Value as string;
                data.SetTag($"{index}.exception.stacktrace", trace);
                index++;
            }
        }
    }
}