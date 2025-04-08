using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// Logs the number of words spoken during a session and saves it as a CSV file.
/// Designed to work in Unity, including Android builds.
/// </summary>
public class SessionLogger : MonoBehaviour
{
    private string filePath;

    void Awake()
    {
        // Set path to persistentDataPath, which works on Android as well
        filePath = Path.Combine(Application.persistentDataPath, "word_log.csv");

        // If file doesn't exist, write the header
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Timestamp,WordCount\n", Encoding.UTF8);
        }
    }

    /// <summary>
    /// Call this method to log a word count for the current session.
    /// </summary>
    /// <param name="wordCount">Number of words spoken</param>
    public void LogWordCount(int wordCount)
    {
        string logEntry = string.Format("{0},{1}\n", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), wordCount);
        File.AppendAllText(filePath, logEntry, Encoding.UTF8);
        Debug.Log("Logged word count: " + logEntry);
    }
}
