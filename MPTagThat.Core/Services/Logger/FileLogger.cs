#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.
#endregion
#region

using System;
using System.IO;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NLog.Filters;
using NLog.Targets;

#endregion

namespace MPTagThat.Core
{
  /// <summary>
  ///   An <see cref = "ILogger" /> implementation that writes messages to a text file.
  /// </summary>
  /// <remarks>
  ///   If the text file exists it will be truncated.
  /// </remarks>
  public class FileLogger : ILogger
  {
    private readonly string fileName; //holds the file to write to.
    private LogLevel level; //holds the treshold for the log level.
    private Logger _logger = null;
    private const int MAXARCHIVES = 10;

    /// <summary>
    ///   Creates a new <see cref = "FileLogger" /> instance and initializes it with the given filename and <see cref = "LogLevel" />.
    /// </summary>
    /// <param name = "fileName">The full path of the file to write the messages to.</param>
    /// <param name = "level">The minimum level messages must have to be written to the file.</param>
    public FileLogger(string fileName, LogLevel level, int portable)
    {
      string logPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MPTagThat\Log";
      if (portable == 1)
        logPath = $@"{Application.StartupPath}\Log";

      if (!Directory.Exists(logPath))
        Directory.CreateDirectory(logPath);

      this.fileName = $@"{logPath}\{fileName}";

      string ext = Path.GetExtension(this.fileName);
      string fileNamePattern = Path.ChangeExtension(this.fileName, ".{#}" + ext);
      ArchiveLogs(this.fileName, fileNamePattern, 0);
      this.level = level;

      // Now configure the NLOG File Target looger
      LoggingConfiguration config = new LoggingConfiguration();
      FileTarget fileTarget = new FileTarget();
      fileTarget.FileName = this.fileName;

      fileTarget.Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.ffffff} " +
                          "[${level:fixedLength=true:padding=5}]" +
                          "[${threadid:padding=3}]" +
													"[${stacktrace:format=Flat:topFrames=1:separator=\":\":fixedLength=true:padding=-30}]: " +
                          "${message} " +
                          "${exception:format=tostring}";

      config.AddTarget("file", fileTarget);

      level = LogLevel.Debug;

			LoggingRule rule = new LoggingRule("*", level, fileTarget);

			// Create a filter to disable Raven Database Debugging
			Filter filter = new ConditionBasedFilter {Action = FilterResult.Ignore, Condition = "starts-with('${logger}','Raven')" };
	    rule.Filters.Add(filter);
			config.LoggingRules.Add(rule);

			LogManager.Configuration = config;

      _logger = LogManager.GetLogger("MPTagThat");
    }

    private void ArchiveLogs(string fileName, string pattern, int archiveNumber)
    {
      if (archiveNumber >= MAXARCHIVES)
      {
        File.Delete(fileName);
        return;
      }

      if (!File.Exists(fileName))
      {
        return;
      }

      string newFileName = ReplaceNumber(pattern, archiveNumber);
      if (File.Exists(fileName))
      {
        ArchiveLogs(newFileName, pattern, archiveNumber + 1);
      }

      try
      {
        File.Move(fileName, newFileName);
      }
      catch (IOException) {}
      catch (UnauthorizedAccessException) {}
    }

    private static string ReplaceNumber(string pattern, int value)
    {
      int firstPart = pattern.IndexOf("{#", StringComparison.Ordinal);
      int lastPart = pattern.IndexOf("#}", StringComparison.Ordinal) + 2;
      int numDigits = lastPart - firstPart - 2;

      return pattern.Substring(0, firstPart) + Convert.ToString(value, 10).PadLeft(numDigits, '0') + pattern.Substring(lastPart);
    }

    #region ILogger Members

    /// <summary>
    /// Returns the defined Logger
    /// </summary>
    public Logger GetLogger
    {
      get { return _logger; }
    }

    /// <summary>
    ///   Gets or sets the log level.
    /// </summary>
    /// <value>A <see cref = "LogLevel" /> value that indicates the minimum level messages must have to be 
    ///   written to the file.</value>
    public NLog.LogLevel Level
    {
      get { return level; }
      set
      {
        level = value;
        LoggingConfiguration config = LogManager.Configuration;
        for (int i = 0; i < 6; ++i)
        {
          if (LogLevel.FromOrdinal(i) < level)
          {
            config.LoggingRules[0].DisableLoggingForLevel(LogLevel.FromOrdinal(i));
          }
          else
          {
            config.LoggingRules[0].EnableLoggingForLevel(LogLevel.FromOrdinal(i));
          }
        }

	      if (Options.RavenDebug == 1 && config.LoggingRules[0].Filters.Count > 0)
	      {
		      config.LoggingRules[0].Filters.RemoveAt(0);
	      }

				LogManager.Configuration = config;
      }
    }

    #endregion
 }
}