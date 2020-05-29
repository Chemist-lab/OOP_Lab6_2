using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace W1
{
    public interface IDataM
    {
        public abstract void HoursOnProject(List<WorkingDay> Days);
        public abstract void AverageTimeWorking(List<WorkingDay> Days);
    }

    public abstract class Employee
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public abstract void HoursOnProject(List<WorkingDay> Days);
        public abstract void AverageTimeWorking(List<WorkingDay> Days);
    }
    public class WorkingDay : Employee
    {
        public string Date { get; set; }
        public string HoursCount { get; set; }
        public string ProjectName { get; set; }

        public override void HoursOnProject(List<WorkingDay> Days)
        {
            Console.Clear();
            var Group = from hours in Days
                        group hours by hours.ProjectName into g
                        select new
                        {
                            Name = g.Key,
                            Count = g.Count(),
                            Works = from p in g select p
                        };
            foreach (var group in Group)
            {
                int hrs = 0;
                foreach (var phone in group.Works)
                {
                    hrs += Convert.ToInt32(phone.HoursCount);
                }
                Console.WriteLine($"{group.Name} : {hrs} hours");
                Console.WriteLine();
            }
        }
        public override void AverageTimeWorking(List<WorkingDay> Days)
        {
            Console.Clear();
            var Group = from AVhours in Days
                        group AVhours by AVhours.Name into g
                        select new
                        {
                            Name = g.Key,
                            Count = g.Count(),
                            Works = from p in g select p
                        };
            foreach (var group in Group)
            {
                float hrs = 0;
                foreach (var gr in group.Works)
                {
                    hrs += Convert.ToInt32(gr.HoursCount);
                }
                Console.WriteLine($"{group.Name} : {hrs / group.Count} hours");
                Console.WriteLine();
            }
        }


    }
    class Program
    {
        private static string FileName = "Data.json";
        private static string FilePath = @"Data.json";
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("╔════════════╤══════════════════════════════╗");
                Console.WriteLine("   Hot key   │            Function       ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      A      │          Add new day  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      C      │          Change day  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      D      │          Delete day ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      T      │        Show all days  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      H      │      Average working hours  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      P      │        Hours on project  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("      M      │     Days with maximum load  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("    Space    │         Clear console  ");
                Console.WriteLine("╠════════════╪══════════════════════════════╣");
                Console.WriteLine("     Esc     │          Exit program  ");
                Console.WriteLine("╚════════════╧══════════════════════════════╝");
                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Close();
                }
                var Days = JsonConvert.DeserializeObject<List<WorkingDay>>(File.ReadAllText(FilePath));
                WorkingDay Wd = new WorkingDay();
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        if (Days == null)
                        {
                            Days = new List<WorkingDay>();
                            Days.Add(CreateNewDay());
                        }
                        else
                        {
                            Days.Add(CreateNewDay());
                        }
                        break;
                    case ConsoleKey.C:
                        ChangeData(Days);
                        break;
                    case ConsoleKey.D:
                        DelteDay(Days);
                        break;
                    case ConsoleKey.T:
                        ShowAll(Days);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.H:
                        Wd.AverageTimeWorking(Days);
                        break;
                    case ConsoleKey.P:
                        Wd.HoursOnProject(Days);
                        break;
                    case ConsoleKey.M:
                        MaxLoad(Days);
                        break;
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        break;
                }
                string serialize = JsonConvert.SerializeObject(Days, Formatting.Indented);
                if (serialize.Count() > 1)
                {
                    if (!File.Exists(FileName))
                    {
                        File.Create(FileName).Close();
                    }
                    File.WriteAllText(FilePath, serialize, Encoding.UTF8);
                }
            }
        }
        public static WorkingDay CreateNewDay()
        {
            Console.Clear();
            WorkingDay Day = new WorkingDay();
            Console.WriteLine("Enter name of employee");
            Day.Name = Console.ReadLine();
            Console.WriteLine("Enter position in team");
            Day.Position = Console.ReadLine();
            Console.WriteLine("Enter date of day like 01.02.2000");
            Day.Date = Console.ReadLine();
            Console.WriteLine("Enter hours count");
            Day.HoursCount = Console.ReadLine();
            Console.WriteLine("Enter project name");
            Day.ProjectName = Console.ReadLine();
            return Day;
        }
        public static void ChangeData(List<WorkingDay> Days)
        {
            Console.WriteLine("Enter date of day that`s you want to change");
            var s = Console.ReadLine();
            WorkingDay day = Days.Find(x => x.Date == s);
            if (day != null)
            {
                Console.WriteLine("Enter value of day that`s you want to change \n1)Name\n2)Position\n3)Date like 01.02.2000\n4)Hours count\n5)Project Name");
                char a = Console.ReadKey().KeyChar;
                Console.WriteLine("Enter new value");
                switch (a)
                {
                    case '1':
                        day.Name = Console.ReadLine();
                        break;
                    case '2':
                        day.Position = Console.ReadLine();
                        break;
                    case '3':
                        day.Date = Console.ReadLine();
                        break;
                    case '4':
                        day.HoursCount = Console.ReadLine();
                        break;
                    case '5':
                        day.ProjectName = Console.ReadLine();
                        break;
                }
            }
        }
        public static void DelteDay(List<WorkingDay> Days)
        {
            if (Days != null)
            {
                Console.WriteLine("Enter date of day that`s you want to delete");
                var s = Console.ReadLine();
                Days.RemoveAll(x => x.Date == s);

            }
        }
        public static void ShowAll(List<WorkingDay> Days)
        {
            Console.Clear();
            Console.WriteLine("╔════════════╤════════════╤══════════╤═════════════╤══════════════╗");
            Console.WriteLine("     Name    │  Position  │   Date   │ Hours Count │ Project Name");
            Console.WriteLine("╠════════════╪════════════╪══════════╪═════════════╪══════════════╣");
            for (int i = 0; i < Days.Count; i++)
            {
                Console.WriteLine("{0,10} {1, 10} {2, 15} {3, 10} {4, 13}", Days[i].Name, Days[i].Position, Days[i].Date, Days[i].HoursCount, Days[i].ProjectName);
                Console.WriteLine("╠════════════╪════════════╪══════════╪═════════════╪══════════════╣");
            }
            Console.WriteLine("╚════════════╧════════════╧══════════╧═════════════╧══════════════╝");
        }
        public static void MaxLoad(List<WorkingDay> Days)
        {
            Console.Clear();
            var Group = from MALhours in Days
                        group MALhours by MALhours.Date into g
                        select new
                        {
                            Name = g.Key,
                            Count = g.Count(),
                            Works = from p in g select p
                        };
            foreach (var group in Group)
            {
                float hrs = 0;
                foreach (var gr in group.Works)
                {
                    hrs += Convert.ToInt32(gr.HoursCount);
                }
                Console.WriteLine($"{group.Name} : {hrs} hours");
                Console.WriteLine();
            }
        }

    }
}
