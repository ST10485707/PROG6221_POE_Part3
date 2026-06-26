// ============================================================
// DatabaseHelper.cs - MySQL Database Operations
// Student: ST10485707
// Description: 
//   This class handles all database operations using MySQL.
//   It creates the database and tasks table, and provides
//   methods to add, view, complete, and delete tasks.
// ============================================================

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace PROG6221_POE_Part3
{
    public class DatabaseHelper
    {
        // ========== CONNECTION STRING ==========
        // This tells C# how to connect to MySQL.
        // My password is: Mpnmil005!
        private string connectionString = "Server=localhost;Database=cybersecurity_chatbot;Uid=root;Pwd=Mpnmil005!;";

        // ========== CONSTRUCTOR ==========
        // Creates the database and table if they don't exist.
        public DatabaseHelper()
        {
            CreateDatabaseAndTable();
        }

        // ==========================================================
        // CREATE DATABASE AND TABLE
        // ==========================================================
        private void CreateDatabaseAndTable()
        {
            try
            {
                // First, connect without specifying a database
                // This uses the SAME password as the main connection
                string connectionStringWithoutDb = "Server=localhost;Uid=root;Pwd=Mpnmil005!;";

                using (var connection = new MySqlConnection(connectionStringWithoutDb))
                {
                    connection.Open();

                    // Create the database if it doesn't exist
                    string createDbQuery = "CREATE DATABASE IF NOT EXISTS cybersecurity_chatbot";
                    using (var command = new MySqlCommand(createDbQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                // Now connect to the database and create the table
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Tasks (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Title VARCHAR(255) NOT NULL,
                            Description TEXT,
                            ReminderDate VARCHAR(50),
                            IsCompleted BOOLEAN DEFAULT FALSE,
                            DateCreated TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                        )";

                    using (var command = new MySqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // This will show in the console if there's a connection error
                Console.WriteLine($"Database Error: {ex.Message}");
            }
        }

        // ==========================================================
        // ADD TASK
        // ==========================================================
        public void AddTask(string title, string description, string reminderDate)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = @"
                        INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted)
                        VALUES (@Title, @Description, @ReminderDate, FALSE)";

                    using (var command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Description", description ?? "");
                        command.Parameters.AddWithValue("@ReminderDate", reminderDate ?? "");
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding task: {ex.Message}");
            }
        }

        // ==========================================================
        // GET ALL TASKS
        // ==========================================================
        public List<TaskItem> GetTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Tasks ORDER BY DateCreated DESC";

                    using (var command = new MySqlCommand(selectQuery, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TaskItem
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                                ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate")) ? "" : reader.GetString("ReminderDate"),
                                IsCompleted = reader.GetBoolean("IsCompleted"),
                                DateCreated = reader.GetDateTime("DateCreated").ToString("yyyy-MM-dd HH:mm")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting tasks: {ex.Message}");
            }

            return tasks;
        }

        // ==========================================================
        // GET ACTIVE TASKS (NOT COMPLETED)
        // ==========================================================
        public List<TaskItem> GetActiveTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Tasks WHERE IsCompleted = FALSE ORDER BY DateCreated DESC";

                    using (var command = new MySqlCommand(selectQuery, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TaskItem
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                                ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate")) ? "" : reader.GetString("ReminderDate"),
                                IsCompleted = reader.GetBoolean("IsCompleted"),
                                DateCreated = reader.GetDateTime("DateCreated").ToString("yyyy-MM-dd HH:mm")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting active tasks: {ex.Message}");
            }

            return tasks;
        }

        // ==========================================================
        // MARK TASK AS COMPLETED
        // ==========================================================
        public void CompleteTask(int taskId)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Tasks SET IsCompleted = TRUE WHERE Id = @Id";

                    using (var command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", taskId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error completing task: {ex.Message}");
            }
        }

        // ==========================================================
        // DELETE TASK
        // ==========================================================
        public void DeleteTask(int taskId)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Tasks WHERE Id = @Id";

                    using (var command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", taskId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting task: {ex.Message}");
            }
        }

        // ==========================================================
        // GET ACTIVE TASK COUNT
        // ==========================================================
        public int GetActiveTaskCount()

        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string countQuery = "SELECT COUNT(*) FROM Tasks WHERE IsCompleted = FALSE";

                    using (var command = new MySqlCommand(countQuery, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting task count: {ex.Message}");
                return 0;
            }
        }
    }
}