﻿using System.Diagnostics;
using OpenTelemetry;
using Serilog.Enrichers.Span;

namespace Common.Telemetry.Processors
{
    public class MySpanTraceProcessor : BaseProcessor<Activity>
    {
        public override void OnEnd(Activity data)
        {
            base.OnEnd(data);


            var spanId = data.GetSpanId();
            var traceId = data.GetTraceId();
            var parentId = data.GetParentId();

            data.AddTag("Span_Id", spanId);
            data.AddTag("Trace_Id", traceId);
            data.AddTag("Parent_Id", parentId);
        }
    }
}