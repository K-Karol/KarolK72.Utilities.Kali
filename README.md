# KALI (Karol's Awesome Logging Interface)
## KarolK72.Utilities.Kali

**This project is currently WIP**

This project contains a logging aggregator framework I'm working on called KALI (**K**arol's **A**wesome **L**ogging **I**nterface).

The aim of this project is to create a logging service that can be ran either as part of a `ASP.NET` application(like Blazor) or standalone using Kestrel or IIS, that will utilise gRPC to capture logs from multiple application and then submit then to a datastore (SQL).
The logger will be easy to use as it will integrate with [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging/) as a `ILoggerProvider` with the `KarolK72.Utilities.Kali.Extensions` package.

The point of the aggregator would be to make log aggregation easier in environments with multiple processes, services and potentially multiple threads in an application which is currently not as easy or user friendly when using file logs or worse...the event viewer.
Therefore the point of creating the logging aggregator is so it can be integrated with a GUI frontend such as a WPF app or Blazor which will allow to view the logs in a friendly way that will make it a please to look at the logs, as well as make it possible for the customer (technical personnel) to view the logs as well.

The aggregator would identify the different sources, implement support for scopes and custom objects being passed from the applicaitons etc. The UI would make it easy to look at logs in realtime (when the service is implemented as part of the actual frontend or whatever) with filters/sorts, allow to write custom queries, and allow for writing extensions/customisations to add extra UI/functionality with the extra data that can come from the logs.

**TODO**: Finish README