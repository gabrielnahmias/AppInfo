using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

/// <summary>
/// Assists in gathering assembly information about the application.
/// </summary>
public class AppInfo
{
    static object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
    public static Version Version {
        get {
            return Assembly.GetCallingAssembly().GetName().Version;
        }
    }

    /// <summary>
    /// Returns the title of the application.
    /// </summary>
    public static string Title
    {
        get
        {
            if (attributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title.Length > 0) return titleAttribute.Title;
            }
            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }
    }

    /// <summary>
    /// Returns the product name of the application.
    /// </summary>
    public static string ProductName
    {
        get
        {
            return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
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
    /// Returns the copyright holder of the application.
    /// </summary>
    public static string CopyrightHolder
    {
        get
        {
            return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
    }

    /// <summary>
    /// Returns the name of the company who made the application.
    /// </summary>
    public static string CompanyName
    {
        get
        {
            return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }
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

}