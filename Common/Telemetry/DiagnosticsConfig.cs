using System.Diagnostics;

public static class DiagnosticsConfig
{
    public static ActivitySource GetSource(string sourceName) => new (sourceName);
}