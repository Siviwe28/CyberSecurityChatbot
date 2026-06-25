using System;
using System.Collections.Generic;
using System.Linq;
using CyberSecurityChatbotWPF.Database;
using CyberSecurityChatbotWPF.Models;

namespace CyberSecurityChatbotWPF.Services
{
    public class TaskStorageHelper
    {
        private readonly ApplicationDbContext db;

        public TaskStorageHelper()
        {
            db = new ApplicationDbContext();
            EnsureDatabaseCreated();
        }

        private void EnsureDatabaseCreated()
        {
            try
            {
                db.Database.EnsureCreated();
                System.Diagnostics.Debug.WriteLine("Database ensured created successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database creation error: {ex.Message}");
            }
        }

        public List<TaskItem> LoadTasks()
        {
            try
            {
                var tasks = db.Tasks.ToList();
                System.Diagnostics.Debug.WriteLine($"Loaded {tasks.Count} tasks from database");
                return tasks;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading tasks: {ex.Message}");
                return new List<TaskItem>();
            }
        }

        public void AddTask(TaskItem task)
        {
            try
            {
                db.Tasks.Add(task);
                int result = db.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"Task added to database: {task.Title}, Rows affected: {result}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding task: {ex.Message}");
                throw;
            }
        }

        public void AddTask(string title, string description, string reminder)
        {
            try
            {
                var task = new TaskItem
                {
                    Title = title,
                    Description = description ?? title,
                    Reminder = reminder ?? "",
                    IsComplete = false,
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                };
                db.Tasks.Add(task);
                int result = db.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"Task added to database: {title}, Rows affected: {result}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding task: {ex.Message}");
                throw;
            }
        }

        public void MarkAsComplete(int id)
        {
            try
            {
                var task = db.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    task.IsComplete = true;
                    db.Tasks.Update(task);
                    int result = db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Task marked complete: {id}, Rows affected: {result}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Task not found with ID: {id}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error marking task complete: {ex.Message}");
                throw;
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                var task = db.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    db.Tasks.Remove(task);
                    int result = db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Task deleted: {id}, Rows affected: {result}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Task not found with ID: {id}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting task: {ex.Message}");
                throw;
            }
        }
    }
}