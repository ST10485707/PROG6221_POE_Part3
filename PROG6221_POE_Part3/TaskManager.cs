// ============================================================
// TaskManager.cs - Manages tasks, quiz, and activity log
// Student: ST10485707
// Description:
//   This class handles all the "brain" work of the chatbot:
//   - Tasks (add, view, complete, delete) stored in memory
//   - Quiz (11 cybersecurity questions with scoring)
//   - Activity log (records what the user does)
// ============================================================

using System;
using System.Collections.Generic;

namespace PROG6221_POE_Part3
{
    public class TaskManager
    {
        // ========== TASK STORAGE ==========
        private List<TaskItem> tasks = new List<TaskItem>();
        private int nextId = 1;

        // ========== ACTIVITY LOG ==========
        private List<string> activityLog = new List<string>();
        private const int MaxLogEntries = 10;

        // ========== TASK METHODS ==========

        public void AddTask(string title, string description, string reminderDate)
        {
            tasks.Add(new TaskItem
            {
                Id = nextId++,
                Title = title,
                Description = description ?? "",
                ReminderDate = reminderDate ?? "",
                IsCompleted = false,
                DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            });

            AddToLog($"Task added: '{title}'");
        }

        public List<TaskItem> GetTasks()
        {
            return tasks;
        }

        public List<TaskItem> GetActiveTasks()
        {
            return tasks.FindAll(t => !t.IsCompleted);
        }

        public void CompleteTask(int taskId)
        {
            var task = tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                AddToLog($"Task completed: '{task.Title}'");
            }
        }

        public void DeleteTask(int taskId)
        {
            var task = tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                string title = task.Title;
                tasks.Remove(task);
                AddToLog($"Task deleted: '{title}'");
            }
        }

        public int GetActiveTaskCount()
        {
            return tasks.FindAll(t => !t.IsCompleted).Count;
        }

        // ========== QUIZ QUESTIONS ==========
        public List<QuizQuestion> GetQuizQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it" },
                    CorrectAnswerIndex = 2
                },
                new QuizQuestion
                {
                    Question = "What is a strong password?",
                    Options = new List<string> { "Your birthday", "Password123", "At least 12 characters with uppercase, lowercase, numbers, and symbols", "Your pet's name" },
                    CorrectAnswerIndex = 2
                },
                new QuizQuestion
                {
                    Question = "What does 'phishing' mean?",
                    Options = new List<string> { "A type of fish", "A scam to steal personal information", "A type of computer virus", "A safe browsing tool" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "What should you look for in a safe website URL?",
                    Options = new List<string> { "http://", "https://", "www.", ".com" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    Options = new List<string> { "A password manager", "An extra layer of security using a second verification step", "A type of antivirus software", "A way to delete your account" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "Why should you avoid public Wi-Fi for banking?",
                    Options = new List<string> { "It's too slow", "It's not secure and hackers can intercept your data", "It costs money", "The bank doesn't allow it" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "What is a common sign of a scam email?",
                    Options = new List<string> { "It addresses you by your name", "It has spelling mistakes and creates urgency", "It offers you a discount", "It asks you to click a link" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "How often should you change your passwords?",
                    Options = new List<string> { "Never", "Every 3-6 months", "Every day", "Only when you suspect a breach" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new List<string> { "A type of computer hardware", "Manipulating people into revealing confidential information", "A programming language", "A type of encryption" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "What should you do if you think you've been scammed?",
                    Options = new List<string> { "Keep it to yourself", "Report it to the bank and authorities immediately", "Wait to see what happens", "Send money to fix it" },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "What is a VPN used for?",
                    Options = new List<string> { "To speed up your internet", "To hide your IP address and encrypt your traffic", "To download files", "To hack other computers" },
                    CorrectAnswerIndex = 1
                }
            };
        }

        // ========== ACTIVITY LOG METHODS ==========

        public void AddToLog(string action)
        {
            string entry = $"{DateTime.Now:HH:mm} - {action}";
            activityLog.Add(entry);

            if (activityLog.Count > MaxLogEntries)
            {
                activityLog.RemoveAt(0);
            }
        }

        public List<string> GetActivityLog()
        {
            return activityLog;
        }

        public void ClearLog()
        {
            activityLog.Clear();
            AddToLog("Activity log cleared");
        }
    }

    // ==========================================================
    // TASK ITEM CLASS (Fixed nullable warnings)
    // ==========================================================
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";        // Default to empty string
        public string Description { get; set; } = "";  // Default to empty string
        public string ReminderDate { get; set; } = ""; // Default to empty string
        public bool IsCompleted { get; set; }
        public string DateCreated { get; set; } = "";  // Default to empty string

        public override string ToString()
        {
            string status = IsCompleted ? "✅ DONE" : "⬜ PENDING";
            string reminder = string.IsNullOrEmpty(ReminderDate) ? "No reminder" : $"Reminder: {ReminderDate}";
            return $"{Title} - {status} | {reminder}";
        }
    }

    // ==========================================================
    // QUIZ QUESTION CLASS (Fixed nullable warnings)
    // ==========================================================
    public class QuizQuestion
    {
        public string Question { get; set; } = "";              // Default to empty string
        public List<string> Options { get; set; } = new List<string>(); // Default to empty list
        public int CorrectAnswerIndex { get; set; }
    }
}