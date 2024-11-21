using JetBrains.Annotations;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Text.Json;

public static class DiagnosticsConfig
{
    public static ActivitySource GetSource(string sourceName) => new (sourceName);

    [CanBeNull]
    public static Activity? StartActivity(string sourceName, string name, ActivityKind kind = ActivityKind.Internal)
    {
        var Source = GetSource(sourceName);
        return Source?.StartActivity(name, kind);
    }

    [CanBeNull]
    public static Activity FailActivity(this Exception ex)
    {
        var activity = Activity.Current;

        if (activity == null) return activity;

        activity.RecordException(ex);
        activity.SetStatus(Status.Error);

        return activity;
    }

    [CanBeNull]
    public static Activity WriteToActivity(this Exception ex)
    {
        var activity = Activity.Current;

        if (activity == null) return activity;

        activity.RecordException(ex);


        return activity;
    }

    [CanBeNull]
    public static Activity AddToActivityAsJsonTag(this object obj, string tag)
    {
        var activity = Activity.Current;
        activity?.AddTag(tag, JsonSerializer.Serialize(obj));
        return activity;
    }

    [CanBeNull]
    public static Activity AddToActivityAsTag(this object obj, string tag)
    {
        var activity = Activity.Current;
        activity?.AddTag(tag, obj);
        return activity;
    }
}