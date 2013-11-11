/**
 * AppInfo
 *  @desc    Assists in gathering assembly information about the application.
 *  @author  Gabriel Nahmias (gabriel@terrasoftlabs.com)
 *  @ver     1.0
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

/// <summary>
/// Assists in gathering assembly information about the application.
/// </summary>
public class AppInfo
{
    static object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(false);

    /// <summary>
    /// Returns the name of the company who made the application.
    /// </summary>
    public static string CompanyName
    {
        get
        {
            return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[5]).Company;
        }
    }

    /// <summary>
    /// Returns a DateTime object containing the moment when the application was last compiled.
    /// </summary>
    public static DateTime CompileTime
    {
        get
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }
    }

    /// <summary>
    /// Returns the copyright for the application.
    /// </summary>
    public static string Copyright
    {
        get
        {
            return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
    }

    /// <summary>
    /// Returns the description of the application.
    /// </summary>
    public static string Description
    {
        get
        {
            return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }
    }

    /// <summary>
    /// Returns the product name of the application.
    /// </summary>
    public static string ProductName
    {
        get
        {
            return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[6]).Product;
        }
    }

    /// <summary>
    /// Returns the target framework for the application.
    /// </summary>
    public static string TargetFramework
    {
        get
        {
            return attributes.Length == 0 ? "" : ((TargetFrameworkAttribute)attributes[3]).FrameworkName;
        }
    }

    /// <summary>
    /// Returns the title of the application.
    /// </summary>
    public static string Title
    {
        get
        {
            // Got any attributes? If so, ...
            if (attributes.Length > 0)
            {
                // ... get the title one and if it has an appropriate length, return it.
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[4];
                if (titleAttribute.Title.Length > 0) return titleAttribute.Title;
            }
            // Otherwise, get the application's filename and return it without its extension.
            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }
    }

    /// <summary>
    /// Returns the trademark for the application.
    /// </summary>
    public static string Trademark
    {
        get
        {
            return attributes.Length == 0 ? "" : ((AssemblyTrademarkAttribute)attributes[8]).Trademark;
        }
    }

    /// <summary>
    /// Returns the Version object for the application.
    /// </summary>
    public static Version Version
    {
        get
        {
            return Assembly.GetCallingAssembly().GetName().Version;
        }
    }

    /* Methods */

    /// <summary>
    /// Given a format string (which recognizes characters 'M' [major], 'm' [minor], 'b' [build], and 'r' [revision]), this returns
    /// a string formatted by this application's Version object's properties.
    /// </summary>
    /// <param name="fmt">The format string (any characters besides 'M' [major], 'm' [minor], 'b' [build], and 'r' [revision] will
    /// appear as is.</param>
    /// <returns>A string containing the specified parts of the application's version.</returns>
    public static string GetVersion(string fmt = "M.m")
    {
        Version ver = Version;
        return fmt.Replace("M", ver.Major.ToString())
                  .Replace("m", ver.Minor.ToString())
                  .Replace("b", ver.Build.ToString())
                  .Replace("r", ver.Revision.ToString());
    }
}