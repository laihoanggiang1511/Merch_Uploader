using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Log
    {
        public static log4net.ILog log;
        public static void SetUpLog(string logName, string logFilePath)
        {
            log = log4net.LogManager.GetLogger(logName);
            Logger logger = (Logger)log.Logger;
            logger.Additivity = false;

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = false;
            string AppFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            roller.File = Path.Combine(AppFolder, logFilePath);
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            logger.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            logger.AddAppender(memory);

            logger.Level = Level.All;
            logger.Hierarchy.Configured = true;
        }
    }
}
