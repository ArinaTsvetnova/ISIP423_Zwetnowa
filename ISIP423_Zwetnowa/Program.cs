using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagementSystem
{
    // ����� ��� ���� ����� � ������������
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

        // �������� � �������������
        public string Name => _name;
        public int Age => _age;
        public string Email => _email;
        public string Phone => _phone;
        public string Id => _id;
    }

    // ����� �������
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
            return $"�������: {_name} (ID: {_studentId}), �������: {_age}, Email: {_email}";
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

    // ����� �������������
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
            return $"�������������: {_name}, Email: {_email}";
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

    // ����� ����
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
            string teacherInfo = _teacher != null ? _teacher.Name : "�� ��������";
            return $"����: {_name} ({_courseCode})\n��������: {_description}\n�������������: {teacherInfo}\n���������: {_students.Count}";
        }

        public List<Student> GetStudents()
        {
            return new List<Student>(_students);
        }

        // ��������
        public string CourseCode => _courseCode;
        public string Name => _name;
        public string Description => _description;
        public Teacher Teacher => _teacher;
    }

    // �������� ����� ������� ������������
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

        // ������ ��� ������ �� ����������
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

        // ������ ��� ������ � ���������������
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

        // ������ ��� ������ � �������
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

        // ������ ��� ���������� ���������
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

    // ����� ��� ����������� ����������
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
            // ��������� ������� ������ ��� ������������
            _system.AddTeacher("���� ������", 45, "ivan.petrov@university.ru", "+79161234567");
            _system.AddTeacher("����� ��������", 38, "maria.sidorova@university.ru", "+79167654321");

            _system.AddStudent("������� ������", 20, "alex.ivanov@student.ru", "+79031112233", "1");
            _system.AddStudent("����� ��������", 19, "elena.smirnova@student.ru", "+79032223344", "2");
            _system.AddStudent("������� ������", 21, "dmitry.kozlov@student.ru", "+79033334455", "3");

            _system.AddCourse("MATH101", "������ ����������", "������ ������ ����������");
            _system.AddCourse("CS101", "������ ����������������", "�������� � ���������������� �� C#");
            _system.AddCourse("PHYS101", "������", "������������ ��������");

            // ��������� �������������� �� �����
            _system.AssignTeacherToCourse("T001", "MATH101");
            _system.AssignTeacherToCourse("T002", "CS101");
            _system.AssignTeacherToCourse("T001", "PHYS101");

            // ���������� ��������� �� �����
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
                        Console.WriteLine("����� �� �������");
                        return;
                    default:
                        Console.WriteLine("�������� �����. ���������� �����.");
                        break;
                }

                Console.WriteLine("\n������� ����� ������� ��� �����������");
                Console.ReadKey();
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("�������� �� ������");
            Console.WriteLine("1. ���������� ����������");
            Console.WriteLine("2. ���������� ���������������");
            Console.WriteLine("3. ���������� �������");
            Console.WriteLine("0. �����");
            Console.Write("�������� �����: ");
        }

        private void ManageStudents()
        {
            while (true)
            {
                Console.WriteLine("���������� ����������");
                Console.WriteLine("1. �������� ��������");
                Console.WriteLine("2. ����������� ���� ���������");
                Console.WriteLine("3. �������� �������� �� ����");
                Console.WriteLine("4. ����������� ����� ��������");
                Console.WriteLine("0. �����");
                Console.Write("�������� �����: ");

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
                        Console.WriteLine("�������� �����.");
                        break;
                }

                Console.WriteLine("\n������� ����� ������� ��� �����������");
                Console.ReadKey();
            }
        }

        private void AddStudent()
        {
            Console.Write("���: ");
            string name = Console.ReadLine();
            Console.Write("�������: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("�������: ");
            string phone = Console.ReadLine();
            Console.Write("Student ID: ");
            string studentId = Console.ReadLine();

            if (_system.AddStudent(name, age, email, phone, studentId))
            {
                Console.WriteLine("������� ������� ��������!");
            }
            else
            {
                Console.WriteLine("������: ������� � ����� ID ��� ����������.");
            }
        }

        private void ShowAllStudents()
        {
            var students = _system.GetAllStudents();
            Console.WriteLine("\n��� ��������");
            foreach (var student in students)
            {
                Console.WriteLine(student.GetInfo());
            }
        }

        private void EnrollStudentInCourse()
        {
            Console.Write("������� ID ��������: ");
            string studentId = Console.ReadLine();
            Console.Write("������� ��� �����: ");
            string courseCode = Console.ReadLine();

            if (_system.EnrollStudentInCourse(studentId, courseCode))
            {
                Console.WriteLine("������� ������� ������� �� ����!");
            }
            else
            {
                Console.WriteLine("������ ��� ������ �� ����.");
            }
        }

        private void ShowStudentCourses()
        {
            Console.Write("������� ID ��������: ");
            string studentId = Console.ReadLine();
            Student student = _system.GetStudent(studentId);

            if (student != null)
            {
                var courses = student.GetCourses();
                Console.WriteLine($"\n����� �������� {student.Name}:");
                foreach (var course in courses)
                {
                    Console.WriteLine($"- {course.Name} ({course.CourseCode})");
                }
            }
            else
            {
                Console.WriteLine("������� �� ������.");
            }
        }

        private void ManageTeachers()
        {
            while (true)
            {
                Console.WriteLine("�������� ���������������");
                Console.WriteLine("1. �������� �������������");
                Console.WriteLine("2. ����������� ���� ��������������");
                Console.WriteLine("3. ��������� ������������� �� ����");
                Console.WriteLine("0. �����");
                Console.Write("�������� �����: ");

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
                        Console.WriteLine("�������� �����.");
                        break;
                }

                Console.WriteLine("\n������� ����� ������� ��� �����������");
                Console.ReadKey();
            }
        }

        private void AddTeacher()
        {
            Console.Write("���: ");
            string name = Console.ReadLine();
            Console.Write("�������: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("�������: ");
            string phone = Console.ReadLine();

            if (_system.AddTeacher(name, age, email, phone))
            {
                Console.WriteLine("������������� ������� ��������!");
            }
            else
            {
                Console.WriteLine("������ ��� ���������� �������������.");
            }
        }

        private void ShowAllTeachers()
        {
            var teachers = _system.GetAllTeachers();
            Console.WriteLine("\n��� �������������");
            foreach (var teacher in teachers)
            {
                Console.WriteLine(teacher.GetInfo());
            }
        }

        private void AssignTeacherToCourse()
        {
            Console.Write("������� ID �������������: ");
            string teacherId = Console.ReadLine();
            Console.Write("������� ��� �����: ");
            string courseCode = Console.ReadLine();

            if (_system.AssignTeacherToCourse(teacherId, courseCode))
            {
                Console.WriteLine("������������� ������� �������� �� ����!");
            }
            else
            {
                Console.WriteLine("������ ��� ���������� �������������.");
            }
        }

        private void ManageCourses()
        {
            while (true)
            {
                Console.WriteLine("��������� �������");
                Console.WriteLine("1. �������� ����");
                Console.WriteLine("2. ����������� ��� �����");
                Console.WriteLine("3. ����������� ���������� � �����");
                Console.WriteLine("4. ����������� ��������� �����");
                Console.WriteLine("0. �����");
                Console.Write("�������� �����: ");

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
                        Console.WriteLine("�������� �����.");
                        break;
                }

                Console.WriteLine("\n������� ����� ������� ��� �����������");
                Console.ReadKey();
            }
        }

        private void AddCourse()
        {
            Console.Write("��� �����: ");
            string courseCode = Console.ReadLine();
            Console.Write("��������: ");
            string name = Console.ReadLine();
            Console.Write("��������: ");
            string description = Console.ReadLine();

            if (_system.AddCourse(courseCode, name, description))
            {
                Console.WriteLine("���� ������� ��������!");
            }
            else
            {
                Console.WriteLine("������: ���� � ����� ����� ��� ����������.");
            }
        }

        private void ShowAllCourses()
        {
            var courses = _system.GetAllCourses();
            Console.WriteLine("\n��� �����");
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Name} ({course.CourseCode})");
            }
        }

        private void ShowCourseInfo()
        {
            Console.Write("������� ��� �����: ");
            string courseCode = Console.ReadLine();
            Course course = _system.GetCourse(courseCode);

            if (course != null)
            {
                Console.WriteLine(course.GetInfo());
            }
            else
            {
                Console.WriteLine("���� �� ������.");
            }
        }

        private void ShowCourseStudents()
        {
            Console.Write("������� ��� �����: ");
            string courseCode = Console.ReadLine();
            Course course = _system.GetCourse(courseCode);

            if (course != null)
            {
                var students = course.GetStudents();
                Console.WriteLine($"\n�������� ����� {course.Name}:");
                foreach (var student in students)
                {
                    Console.WriteLine($"- {student.Name} ({student.StudentId})");
                }
            }
            else
            {
                Console.WriteLine("���� �� ������.");
            }
        }

        // ������� ����� ���������
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
