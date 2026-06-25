using System;
using System.Collections.Generic;
using System.Linq;
using CyberSecurityChatbotWPF.Database;
using CyberSecurityChatbotWPF.Models;

namespace CyberSecurityChatbotWPF.Utilities
{
    public static class ActivityLogger
    {
        public static void Log(string action)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var log = new LogEntry
                    {
                        Description = action,
                        CreatedAt = DateTime.Now.ToString("HH:mm")
                    };
                    db.Logs.Add(log);
                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Log entry added: {action}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Log error: {ex.Message}");
            }
        }

        public static List<string> GetRecentLog(int count = 10)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var logs = db.Logs.OrderByDescending(l => l.Id).Take(count).ToList();
                    var result = new List<string>();
                    int number = 1;
                    foreach (var log in logs)
                    {
                        result.Add($"{number}. {log.Description} [{log.CreatedAt}]");
                        number++;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetRecentLog error: {ex.Message}");
                return new List<string> { "No logs available" };
            }
        }

        public static List<string> GetFullLog()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var logs = db.Logs.OrderByDescending(l => l.Id).ToList();
                    var result = new List<string>();
                    int number = 1;
                    foreach (var log in logs)
                    {
                        result.Add($"{number}. {log.Description} [{log.CreatedAt}]");
                        number++;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetFullLog error: {ex.Message}");
                return new List<string> { "No logs available" };
            }
        }

        public static int GetCount()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    return db.Logs.Count();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetCount error: {ex.Message}");
                return 0;
            }
        }

        public static void ClearLogs()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Logs.RemoveRange(db.Logs);
                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("All logs cleared");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ClearLogs error: {ex.Message}");
            }
        }
    }
}