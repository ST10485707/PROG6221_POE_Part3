// ============================================================
// DatabaseHelper.cs - Handles SQLite database operations
// Student: ST10485707
// Description: Stores tasks and reminders for the Task Assistant
// ============================================================

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace PROG6221_POE_Part3
{
    public class DatabaseHelper
    {
        // Database file location
        private string connectionString = "Data Source=tasks.db;Version=3;";

        // Constructor - creates database and table if they don't exist
        public DatabaseHelper()
        {
            CreateDatabaseAndTable();
        }

        // Creates the database file and Tasks table
        private void CreateDatabaseAndTable()
        {
            // Create the file if it doesn't exist
            if (!File.Exists("tasks.db"))
            {
                SQLiteConnection.CreateFile("tasks.db");
            }

            // Create the Tasks table
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Description TEXT,
                        ReminderDate TEXT,
                        IsCompleted INTEGER DEFAULT 0,
                        DateCreated TEXT DEFAULT CURRENT_TIMESTAMP
                    )";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // ========== ADD A TASK ==========
        public void AddTask(string title, string description, string reminderDate)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted)
                    VALUES (@Title, @Description, @ReminderDate, 0)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", description ?? "");
                    command.Parameters.AddWithValue("@ReminderDate", reminderDate ?? "");
                    command.ExecuteNonQuery();
                }
            }
        }

        // ========== GET ALL TASKS ==========
        public List<TaskItem> GetTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Tasks ORDER BY DateCreated DESC";

                using (var command = new SQLiteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            ReminderDate = reader["ReminderDate"].ToString(),
                            IsCompleted = Convert.ToInt32(reader["IsCompleted"]) == 1,
                            DateCreated = reader["DateCreated"].ToString()
                        });
                    }
                }
            }

            return tasks;
        }

        // ========== GET ACTIVE TASKS (NOT COMPLETED) ==========
        public List<TaskItem> GetActiveTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Tasks WHERE IsCompleted = 0 ORDER BY DateCreated DESC";

                using (var command = new SQLiteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            ReminderDate = reader["ReminderDate"].ToString(),
                            IsCompleted = Convert.ToInt32(reader["IsCompleted"]) == 1,
                            DateCreated = reader["DateCreated"].ToString()
                        });
                    }
                }
            }

            return tasks;
        }

        // ========== MARK TASK AS COMPLETED ==========
        public void CompleteTask(int taskId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id";

                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // ========== DELETE TASK ==========
        public void DeleteTask(int taskId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Tasks WHERE Id = @Id";

                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // ========== GET COUNT OF ACTIVE TASKS ==========
        public int GetActiveTaskCount()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string countQuery = "SELECT COUNT(*) FROM Tasks WHERE IsCompleted = 0";

                using (var command = new SQLiteCommand(countQuery, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
    }

    // ========== TASK ITEM CLASS ==========
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public string DateCreated { get; set; }

        // Format for displaying tasks
        public override string ToString()
        {
            string status = IsCompleted ? "✅ DONE" : "⬜ PENDING";
            string reminder = string.IsNullOrEmpty(ReminderDate) ? "No reminder" : $"Reminder: {ReminderDate}";
            return $"{Title} - {status} | {reminder}";
        }
    }
}