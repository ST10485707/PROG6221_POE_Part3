// ============================================================
// MainWindow.xaml.cs - The user interface logic
// Student: ST10485707
// Description:
//   This file handles everything the user sees and interacts with.
//   It manages the chat display, user input, and calls the TaskManager
//   when tasks need to be added, viewed, or managed.
// ============================================================

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PROG6221_POE_Part3
{
    // ==========================================================
    // MAIN WINDOW CLASS
    // ==========================================================
    // This is the "brain" of the user interface.
    // It connects the visual design (MainWindow.xaml) with the
    // task management logic (TaskManager.cs).
    // ==========================================================
    public partial class MainWindow : Window
    {
        // ========== CLASS VARIABLES ==========

        // This creates an instance of TaskManager so we can use it
        // to add, view, complete, and delete tasks.
        private TaskManager taskManager = new TaskManager();

        // This stores the user's name so we can personalise responses
        // throughout the conversation.
        private string userName = "";

        // ==========================================================
        // CONSTRUCTOR
        // ==========================================================
        // Runs when the window first opens.
        // InitializeComponent() loads the XAML design.
        // AddWelcomeMessage() displays the bot's first messages.
        // ==========================================================
        public MainWindow()
        {
            InitializeComponent();
            AddWelcomeMessage();
        }

        // ==========================================================
        // WELCOME MESSAGE
        // ==========================================================
        // Shows the bot's opening messages when the app starts.
        // It tells the user what the chatbot can do and asks for their name.
        // ==========================================================
        private void AddWelcomeMessage()
        {
            AddToChat("Bot", "Hello! Welcome to Cybersecurity Assistant - Part 3!", "Green");
            AddToChat("Bot", "I can help you with:", "Green");
            AddToChat("Bot", "📋 Manage cybersecurity tasks with reminders", "Green");
            AddToChat("Bot", "🎯 Take a cybersecurity quiz", "Green");
            AddToChat("Bot", "📜 View activity log", "Green");
            AddToChat("Bot", "What is your name?", "Green");
        }

        // ==========================================================
        // ADD MESSAGE TO CHAT
        // ==========================================================
        // This is a helper method that adds a message to the chat display.
        // Parameters:
        //   - sender: Who is speaking ("Bot", "You", or "System")
        //   - message: The text to display
        //   - color: The text color (Green for bot, Magenta for user, etc.)
        // ==========================================================
        private void AddToChat(string sender, string message, string color)
        {
            // Format the message with brackets around the sender name
            // Example: "[Bot] Hello!"
            string formattedMessage = $"[{sender}] {message}";

            // Add the message to the chat display list box
            ChatDisplay.Items.Add(new ListBoxItem
            {
                Content = formattedMessage,
                Foreground = GetBrush(color)
            });

            // Auto-scroll to the bottom to show the newest message
            ChatDisplay.ScrollIntoView(ChatDisplay.Items[ChatDisplay.Items.Count - 1]);
        }

        // ==========================================================
        // COLOR HELPER
        // ==========================================================
        // This converts a color name (e.g., "Green") to a WPF Brush object.
        // This allows dynamic coloring of chat messages.
        // ==========================================================
        private System.Windows.Media.Brush GetBrush(string color)
        {
            switch (color.ToLower())
            {
                case "green": return System.Windows.Media.Brushes.LightGreen;
                case "magenta": return System.Windows.Media.Brushes.Magenta;
                case "cyan": return System.Windows.Media.Brushes.Cyan;
                case "yellow": return System.Windows.Media.Brushes.Yellow;
                default: return System.Windows.Media.Brushes.White;
            }
        }

        // ==========================================================
        // SEND BUTTON CLICK
        // ==========================================================
        // This runs when the user clicks the "Send" button.
        // It calls ProcessUserInput() to handle the user's message.
        // ==========================================================
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        // ==========================================================
        // ENTER KEY HANDLER
        // ==========================================================
        // This allows the user to press Enter instead of clicking Send.
        // This creates a more natural chat experience.
        // ==========================================================
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }

        // ==========================================================
        // MAIN PROCESSING METHOD
        // ==========================================================
        // This is the core method that handles all user input.
        // It validates the input, captures the user's name,
        // and processes commands like "add task" and "view tasks".
        // ==========================================================
        private void ProcessUserInput()
        {
            // Get the user's message and remove any leading/trailing spaces
            string userMessage = UserInput.Text.Trim();

            // ========== VALIDATION ==========
            // If the user typed nothing, ask them to type something.
            // This prevents the app from crashing or responding to empty messages.
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                AddToChat("System", "Please type something!", "Yellow");
                UserInput.Clear();
                UserInput.Focus();
                return;
            }

            // Display the user's message in the chat (in Magenta)
            AddToChat("You", userMessage, "Magenta");

            // ========== GET USER'S NAME ==========
            // If we don't have the user's name yet, capture it now.
            // This runs only the FIRST time the user types something.
            if (string.IsNullOrEmpty(userName))
            {
                userName = userMessage;
                AddToChat("Bot", $"Nice to meet you, {userName}! 😊", "Green");
                AddToChat("Bot", "Try typing: 'add task', 'view tasks', 'start quiz', or 'activity log'", "Green");
                AddToChat("Bot", "Type 'exit' to quit.", "Green");
                UserInput.Clear();
                UserInput.Focus();
                return; // Exit early so we don't process this as a command
            }

            // ========== EXIT COMMAND ==========
            // Allow the user to end the conversation gracefully.
            if (userMessage.ToLower() == "exit")
            {
                AddToChat("Bot", $"Goodbye, {userName}! Stay safe online! 😊", "Green");
                // Disable input controls so the user can't type after exiting
                UserInput.IsEnabled = false;
                SendButton.IsEnabled = false;
                return;
            }

            // ========== PROCESS COMMANDS ==========
            // Convert to lowercase for easier keyword matching.
            // This means "Add Task" and "add task" work the same.
            string lowerMsg = userMessage.ToLower();

            // --- ADD TASK ---
            // Format: "add task: Review privacy settings"
            // Detects if the message starts with "add task" or contains "add a task".
            if (lowerMsg.StartsWith("add task") || lowerMsg.Contains("add a task"))
            {
                // Extract the task name (everything after "task")
                string taskName = userMessage.Substring(userMessage.IndexOf("task") + 4).Trim();

                // Make sure the task name is not empty
                if (!string.IsNullOrEmpty(taskName))
                {
                    // Call TaskManager to add the task to the list
                    taskManager.AddTask(taskName, "No description", "");
                    AddToChat("Bot", $"✅ Task '{taskName}' added successfully!", "Green");
                }
                else
                {
                    // If the user typed "add task" but didn't give a name
                    AddToChat("Bot", "Please specify the task name. Example: 'add task: Review privacy settings'", "Green");
                }
            }

            // --- VIEW TASKS ---
            // Detects if the user wants to see their tasks.
            else if (lowerMsg.Contains("view tasks") || lowerMsg.Contains("show tasks") || lowerMsg.Contains("tasks"))
            {
                // Get the list of tasks from TaskManager
                var tasks = taskManager.GetTasks();

                // If there are no tasks, let the user know
                if (tasks.Count == 0)
                {
                    AddToChat("Bot", "📋 You have no tasks yet. Add one by typing: 'add task: Your task name'", "Green");
                }
                else
                {
                    // Display all tasks one by one
                    AddToChat("Bot", $"📋 You have {tasks.Count} tasks:", "Green");
                    foreach (var task in tasks)
                    {
                        // Each task uses its ToString() method for formatting
                        AddToChat("Bot", $"  • {task.ToString()}", "Green");
                    }
                }
            }

            // --- START QUIZ ---
            // This is a placeholder for the quiz feature (coming next)
            else if (lowerMsg.Contains("start quiz") || lowerMsg.Contains("quiz"))
            {
                AddToChat("Bot", "🎯 Starting Cybersecurity Quiz!", "Green");
                AddToChat("Bot", "Quiz feature coming soon! (Will be added in the next step)", "Green");
            }

            // --- ACTIVITY LOG ---
            // This is a placeholder for the activity log feature (coming next)
            else if (lowerMsg.Contains("activity log") || lowerMsg.Contains("log"))
            {
                AddToChat("Bot", "📜 Activity Log feature coming soon! (Will be added in the next step)", "Green");
            }

            // --- DEFAULT RESPONSE ---
            // If the user types something that doesn't match any command
            // Remind them what they can do.
            else
            {
                AddToChat("Bot", $"I'm here to help, {userName}! Try: 'add task', 'view tasks', 'start quiz', or 'activity log'", "Green");
            }

            // Clear the input box and refocus for the next message
            UserInput.Clear();
            UserInput.Focus();
        }

        // ==========================================================
        // BUTTON CLICK HANDLERS
        // ==========================================================
        // These buttons automatically type the command for the user.
        // This makes it easier for users who don't want to type.
        // ==========================================================

        // View Tasks button - fills "view tasks" into the text box
        private void ViewTasksButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput.Text = "view tasks";
            ProcessUserInput();
        }

        // Start Quiz button - fills "start quiz" into the text box
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput.Text = "start quiz";
            ProcessUserInput();
        }

        // Activity Log button - fills "activity log" into the text box
        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput.Text = "activity log";
            ProcessUserInput();
        }
    }
}