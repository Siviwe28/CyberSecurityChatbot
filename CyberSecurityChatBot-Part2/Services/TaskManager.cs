using System;
using System.Collections.Generic;
using CyberSecurityChatbotWPF.Models;
using CyberSecurityChatbotWPF.Utilities;

namespace CyberSecurityChatbotWPF.Services
{
    public class TaskManager
    {
        private TaskStorageHelper storage;

        public TaskManager()
        {
            storage = new TaskStorageHelper();
        }

        public string AddTask(string title, string description, string reminder = "")
        {
            try
            {
                storage.AddTask(title, description, reminder);
                string logMessage = $"Task added: '{title}'";
                if (!string.IsNullOrEmpty(reminder))
                {
                    logMessage += $" (Reminder: {reminder})";
                }
                ActivityLogger.Log(logMessage);
                return $"Task added: {title}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddTask Error: {ex.Message}");
                return $"Error adding task: {ex.Message}";
            }
        }

        public List<TaskItem> GetAllTasks()
        {
            try
            {
                return storage.LoadTasks();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllTasks Error: {ex.Message}");
                return new List<TaskItem>();
            }
        }

        public string MarkAsComplete(int id)
        {
            try
            {
                storage.MarkAsComplete(id);
                ActivityLogger.Log($"Task marked complete: {id}");
                return "Task marked as complete";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MarkAsComplete Error: {ex.Message}");
                return $"Error marking task complete: {ex.Message}";
            }
        }

        public string DeleteTask(int id)
        {
            try
            {
                storage.DeleteTask(id);
                ActivityLogger.Log($"Task deleted: {id}");
                return "Task deleted";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteTask Error: {ex.Message}");
                return $"Error deleting task: {ex.Message}";
            }
        }

        public TaskItem GetTaskById(int id)
        {
            try
            {
                var tasks = storage.LoadTasks();
                return tasks.Find(t => t.Id == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTaskById Error: {ex.Message}");
                return null;
            }
        }

        public int GetTaskCount()
        {
            try
            {
                return storage.LoadTasks().Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}