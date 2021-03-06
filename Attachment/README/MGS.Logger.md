[TOC]

﻿# MGS.Logger.dll

## Summary
- Logger for C# project develop.

## Platform

- Windows
- Android

## Environment
- .Net Framework 3.5 or above.

## Dependence
- System.dll

## Demand
- Output log to local file.
- Implement custom logger to output the log that print by LogUtility from other module.

## Implemented

```C#
public interface IFilter{}

public interface ILogger{}

public class FileLogger : ILogger{}

public sealed class LogUtility{}
```



## Usage

- Register logger to LogUtility.

```c#
var logDir = string.Format("{0}/Log/", Environment.CurrentDirectory);
LogUtility.Register(new FileLogger(logDir));
```

- Register logger with custom filter.

```C#
//Implemente IFilter base your logic.
public class Filter : IFilter
{
    public bool Select(string tag, string format, params object[] args)
    {
    //TODO: Decide whether to select this log.
    return tag.Contains(FileLogger.TAG_ERROR);
    }
}
  
//Register logger to LogUtility.
var logDir = string.Format("{0}/Log/", Environment.CurrentDirectory);
LogUtility.Register(new FileLogger(logDir, new Filter()));
```

- Use LogUtility to output log content.

```C#
LogUtility.Log("Log info is {0}", info);
LogUtility.LogError("Log error is {0}", error);
LogUtility.LogWarning("Log warning is {0}", warning);
```

- Wrap LogUtility specifically.

```C#
public sealed class Logger
{
    // A good way to use the LogUtility is wrap it specifically.
    // Example: add module prefix identification and more infos.

    public static void Log(string format, params object[] args)
    {
        LogUtility.Log(AddPrefix(format), args);
    }
    
    private static string AddPrefix(string origin)
    {
        return "[Demo] " + origin;
    }
}

Logger.Log("Log info is {0}", info);
```

------

[Previous](../../README.md)

------

Copyright © 2021 Mogoson.	mogoson@outlook.com