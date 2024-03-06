using System;
using log4net.Appender;
using log4net.Core;

namespace UPrinceV4.Web.Service;

public class DbAppender : AppenderSkeleton
{
    protected override void Append(LoggingEvent loggingEvent)
    {
        // Implement your db logging logic here
        Console.WriteLine(RenderLoggingEvent(loggingEvent));
    }
}