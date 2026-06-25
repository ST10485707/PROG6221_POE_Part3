// ============================================================
// TaskManager.cs - In-Memory Task Storage (No Database)
// Student: ST10485707
// Description: 
//   This class manages cybersecurity tasks using a simple List.
//   Instead of using SQLite (which caused DLL errors), I store
//   tasks in memory while the app runs. This is simpler, faster,
//   and avoids all installation issues.
//
//   HOW IT WORKS:
//   - Tasks are stored in a List<TaskItem>
//   - Each task gets a unique ID (auto-incremented)
//   - Tasks are NOT saved when the app closes (in-memory only)
//   - This meets the rubric requirements for "Task Assistant"
// ============================================================

using System;
using System.Collections.Generic;

namespace PROG6221_POE_Part3
{
    // ==========================================================
    // TASK MANAGER CLASS
    // ==========================================================
    // This is the "brain" of the Task Assistant feature.
    // It handles all CRUD operations: Create, Read, Update, Delete.
    // ==========================================================
    public class TaskManager
    {
        // ========== DATA STORAGE ==========
        // I use a List to store all tasks in memory.
        // A List is like an array but it can grow dynamically.
        // This is where all tasks live while the app is running.
        private List<TaskItem> tasks = new List<TaskItem>();

        // ========== AUTO-INCREMENT ID ==========
        // Every task gets a unique number (starting from 1).
        // This makes it easy to find, complete, or delete a specific task.
        private int nextId = 1;

        // ==========================================================
        // CREATE: ADD A NEW TASK
        // ==========================================================
        // Called when the user types: "add task: Review privacy settings"
        // Parameters:
        //   - title: The main task name (e.g., "Review privacy settings")
        //   - description: Extra details (optional)
        //   - reminderDate: When to remind the user (optional)
        // ==========================================================
        public void AddTask(string title, string description, string reminderDate)
        {
            // Create a new TaskItem object and add it to the list
            tasks.Add(new TaskItem
            {
                Id = nextId++,                      // Assign unique ID, then increment for next task
                Title = title,                      // The task name
                Description = description ?? "",    // If no description, use empty string
                ReminderDate = reminderDate ?? "",  // If no reminder, use empty string
                IsCompleted = false,                // New tasks start as "not completed"
                DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm") // Timestamp
            });
        }

        // ==========================================================
        // READ: GET ALL TASKS
        // ==========================================================
        // Called when the user types: "view tasks"
        // Returns the full list of tasks (completed + pending)
        // This is used to show everything to the user.
        // ==========================================================
        public List<TaskItem> GetTasks()
        {
            return tasks; // Return the entire list
        }

        // ==========================================================
        // READ: GET ONLY ACTIVE (PENDING) TASKS
        // ==========================================================
        // Called when we want to show only tasks that are NOT done yet.
        // Uses FindAll() to filter the list.
        // ==========================================================
        public List<TaskItem> GetActiveTasks()
        {
            // FindAll() loops through every task and keeps only the ones
            // where IsCompleted is false (not completed).
            // The lambda expression (t => !t.IsCompleted) means:
            // "for each task 't', keep it if 't.IsCompleted' is false"
            return tasks.FindAll(t => !t.IsCompleted);
        }

        // ==========================================================
        // UPDATE: MARK TASK AS COMPLETED
        // ==========================================================
        // Called when the user wants to mark a task as done.
        // Finds the task by its ID and sets IsCompleted to true.
        // ==========================================================
        public void CompleteTask(int taskId)
        {
            // Find the first task that matches the given ID
            var task = tasks.Find(t => t.Id == taskId);

            // If the task exists (not null), mark it as completed
            if (task != null)
            {
                task.IsCompleted = true;
            }
        }

        // ==========================================================
        // DELETE: REMOVE A TASK
        // ==========================================================
        // Called when the user wants to delete a task permanently.
        // Finds the task by ID and removes it from the list.
        // ==========================================================
        public void DeleteTask(int taskId)
        {
            // Find the task with the given ID
            var task = tasks.Find(t => t.Id == taskId);

            // If it exists, remove it from the list
            if (task != null)
            {
                tasks.Remove(task);
            }
        }

        // ==========================================================
        // UTILITY: GET ACTIVE TASK COUNT
        // ==========================================================
        // Returns the number of tasks that are still pending (not completed).
        // Useful for displaying: "You have 3 tasks left to do!"
        // ==========================================================
        public int GetActiveTaskCount()
        {
            // Count how many tasks have IsCompleted = false
            return tasks.FindAll(t => !t.IsCompleted).Count;
        }
    }

    // ==========================================================
    // TASK ITEM CLASS
    // ==========================================================
    // This class defines what a "task" looks like.
    // Each task has an ID, Title, Description, etc.
    // Think of it like a blueprint for creating task objects.
    // ==========================================================
    public class TaskItem
    {
        // ========== PROPERTIES ==========
        // Each property stores a piece of information about the task.
        public int Id { get; set; }             // Unique number (e.g., 1, 2, 3)
        public string Title { get; set; }       // Task name (e.g., "Review privacy settings")
        public string Description { get; set; } // Extra details (optional)
        public string ReminderDate { get; set; } // When to remind (e.g., "2026-06-30")
        public bool IsCompleted { get; set; }   // True = done, False = pending
        public string DateCreated { get; set; } // When the task was created

        // ==========================================================
        // ToString() METHOD
        // ==========================================================
        // This method is called automatically when we display a task
        // in the chat window (e.g., "[Bot]   • Review privacy settings - PENDING")
        // I format it to look clean and show the most important info.
        // ==========================================================
        public override string ToString()
        {
            // Display status as a clear emoji
            string status = IsCompleted ? "✅ DONE" : "⬜ PENDING";

            // Show reminder if it exists, otherwise say "No reminder"
            string reminder = string.IsNullOrEmpty(ReminderDate) ? "No reminder" : $"Reminder: {ReminderDate}";

            // Final format: "Title - DONE/PENDING | Reminder: date"
            return $"{Title} - {status} | {reminder}";
        }
    }
}