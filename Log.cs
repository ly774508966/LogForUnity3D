using System;
using UnityEngine;
using System.IO;


public class Log {

    enum enLogType {
        Info,
        Warning,
        Error,
        Fatal,
    }

    private const string File_INFO = "Info";
    private const string File_Error = "Error";
    private const string File_Fatal = "Fatal";
    private const string File_Wraning = "Warning";

    public static bool disable = false;

    static public void Write(string _logFileName, object _class, params object[] logValues) {
        log(_logFileName, _class, enLogType.Info, logValues);
    }

    static public void Info(object _class, params object[] logValues) {
        log(File_INFO, _class, enLogType.Info, logValues);
    }

    static public void Error(object _class, params object[] logValues) {
        log(File_Error, _class, enLogType.Error, logValues);
    }


    private static void log(string _logFileName, object _class, enLogType logType, object[] values) {

        if(disable == true) {
            return;
        }

        string logStr = GetDebugString(_class, values);

        if (Application.isEditor) {
            if (logType == enLogType.Error) {
                Debug.LogError(logStr);
            } else if (logType == enLogType.Warning) {
                Debug.LogWarning(logStr);
            } else if (logType == enLogType.Fatal) {
                Debug.LogError(logStr);
            } else {
                Debug.Log(logStr);
            }
        }

        try {
            string fileName = UnityEngine.Application.persistentDataPath + "/Log/Unity/" + _logFileName + ".log";

            logStr += "\n";

            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write)) {
                byte[] data = new System.Text.UTF8Encoding().GetBytes(logStr);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }


        } catch (Exception e) {
            Debug.LogError(e);
        }

    }



    public static string GetDebugString(object _class, params object[] values) {
        string logStr = "[Log] " + DateTime.Now.ToString("T") + " ";
        if (_class == null) {
            logStr += "[file:\"\"]";
        } else {
            logStr += "[file:\"" + _class + "\"] - ";
        }

        for (int i = 0; i < values.Length; ++i) {
            try {
                logStr += values[i] == null ? "null" : values[i].ToString() + " ";
            } catch (Exception e) {
                logStr += "";
                Debug.LogError(e);
            }
        }

        return logStr;
    }
}

