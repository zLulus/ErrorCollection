using ExpressionConvertFailDemo.Entities;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpressionConvertFailDemo
{
    class Program
    {
        static DataConnection connection { get; set; }
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the number to perform the corresponding operation:");
            Console.WriteLine("1. Create database and initialize data");
            Console.WriteLine("2. Query data(throw exception)");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "1")
                {
                    await CreateDbAndInitializeData();
                    Console.WriteLine("Finish");
                }
                else if (input == "2")
                {
                    await TryToQuery();
                }
            }

        }

        private static async Task CreateDbAndInitializeData()
        {
            var dbPath = $"{Directory.GetCurrentDirectory()}\\ExpressionConvertFailDemoDatabase.db";
            string connectionStr = $"Data Source={dbPath};Version=3;";
            LinqToDbConnectionOptionsBuilder dbConnectionOptionsBuilder = new LinqToDbConnectionOptionsBuilder();
            dbConnectionOptionsBuilder.UseSQLite(connectionStr);
            dbConnectionOptionsBuilder.WithTraceLevel(TraceLevel.Verbose);
            var dbConnectionOptions = dbConnectionOptionsBuilder.Build();
            connection = new DataConnection(dbConnectionOptions);
            if (File.Exists(dbPath))
            {
                return;
            }
            //create db
            connection.CreateTable<Student>();
            connection.CreateTable<Class>();
            connection.CreateTable<ChooseClass>();

            #region Initialize data
            List<Student> students = new List<Student>();
            students.Add(new Student() { ID = 1, Name = "Aaron", Age = 9 });
            students.Add(new Student() { ID = 2, Name = "Bobby", Age = 10 });
            students.Add(new Student() { ID = 3, Name = "Carl", Age = 11 });
            students.Add(new Student() { ID = 4, Name = "Donald", Age = 13 });
            students.Add(new Student() { ID = 5, Name = "Evan", Age = 15 });
            connection.GetTable<Student>().BulkCopy(students);

            List<Class> classes = new List<Class>();
            classes.Add(new Class() { ID = 1, ClassName = "Math" });
            classes.Add(new Class() { ID = 2, ClassName = "English" });
            connection.GetTable<Class>().BulkCopy(classes);

            List<ChooseClass> chooseClasses = new List<ChooseClass>();
            chooseClasses.Add(new ChooseClass() { ID = 1, StudentID = 1, ClassID = 1 });
            chooseClasses.Add(new ChooseClass() { ID = 2, StudentID = 2, ClassID = 1 });
            chooseClasses.Add(new ChooseClass() { ID = 3, StudentID = 3, ClassID = 1 });
            chooseClasses.Add(new ChooseClass() { ID = 4, StudentID = 4, ClassID = 1 });
            chooseClasses.Add(new ChooseClass() { ID = 5, StudentID = 1, ClassID = 2 });
            chooseClasses.Add(new ChooseClass() { ID = 6, StudentID = 3, ClassID = 2 });
            chooseClasses.Add(new ChooseClass() { ID = 7, StudentID = 5, ClassID = 2 });
            connection.GetTable<ChooseClass>().BulkCopy(chooseClasses);
            #endregion
        }

        private static async Task TryToQuery()
        {
            try
            {
                var studentQueryable= connection.GetTable<Student>() as IQueryable<Student>;
                var classQueryable= connection.GetTable<Class>() as IQueryable<Class>;
                var chooseClassQueryable = connection.GetTable<ChooseClass>() as IQueryable<ChooseClass>;

                var linq = from student in studentQueryable
                           join chooseClass in chooseClassQueryable on student.ID equals chooseClass.StudentID
                           join myClass in classQueryable on chooseClass.ClassID equals myClass.ID
                           select new{ student,chooseClass,myClass};

                //query student who choose 'Math' class and is older than 10 years old.
                linq = linq.Where(x => x.myClass.ClassName == "Math").Where(x=>x.student.Age>10);
                var newStudentQuery = linq.Select(x => x.student);
                //throw exception
                var predicate = Expression.Lambda<Func<Student, bool>>(newStudentQuery.Expression, Expression.Parameter(typeof(Student)));
                var query = connection.GetTable<Student>() as IQueryable<Student>;
                query = query.Where(predicate);
                var data = query.ToList();

                //One of the right ways
                //var ids = newStudentQuery.Select(x => x.ID).ToList();
                //var query = connection.GetTable<Student>() as IQueryable<Student>;
                //query = query.Where(x=>ids.Contains(x.ID));
                //var data = query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
