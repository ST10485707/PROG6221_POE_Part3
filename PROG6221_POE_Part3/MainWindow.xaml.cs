// ============================================================
// MainWindow.xaml.cs - The user interface logic
// Student: ST10485707
// Description:
//   This file handles everything the user sees and interacts with.
//   It manages the chat display, user input, and calls the TaskManager
//   when tasks need to be added, viewed, or managed.
//   It also handles the Quiz and Activity Log features.
// ============================================================

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PROG6221_POE_Part3
{
    public partial class MainWindow : Window
    {
        // ========== CLASS VARIABLES ==========

        private TaskManager taskManager = new TaskManager();
        private string userName = "";

        // Quiz-related variables
        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
        private int currentQuestionIndex = 0;
        private int quizScore = 0;
        private bool quizActive = false;

        // ==========================================================
        // CONSTRUCTOR
        // ==========================================================
        public MainWindow()
        {
            InitializeComponent();
            AddWelcomeMessage();
        }

        // ==========================================================
        // WELCOME MESSAGE
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
        private void AddToChat(string sender, string message, string color)
        {
            string formattedMessage = $"[{sender}] {message}";

            ChatDisplay.Items.Add(new ListBoxItem
            {
                Content = formattedMessage,
                Foreground = GetBrush(color)
            });

            ChatDisplay.ScrollIntoView(ChatDisplay.Items[ChatDisplay.Items.Count - 1]);
        }

        // ==========================================================
        // COLOR HELPER
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
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        // ==========================================================
        // ENTER KEY HANDLER
        // ==========================================================
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }

        // ==========================================================
        // DISPLAY QUIZ QUESTION
        // ==========================================================
        private void DisplayCurrentQuestion()
        {
            if (!quizActive || currentQuestionIndex >= quizQuestions.Count)
            {
                quizActive = false;
                AddToChat("Bot", $"🎯 Quiz complete! Your score: {quizScore}/{quizQuestions.Count}", "Green");
                AddToChat("Bot", GetQuizFeedback(quizScore, quizQuestions.Count), "Green");
                return;
            }

            var q = quizQuestions[currentQuestionIndex];
            AddToChat("Bot", $"Question {currentQuestionIndex + 1}: {q.Question}", "Green");

            for (int i = 0; i < q.Options.Count; i++)
            {
                AddToChat("Bot", $"  {i + 1}. {q.Options[i]}", "Green");
            }
        }

        // ==========================================================
        // QUIZ FEEDBACK
        // ==========================================================
        private string GetQuizFeedback(int score, int total)
        {
            double percentage = (double)score / total * 100;
            if (percentage >= 80) return "🌟 Excellent! You're a cybersecurity pro!";
            else if (percentage >= 60) return "👍 Good job! Keep learning to improve your score!";
            else if (percentage >= 40) return "📚 Nice try! Review the topics and try again!";
            else return "💪 Don't give up! Cybersecurity is important - try the quiz again!";
        }

        // ==========================================================
        // MAIN PROCESSING METHOD
        // ==========================================================
        private void ProcessUserInput()
        {
            string userMessage = UserInput.Text.Trim();

            // ========== VALIDATION ==========
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                AddToChat("System", "Please type something!", "Yellow");
                UserInput.Clear();
                UserInput.Focus();
                return;
            }

            AddToChat("You", userMessage, "Magenta");

            // ========== GET USER'S NAME ==========
            if (string.IsNullOrEmpty(userName))
            {
                userName = userMessage;
                AddToChat("Bot", $"Nice to meet you, {userName}! 😊", "Green");
                AddToChat("Bot", "Try typing: 'add task', 'view tasks', 'start quiz', or 'activity log'", "Green");
                AddToChat("Bot", "Type 'exit' to quit.", "Green");
                UserInput.Clear();
                UserInput.Focus();
                return;
            }

            // ========== EXIT COMMAND ==========
            if (userMessage.ToLower() == "exit")
            {
                AddToChat("Bot", $"Goodbye, {userName}! Stay safe online! 😊", "Green");
                UserInput.IsEnabled = false;
                SendButton.IsEnabled = false;
                return;
            }

            // ========== QUIZ MODE ==========
            // If the quiz is active, check if the user typed a number (1-4)
            if (quizActive && currentQuestionIndex < quizQuestions.Count)
            {
                if (int.TryParse(userMessage, out int answer) && answer >= 1 && answer <= 4)
                {
                    var q = quizQuestions[currentQuestionIndex];
                    if (answer - 1 == q.CorrectAnswerIndex)
                    {
                        quizScore++;
                        AddToChat("Bot", "✅ Correct! Good job!", "Green");
                    }
                    else
                    {
                        AddToChat("Bot", $"❌ Sorry, the correct answer was: {q.Options[q.CorrectAnswerIndex]}", "Green");
                    }

                    currentQuestionIndex++;
                    DisplayCurrentQuestion();
                    UserInput.Clear();
                    UserInput.Focus();
                    return;
                }
                else
                {
                    AddToChat("Bot", "Please type a number between 1 and 4 for your answer.", "Yellow");
                    UserInput.Clear();
                    UserInput.Focus();
                    return;
                }
            }

            // ========== PROCESS COMMANDS ==========
            string lowerMsg = userMessage.ToLower();

            // --- ADD TASK ---
            if (lowerMsg.StartsWith("add task") || lowerMsg.Contains("add a task"))
            {
                string taskName = userMessage.Substring(userMessage.IndexOf("task") + 4).Trim();
                if (!string.IsNullOrEmpty(taskName))
                {
                    taskManager.AddTask(taskName, "No description", "");
                    AddToChat("Bot", $"✅ Task '{taskName}' added successfully!", "Green");
                }
                else
                {
                    AddToChat("Bot", "Please specify the task name. Example: 'add task: Review privacy settings'", "Green");
                }
            }

            // --- VIEW TASKS ---
            else if (lowerMsg.Contains("view tasks") || lowerMsg.Contains("show tasks") || lowerMsg.Contains("tasks"))
            {
                var tasks = taskManager.GetTasks();
                if (tasks.Count == 0)
                {
                    AddToChat("Bot", "📋 You have no tasks yet. Add one by typing: 'add task: Your task name'", "Green");
                }
                else
                {
                    AddToChat("Bot", $"📋 You have {tasks.Count} tasks:", "Green");
                    foreach (var task in tasks)
                    {
                        AddToChat("Bot", $"  • {task.ToString()}", "Green");
                    }
                }
            }

            // --- START QUIZ ---
            else if (lowerMsg.Contains("start quiz") || lowerMsg.Contains("quiz"))
            {
                quizQuestions = taskManager.GetQuizQuestions();
                currentQuestionIndex = 0;
                quizScore = 0;
                quizActive = true;

                AddToChat("Bot", "🎯 Welcome to the Cybersecurity Quiz!", "Green");
                AddToChat("Bot", $"There are {quizQuestions.Count} questions. Type the number of your answer (1-4).", "Green");
                AddToChat("Bot", $"Let's start! Question 1 of {quizQuestions.Count}:", "Green");
                DisplayCurrentQuestion();
            }

            // --- ACTIVITY LOG ---
            else if (lowerMsg.Contains("activity log") || lowerMsg.Contains("log"))
            {
                var log = taskManager.GetActivityLog();
                if (log.Count == 0)
                {
                    AddToChat("Bot", "📜 No activity logged yet. Start using the chatbot to build your log!", "Green");
                }
                else
                {
                    AddToChat("Bot", "📜 Here's your recent activity:", "Green");
                    foreach (var entry in log)
                    {
                        AddToChat("Bot", $"  • {entry}", "Green");
                    }
                }
            }

            // --- DEFAULT RESPONSE ---
            else
            {
                AddToChat("Bot", $"I'm here to help, {userName}! Try: 'add task', 'view tasks', 'start quiz', or 'activity log'", "Green");
            }

            UserInput.Clear();
            UserInput.Focus();
        }

        // ==========================================================
        // BUTTON CLICK HANDLERS
        // ==========================================================

        private void ViewTasksButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput.Text = "view tasks";
            ProcessUserInput();
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput.Text = "start quiz";
            ProcessUserInput();
        }

        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput.Text = "activity log";
            ProcessUserInput();
        }
    }
}