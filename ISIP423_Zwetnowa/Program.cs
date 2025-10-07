using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagementSystem
{
    // Класс для всех людей в университете
    public abstract class Person
    {
        protected string _name;
        protected int _age;
        protected string _email;
        protected string _phone;
        protected string _id;

        public Person(string name, int age, string email, string phone)
        {
            _name = name;
            _age = age;
            _email = email;
            _phone = phone;
            _id = GenerateId();
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }

        public abstract string GetInfo();

        // Свойства с инкапсуляцией
        public string Name => _name;
        public int Age => _age;
        public string Email => _email;
        public string Phone => _phone;
        public string Id => _id;
    }

    // Класс Студент
    public class Student : Person
    {
        private string _studentId;
        private List<Course> _courses;

        public Student(string name, int age, string email, string phone, string studentId)
            : base(name, age, email, phone)
        {
            _studentId = studentId;
            _courses = new List<Course>();
        }

        public override string GetInfo()
        {
            return $"Студент: {_name} (ID: {_studentId}), Возраст: {_age}, Email: {_email}";
        }

        public bool EnrollCourse(Course course)
        {
            if (!_courses.Contains(course))
            {
                _courses.Add(course);
                course.AddStudent(this);
                return true;
            }
            return false;
        }

        public List<Course> GetCourses()
        {
            return new List<Course>(_courses);
        }

        public string StudentId => _studentId;
    }

    // Класс Преподаватель
    public class Teacher : Person
    {
        private string _department;
        private List<Course> _courses;

        public Teacher(string name, int age, string email, string phone)
            : base(name, age, email, phone)
        {
            _courses = new List<Course>();
        }

        public override string GetInfo()
        {
            return $"Преподаватель: {_name}, Email: {_email}";
        }

        public bool AssignToCourse(Course course)
        {
            if (!_courses.Contains(course))
            {
                _courses.Add(course);
                course.AssignTeacher(this);
                return true;
            }
            return false;
        }

        public List<Course> GetCourses()
        {
            return new List<Course>(_courses);
        }
    }

    // Класс Курс
    public class Course
    {
        private string _courseCode;
        private string _name;
        private string _description;
        private Teacher _teacher;
        private List<Student> _students;

        public Course(string courseCode, string name, string description)
        {
            _courseCode = courseCode;
            _name = name;
            _description = description;
            _students = new List<Student>();
        }

        public bool AddStudent(Student student)
        {
            if (!_students.Contains(student))
            {
                _students.Add(student);
                return true;
            }
            return false;
        }

        public bool AssignTeacher(Teacher teacher)
        {
            _teacher = teacher;
            return true;
        }

        public string GetInfo()
        {
            string teacherInfo = _teacher != null ? _teacher.Name : "Не назначен";
            return $"Курс: {_name} ({_courseCode})\nОписание: {_description}\nПреподаватель: {teacherInfo}\nСтудентов: {_students.Count}";
        }

        public List<Student> GetStudents()
        {
            return new List<Student>(_students);
        }

        // Свойства
        public string CourseCode => _courseCode;
        public string Name => _name;
        public string Description => _description;
        public Teacher Teacher => _teacher;
    }

    // Основной класс системы университета
    public class UniversitySystem
    {
        private Dictionary<string, Student> _students;
        private Dictionary<string, Teacher> _teachers;
        private Dictionary<string, Course> _courses;

        public UniversitySystem()
        {
            _students = new Dictionary<string, Student>();
            _teachers = new Dictionary<string, Teacher>();
            _courses = new Dictionary<string, Course>();
        }

        // Методы для работы со студентами
        public bool AddStudent(string name, int age, string email, string phone, string studentId)
        {
            if (_students.ContainsKey(studentId))
                return false;

            _students[studentId] = new Student(name, age, email, phone, studentId);
            return true;
        }

        public Student GetStudent(string studentId)
        {
            return _students.ContainsKey(studentId) ? _students[studentId] : null;
        }

        public List<Student> GetAllStudents()
        {
            return _students.Values.ToList();
        }

        // Методы для работы с преподавателями
        public bool AddTeacher(string name, int age, string email, string phone)
        {
            string teacherId = $"T{_teachers.Count + 1:D3}";
            _teachers[teacherId] = new Teacher(name, age, email, phone);
            return true;
        }

        public Teacher GetTeacher(string teacherId)
        {
            return _teachers.ContainsKey(teacherId) ? _teachers[teacherId] : null;
        }

        public List<Teacher> GetAllTeachers()
        {
            return _teachers.Values.ToList();
        }

        // Методы для работы с курсами
        public bool AddCourse(string courseCode, string name, string description)
        {
            if (_courses.ContainsKey(courseCode))
                return false;

            _courses[courseCode] = new Course(courseCode, name, description);
            return true;
        }

        public Course GetCourse(string courseCode)
        {
            return _courses.ContainsKey(courseCode) ? _courses[courseCode] : null;
        }

        public List<Course> GetAllCourses()
        {
            return _courses.Values.ToList();
        }

        // Методы для связывания сущностей
        public bool EnrollStudentInCourse(string studentId, string courseCode)
        {
            Student student = GetStudent(studentId);
            Course course = GetCourse(courseCode);

            if (student != null && course != null)
            {
                return student.EnrollCourse(course);
            }
            return false;
        }

        public bool AssignTeacherToCourse(string teacherId, string courseCode)
        {
            Teacher teacher = GetTeacher(teacherId);
            Course course = GetCourse(courseCode);

            if (teacher != null && course != null)
            {
                return teacher.AssignToCourse(course);
            }
            return false;
        }
    }

    // Класс для консольного интерфейса
    public class UniversityConsole
    {
        private UniversitySystem _system;

        public UniversityConsole()
        {
            _system = new UniversitySystem();
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            // Добавляем примеры данных для демонстрации
            _system.AddTeacher("Иван Петров", 45, "ivan.petrov@university.ru", "+79161234567");
            _system.AddTeacher("Мария Сидорова", 38, "maria.sidorova@university.ru", "+79167654321");

            _system.AddStudent("Алексей Иванов", 20, "alex.ivanov@student.ru", "+79031112233", "1");
            _system.AddStudent("Елена Смирнова", 19, "elena.smirnova@student.ru", "+79032223344", "2");
            _system.AddStudent("Дмитрий Козлов", 21, "dmitry.kozlov@student.ru", "+79033334455", "3");

            _system.AddCourse("MATH101", "Высшая математика", "Основы высшей математики");
            _system.AddCourse("CS101", "Основы программирования", "Введение в программирование на C#");
            _system.AddCourse("PHYS101", "Физика", "Классическая механика");

            // Назначаем преподавателей на курсы
            _system.AssignTeacherToCourse("T001", "MATH101");
            _system.AssignTeacherToCourse("T002", "CS101");
            _system.AssignTeacherToCourse("T001", "PHYS101");

            // Записываем студентов на курсы
            _system.EnrollStudentInCourse("1", "MATH101");
            _system.EnrollStudentInCourse("1", "CS101");
            _system.EnrollStudentInCourse("2", "CS101");
            _system.EnrollStudentInCourse("3", "PHYS101");
        }

        public void Run()
        {
            while (true)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageStudents();
                        break;
                    case "2":
                        ManageTeachers();
                        break;
                    case "3":
                        ManageCourses();
                        break;
                    case "0":
                        Console.WriteLine("Выход из системы");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("Выберете из списка");
            Console.WriteLine("1. Управление студентами");
            Console.WriteLine("2. Управление преподавателями");
            Console.WriteLine("3. Управление курсами");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");
        }

        private void ManageStudents()
        {
            while (true)
            {
                Console.WriteLine("Управление студентами");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Просмотреть всех студентов");
                Console.WriteLine("3. Записать студента на курс");
                Console.WriteLine("4. Просмотреть курсы студента");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        ShowAllStudents();
                        break;
                    case "3":
                        EnrollStudentInCourse();
                        break;
                    case "4":
                        ShowStudentCourses();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
            }
        }

        private void AddStudent()
        {
            Console.Write("Имя: ");
            string name = Console.ReadLine();
            Console.Write("Возраст: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Телефон: ");
            string phone = Console.ReadLine();
            Console.Write("Student ID: ");
            string studentId = Console.ReadLine();

            if (_system.AddStudent(name, age, email, phone, studentId))
            {
                Console.WriteLine("Студент успешно добавлен!");
            }
            else
            {
                Console.WriteLine("Ошибка: студент с таким ID уже существует.");
            }
        }

        private void ShowAllStudents()
        {
            var students = _system.GetAllStudents();
            Console.WriteLine("\nВсе студенты");
            foreach (var student in students)
            {
                Console.WriteLine(student.GetInfo());
            }
        }

        private void EnrollStudentInCourse()
        {
            Console.Write("Введите ID студента: ");
            string studentId = Console.ReadLine();
            Console.Write("Введите код курса: ");
            string courseCode = Console.ReadLine();

            if (_system.EnrollStudentInCourse(studentId, courseCode))
            {
                Console.WriteLine("Студент успешно записан на курс!");
            }
            else
            {
                Console.WriteLine("Ошибка при записи на курс.");
            }
        }

        private void ShowStudentCourses()
        {
            Console.Write("Введите ID студента: ");
            string studentId = Console.ReadLine();
            Student student = _system.GetStudent(studentId);

            if (student != null)
            {
                var courses = student.GetCourses();
                Console.WriteLine($"\nКурсы студента {student.Name}:");
                foreach (var course in courses)
                {
                    Console.WriteLine($"- {course.Name} ({course.CourseCode})");
                }
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
        }

        private void ManageTeachers()
        {
            while (true)
            {
                Console.WriteLine("Упавлние преподавателями");
                Console.WriteLine("1. Добавить преподавателя");
                Console.WriteLine("2. Просмотреть всех преподавателей");
                Console.WriteLine("3. Назначить преподавателя на курс");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTeacher();
                        break;
                    case "2":
                        ShowAllTeachers();
                        break;
                    case "3":
                        AssignTeacherToCourse();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
            }
        }

        private void AddTeacher()
        {
            Console.Write("Имя: ");
            string name = Console.ReadLine();
            Console.Write("Возраст: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Телефон: ");
            string phone = Console.ReadLine();

            if (_system.AddTeacher(name, age, email, phone))
            {
                Console.WriteLine("Преподаватель успешно добавлен!");
            }
            else
            {
                Console.WriteLine("Ошибка при добавлении преподавателя.");
            }
        }

        private void ShowAllTeachers()
        {
            var teachers = _system.GetAllTeachers();
            Console.WriteLine("\nВсе преподаватели");
            foreach (var teacher in teachers)
            {
                Console.WriteLine(teacher.GetInfo());
            }
        }

        private void AssignTeacherToCourse()
        {
            Console.Write("Введите ID преподавателя: ");
            string teacherId = Console.ReadLine();
            Console.Write("Введите код курса: ");
            string courseCode = Console.ReadLine();

            if (_system.AssignTeacherToCourse(teacherId, courseCode))
            {
                Console.WriteLine("Преподаватель успешно назначен на курс!");
            }
            else
            {
                Console.WriteLine("Ошибка при назначении преподавателя.");
            }
        }

        private void ManageCourses()
        {
            while (true)
            {
                Console.WriteLine("Управлени крусами");
                Console.WriteLine("1. Добавить курс");
                Console.WriteLine("2. Просмотреть все курсы");
                Console.WriteLine("3. Просмотреть информацию о курсе");
                Console.WriteLine("4. Просмотреть студентов курса");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCourse();
                        break;
                    case "2":
                        ShowAllCourses();
                        break;
                    case "3":
                        ShowCourseInfo();
                        break;
                    case "4":
                        ShowCourseStudents();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
            }
        }

        private void AddCourse()
        {
            Console.Write("Код курса: ");
            string courseCode = Console.ReadLine();
            Console.Write("Название: ");
            string name = Console.ReadLine();
            Console.Write("Описание: ");
            string description = Console.ReadLine();

            if (_system.AddCourse(courseCode, name, description))
            {
                Console.WriteLine("Курс успешно добавлен!");
            }
            else
            {
                Console.WriteLine("Ошибка: курс с таким кодом уже существует.");
            }
        }

        private void ShowAllCourses()
        {
            var courses = _system.GetAllCourses();
            Console.WriteLine("\nВсе курсы");
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Name} ({course.CourseCode})");
            }
        }

        private void ShowCourseInfo()
        {
            Console.Write("Введите код курса: ");
            string courseCode = Console.ReadLine();
            Course course = _system.GetCourse(courseCode);

            if (course != null)
            {
                Console.WriteLine(course.GetInfo());
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
        }

        private void ShowCourseStudents()
        {
            Console.Write("Введите код курса: ");
            string courseCode = Console.ReadLine();
            Course course = _system.GetCourse(courseCode);

            if (course != null)
            {
                var students = course.GetStudents();
                Console.WriteLine($"\nСтуденты курса {course.Name}:");
                foreach (var student in students)
                {
                    Console.WriteLine($"- {student.Name} ({student.StudentId})");
                }
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
        }

        // Главный класс программы
        class Program
        {
            static void Main(string[] args)
            {
                UniversityConsole console = new UniversityConsole();
                console.Run();
            }
        }
    }
}
