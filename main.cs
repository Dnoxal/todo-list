using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class TaskItem
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        return $"{(IsCompleted ? "[x]" : "[ ]")} {Description}";
    }

    public string ToFileString()
    {
        return $"{IsCompleted}|{Description}";
    }

    public static TaskItem FromFileString(string line)
    {
        var parts = line.Split('|');
        return new TaskItem
        {
            IsCompleted = parts[0] == "True",
            Description = parts[1]
        };
    }
}

class TodoApp
{
    private const string FilePath = "tasks.txt";
    private List<TaskItem> tasks = new List<TaskItem>();

    public void Run()
    {
        LoadTasks();

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- To-Do List ---");
            DisplayTasks();
            Console.WriteLine("Options: [1] Add [2] Complete [3] Delete [4] Save & Exit");
            Console.Write("Choice: ");

            switch (Console.ReadLine())
            {
                case "1": AddTask(); break;
                case "2": CompleteTask(); break;
                case "3": DeleteTask(); break;
                case "4": SaveTasks(); running = false; break;
                default: Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private void AddTask()
    {
        Console.Write("Enter task description: ");
        string desc = Console.ReadLine();
        tasks.Add(new TaskItem { Description = desc, IsCompleted = false });
        Console.WriteLine("Task added.");
    }

    private void CompleteTask()
    {
        Console.Write("Enter task number to mark as complete: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
        {
            tasks[index - 1].IsCompleted = true;
            Console.WriteLine("Task marked as completed.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
    }

    private void DeleteTask()
    {
        Console.Write("Enter task number to delete: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
        {
            tasks.RemoveAt(index - 1);
            Console.WriteLine("Task deleted.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
    }

    private void DisplayTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found.");
        }
        else
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }
    }

    private void SaveTasks()
    {
        File.WriteAllLines(FilePath, tasks.Select(t => t.ToFileString()));
        Console.WriteLine("Tasks saved.");
    }

    private void LoadTasks()
    {
        if (File.Exists(FilePath))
        {
            var lines = File.ReadAllLines(FilePath);
            tasks = lines.Select(TaskItem.FromFileString).ToList();
            Console.WriteLine("Loaded previous tasks.");
        }
    }
}

class Program
{
    static void Main()
    {
        new TodoApp().Run();
    }
}