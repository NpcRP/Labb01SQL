using System;
using System.Data;
using System.Data.SqlClient;

class SchoolManagementSystem
{
    private static readonly string connectionString = "Data Source=(localdb)\\TestDatabase;Initial Catalog=NEwDB;Integrated Security=True";

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("School Management System \n");
            Console.WriteLine("\n>>Initialize School System<<");
            Console.WriteLine("Q. Initialize School System setup");
            Console.WriteLine("===  Management ===");
            Console.WriteLine("A. Manage Classes ");
            Console.WriteLine("B. Manage Courses ");
            Console.WriteLine("C. Manage Students ");
            Console.WriteLine("D. Manage Staff ");
            Console.WriteLine("E. Manage Grades ");
            Console.WriteLine("\n===  Analytics ===");
            Console.WriteLine("F. View Grades By Course ");
            Console.WriteLine("G. Calculate Average Grades ");
            Console.WriteLine("\n===  System ===");
            Console.WriteLine("H. Create New System ");
            Console.WriteLine("I. Reset System ");
            Console.WriteLine("J. Display Database Schema ");
            Console.WriteLine("K. Check Database Connection ");
            Console.WriteLine("X. Exit ");
            Console.Write("\nSelect an option: ");
            var choice = Console.ReadKey().KeyChar.ToString().ToUpper();
            ProcessMenuChoice(choice);
        }
    }

    static void ProcessMenuChoice(string choice)
    {
        switch (choice)
        {
            case "Q": InitializeSchoolSystem(); break; 
            case "A": ManageClasses(); break;
            case "B": ManageCourses(); break;
            case "C": ManageStudents(); break;
            case "D": ManageStaff(); break;
            case "E": ManageGrades(); break;
            case "F": ViewGradesByCourse(); break;
            case "G": CalculateAverageGradesPerCourse(); break;
            case "H": CreateNewSystem();  break;
            case "I": ResetSystem(); break;
            case "J": DisplayRecommendedDatabaseSchema(); break;
            case "K": CheckDatabaseConnection(); break;
            case "X": Console.WriteLine("\nExiting... Thank you for using the system."); return;
            default: Console.WriteLine("\nInvalid option. Please try again."); break;
        }
        Console.WriteLine("\nPress any key to continue...");
    }
    static void InitializeSchoolSystem()
    {
        Console.Clear();
        Console.WriteLine("Initialize School System");

        bool continueInitialization = true;
        while (continueInitialization)
        {
            Console.WriteLine("\nPlease select an option to initialize:");
            Console.WriteLine("1. Add New Class");
            Console.WriteLine("2. Add New Course");
            Console.WriteLine("3. Add New Student");
            Console.WriteLine("4. Add New Staff Member");
            Console.WriteLine("5. Finish Initialization");

            Console.Write("Enter your choice (1-5): ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListClasses();
                    AddNewClassInteractive();
                    break;
                case "2":
                    ListCourses();
                    AddNewCourseInteractive();
                    break;
                case "3":
                    ListStudents();
                    AddNewStudentInteractive();
                    break;
                case "4":
                    ListStaff();
                    AddNewStaffMemberInteractive();
                    break;
                case "5":
                    continueInitialization = false;
                    Console.WriteLine("Initialization completed.");
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }

            if (continueInitialization)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
    static void AddNewClassInteractive()
    {
        Console.Clear();
        ListClasses();
        Console.WriteLine("\nEnter Class Name (or type 'exit' to go back): ");
        var className = Console.ReadLine();

        if (className.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operation cancelled.");
            return;
        }

        if (ClassExists(className))
        {
            Console.WriteLine($"A class with the name '{className}' already exists.");
            return;
        }

        if (string.IsNullOrWhiteSpace(className))
        {
            Console.WriteLine("Class name cannot be empty. Please try again.");
            return;
        }

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO Classes (ClassName) VALUES (@ClassName)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ClassName", className);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Class added successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.Clear();
        ListClasses();
    }

    static void AddNewCourseInteractive()
    {
        Console.Clear();
        ListCourses(); 
        Console.WriteLine("\nEnter Course Name (or type 'exit' to go back): ");
        var courseName = Console.ReadLine().Trim();

        if (courseName.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operation cancelled.");
        }
        else if (string.IsNullOrWhiteSpace(courseName))
        {
            Console.WriteLine("Course name cannot be empty. Please try again.");
        }
        else if (CourseExists(courseName)) 
        {
            Console.WriteLine($"A course with the name '{courseName}' already exists.");
        }
        else
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "INSERT INTO Courses (CourseName) VALUES (@CourseName)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CourseName", courseName);
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            Console.WriteLine("Course added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add course.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    static void AddNewStudentInteractive()
    {
        Console.Clear();
        Console.WriteLine("Adding New Student:");
        Console.WriteLine("Available Classes:");
        ListClasses();

        Console.Write("Enter First Name: ");
        var firstName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(firstName))
        {
            Console.WriteLine("First Name cannot be empty.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Last Name: ");
        var lastName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Last Name cannot be empty.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Birth Date (YYYY-MM-DD): ");
        var birthDateInput = Console.ReadLine();
        if (!DateTime.TryParse(birthDateInput, out DateTime birthDate))
        {
            Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Class ID: ");
        if (!int.TryParse(Console.ReadLine(), out int classId) || !ClassExists(classId))
        {
            Console.WriteLine("Invalid or non-existing Class ID.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }
        if (AddStudent(firstName, lastName, birthDate, classId))
        {
            Console.WriteLine("Student added successfully.");
        }
        else
        {
            Console.WriteLine("Failed to add student.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static bool AddStudent(string firstName, string lastName, DateTime birthDate, int classId)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = @"
INSERT INTO Students (FirstName, LastName, BirthDate, ClassID) 
VALUES (@FirstName, @LastName, @BirthDate, @ClassID)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@BirthDate", birthDate);
                    command.Parameters.AddWithValue("@ClassID", classId);

                    int result = command.ExecuteNonQuery();
                    return result == 1; 
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while adding the student: {ex.Message}");
            return false;
        }
    }
    static void AddNewStaffMemberInteractive()
    {
        Console.Clear();
        ListStaff();
        Console.WriteLine("\nAdding New Staff Member:");
        Console.Write("Enter Staff Name: ");
        var name = Console.ReadLine();
        Console.Write("Enter Role (e.g., Teacher, Administrator): ");
        var role = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role))
        {
            Console.WriteLine("Name and Role cannot be empty.");
            return;
        }

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "INSERT INTO Staff (Name, Role) VALUES (@Name, @Role)";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Role", role);
                    var result = command.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Staff member added successfully." : "Failed to add staff member.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        Console.Clear();
        ListStaff();
    }
    static void ManageClasses()
    {
        bool showMenu = true;
        while (showMenu)
        {
            Console.Clear();
            Console.WriteLine("Class Management");
            Console.WriteLine("1. Add New Class");
            Console.WriteLine("2. View Classes");
            Console.WriteLine("3. Update Class");
            Console.WriteLine("4. Delete Class");
            Console.WriteLine("5. Return to Main Menu");
            Console.Write("\nSelect an option: ");
            var option = Console.ReadKey().Key;

            Console.WriteLine();

            switch (option)
            {
                case ConsoleKey.D1:
                    AddNewClassInteractive();
                    break;
                case ConsoleKey.D2:
                    ListClasses();
                    break;
                case ConsoleKey.D3:
                    UpdateClass();
                    break;
                case ConsoleKey.D4:
                    DeleteClass();
                    break;
                case ConsoleKey.D5:
                    showMenu = false;
                    break;
                default:
                    Console.WriteLine("\nInvalid option. Please try again.");
                    break;
            }

            if (showMenu)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    static bool ClassExists(string className)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var sql = "SELECT COUNT(1) FROM Classes WHERE ClassName = @ClassName";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ClassName", className);
                int exists = (int)command.ExecuteScalar();
                return exists > 0;
            }
        }
    }


    static bool ClassExists(int classId)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var sql = "SELECT COUNT(1) FROM Classes WHERE ClassID = @ClassID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ClassID", classId);
                int exists = (int)command.ExecuteScalar();
                return exists > 0;
            }
        }
    }
    static void ListClasses()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "SELECT ClassID, ClassName FROM Classes ORDER BY ClassName";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No classes found.");
                            return;
                        }
                        while (reader.Read())
                        {
                            Console.WriteLine($"Class ID: {reader["ClassID"]}, Name: {reader["ClassName"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to list classes: {ex.Message}");
        }
    }
    static void UpdateClass()
    {
        Console.Clear();
        ListClasses(); 
        Console.Write("\nEnter Class ID to update or 'exit' to return to the menu: ");
        var input = Console.ReadLine();
        if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operation cancelled.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        if (!int.TryParse(input, out int classId))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value for Class ID.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter new Class Name: ");
        string newClassName = Console.ReadLine().Trim();

        if (string.IsNullOrWhiteSpace(newClassName))
        {
            Console.WriteLine("Class name cannot be empty. Operation cancelled.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "UPDATE Classes SET ClassName = @NewClassName WHERE ClassID = @ClassID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ClassID", classId);
                    command.Parameters.AddWithValue("@NewClassName", newClassName);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Class updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Class not found or no changes made.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating the class: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
    static void DeleteClass()
    {
        Console.Clear();
        ListClasses();
        Console.Write("\nEnter Class ID to delete: ");

        if (!int.TryParse(Console.ReadLine(), out int classId))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value for Class ID.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Are you sure you want to delete this class? (yes/no)");
        string confirmation = Console.ReadLine().ToLower();
        if (confirmation != "yes")
        {
            Console.WriteLine("Class deletion canceled.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlCheck = "SELECT COUNT(*) FROM Students WHERE ClassID = @ClassID";
                using (SqlCommand commandCheck = new SqlCommand(sqlCheck, connection))
                {
                    commandCheck.Parameters.AddWithValue("@ClassID", classId);
                    int studentsInClass = (int)commandCheck.ExecuteScalar();
                    if (studentsInClass > 0)
                    {
                        Console.WriteLine("Cannot delete class because it has students enrolled.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return;
                    }
                }

                string sql = "DELETE FROM Classes WHERE ClassID = @ClassID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ClassID", classId);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Class deleted successfully." : "Class not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}. Make sure the class is not referenced elsewhere before deleting.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        Console.Clear();
        ListClasses();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    static void ManageCourses()
    {
        bool showMenu = true;
        while (showMenu)
        {
            Console.Clear();
            Console.WriteLine(" Course Management");
            Console.WriteLine("1. Add New Course ");
            Console.WriteLine("2. View Courses ");
            Console.WriteLine("3. Update Course ");
            Console.WriteLine("4. Delete Course ");
            Console.WriteLine("5. Return to Main Menu ");
            Console.Write("\nSelect an option: ");
            var option = Console.ReadKey();

            switch (option.Key)
            {
                case ConsoleKey.D1:
                    AddNewCourseInteractive();
                    break;
                case ConsoleKey.D2:
                    ListCourses();
                    break;
                case ConsoleKey.D3:
                    UpdateCourse();
                    break;
                case ConsoleKey.D4:
                    DeleteCourse();
                    break;
                case ConsoleKey.D5:
                    showMenu = false;
                    break;
                default:
                    Console.WriteLine("\nInvalid option. Please try again.");
                    break;
            }

            if (showMenu)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
    static bool CourseExists(string courseName)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var sql = "SELECT COUNT(1) FROM Courses WHERE CourseName = @CourseName";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CourseName", courseName);
                var exists = (int)command.ExecuteScalar();
                return exists > 0;
            }
        }
    }
    static void ListCourses()
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "SELECT CourseID, CourseName FROM Courses ORDER BY CourseName";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No courses found.");
                            return;
                        }
                        while (reader.Read())
                        {
                            Console.WriteLine($"CourseID: {reader["CourseID"]}, Course Name: {reader["CourseName"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve courses: {ex.Message}");
        }
    }
    static void UpdateCourse()
    {
        Console.Clear();
        ListCourses();
        Console.Write("\nEnter Course ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Invalid Course ID. Please enter a numeric value.");
            return;
        }
        Console.Write("Enter new Course Name: ");
        var newCourseName = Console.ReadLine().Trim();

        if (string.IsNullOrEmpty(newCourseName))
        {
            Console.WriteLine("Course name cannot be empty.");
            return;
        }

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE Courses SET CourseName = @NewCourseName WHERE CourseID = @CourseID";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseId);
                    command.Parameters.AddWithValue("@NewCourseName", newCourseName);
                    int result = command.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Course updated successfully." : "Course not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating the course: {ex.Message}");
        }
        Console.Clear();
        ListCourses();
    }
    static void DeleteCourse()
    {
        Console.Clear();
        ListCourses();
        Console.Write("\nEnter Course ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Invalid Course ID. Please enter a numeric value.");
            return;
        }

        var sql = "DELETE FROM Courses WHERE CourseID = @CourseID";
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseId);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        Console.WriteLine("\nCourse deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("\nCourse not found.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nAn error occurred: {ex.Message}. Make sure the course is not being referenced by any grades or students before deleting.");
        }
        Console.Clear();
        ListCourses();
    }

    static void ManageStudents()
    {
        bool showMenu = true;
        while (showMenu)
        {
            Console.Clear();
            Console.WriteLine("Student Management");
            Console.WriteLine("1. Add New Student");
            Console.WriteLine("2  List Students");
            Console.WriteLine("3. View Students asc/desc ");
            Console.WriteLine("4. Update Student ");
            Console.WriteLine("5. Delete Student ");
            Console.WriteLine("6. Add grade for Student");
            Console.WriteLine("7. Return to Main Menu");
            Console.Write("\nSelect an option: ");
            var option = Console.ReadKey();

            switch (option.Key)
            {
                case ConsoleKey.D1:
                    AddNewStudentInteractive();
                    break;
                case ConsoleKey.D2:
                    ListStudents();
                    break;
                case ConsoleKey.D3:
                    ViewAllStudents();
                    break;
                case ConsoleKey.D4:
                    UpdateStudent();
                    break;
                case ConsoleKey.D5:
                    DeleteStudent();
                    break;
                case ConsoleKey.D6:
                    AddGradeForStudent();
                    break;
                case ConsoleKey.D7:
                    showMenu = false;
                    break;
                default:
                    Console.WriteLine("\nInvalid option. Please try again.");
                    break;
            }


            if (showMenu)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
    static void ViewAllStudents()
    {
        Console.WriteLine("\nViewing All Students");
        Console.WriteLine("Choose sorting option:");
        Console.WriteLine("1. First Name Ascending");
        Console.WriteLine("2. First Name Descending");
        Console.WriteLine("3. Last Name Ascending");
        Console.WriteLine("4. Last Name Descending");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();
        string orderByClause = option switch
        {
            "1" => "ORDER BY FirstName ASC",
            "2" => "ORDER BY FirstName DESC",
            "3" => "ORDER BY LastName ASC",
            "4" => "ORDER BY LastName DESC",
            _ => "ORDER BY LastName ASC"
        };

        var sql = $"SELECT StudentID, FirstName, LastName FROM Students {orderByClause}";
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["StudentID"]}: {reader["FirstName"]} {reader["LastName"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    static void ListStudents()
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "SELECT StudentID, FirstName, LastName, BirthDate, ClassID FROM Students ORDER BY StudentID";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No students found.");
                            return;
                        }
                        while (reader.Read())
                        {
                            Console.WriteLine($"StudentID: {reader["StudentID"]}, Name: {reader["FirstName"]} {reader["LastName"]}, BirthDate: {reader["BirthDate"]:yyyy-MM-dd}, ClassID: {reader["ClassID"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    static void UpdateStudent()
    {
        Console.Clear();
        ListStudents();
        Console.WriteLine("\nEnter Student ID to update:");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Invalid input for Student ID.");
            return;
        }

        Console.Write("Enter new First Name (press Enter to skip): ");
        var firstName = Console.ReadLine();

        Console.Write("Enter new Last Name (press Enter to skip): ");
        var lastName = Console.ReadLine();

        Console.Write("Enter new Class ID (press Enter to skip): ");
        var classIdInput = Console.ReadLine();
        int? classId = null;
        if (!string.IsNullOrWhiteSpace(classIdInput) && int.TryParse(classIdInput, out int parsedClassId))
        {
            classId = parsedClassId;
        }

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = @"
                UPDATE Students 
                SET FirstName = ISNULL(NULLIF(@FirstName, ''), FirstName),
                    LastName = ISNULL(NULLIF(@LastName, ''), LastName)" +
                        (classId.HasValue ? ", ClassID = @ClassID" : "") +
                    " WHERE StudentID = @StudentID";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@FirstName", string.IsNullOrEmpty(firstName) ? (object)DBNull.Value : firstName);
                    command.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : lastName);
                    if (classId.HasValue)
                    {
                        command.Parameters.AddWithValue("@ClassID", classId.Value);
                    }

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Student updated successfully." : "Student not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        Console.Clear();
        ListStudents();
    }
    static void DeleteStudent()
    {
        Console.Clear();
        ListStudents();
        Console.Write("Enter Student ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Invalid Student ID.");
            return;
        }

        string sql = "DELETE FROM Students WHERE StudentID = @StudentID";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Student deleted successfully." : "Student not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}. Ensure the student is not referenced by other records before deleting.");
        }
        Console.Clear();
        ListStudents();
    }

    static bool StudentExists(int studentId)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var sql = "SELECT COUNT(1) FROM Students WHERE StudentID = @StudentID";
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@StudentID", studentId);
                var exists = (int)command.ExecuteScalar();
                return exists > 0;
            }
        }
    }

    static bool CourseExists(int courseId)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var sql = "SELECT COUNT(1) FROM Courses WHERE CourseID = @CourseID";
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@CourseID", courseId);
                var exists = (int)command.ExecuteScalar();
                return exists > 0;
            }
        }
    }

    static void ManageStaff()
    {
        string option = "";
        while (option != "0")
        {
            Console.Clear();
            Console.WriteLine(" Staff Management");
            Console.WriteLine("1. Add New Staff Member");
            Console.WriteLine("2. Update Staff Member");
            Console.WriteLine("3. List Staff Members");
            Console.WriteLine("4. Delete Staff Member");
            Console.WriteLine("0. Return to Main Menu");
            Console.Write("Select an option: ");
            option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddNewStaffMemberInteractive();
                    break;
                case "2":
                    UpdateStaffMember();
                    break;
                case "3":
                    ListStaff();
                    break;
                case "4":
                    DeleteStaffMember();
                    break;
            }
        }
    }
    static void ListStaff()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT StaffID, Name, Role FROM Staff ORDER BY Name";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No staff members found.");
                            return;
                        }
                        while (reader.Read())
                        {
                            Console.WriteLine($"StaffID: {reader["StaffID"]}, Name: {reader["Name"]}, Role: {reader["Role"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve staff members: {ex.Message}");
        }
    }
    static void UpdateStaffMember()
    {
        Console.Clear();
        ListStaff();
        Console.Write("\nEnter Staff ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int staffId))
        {
            Console.WriteLine("Invalid Staff ID.");
            return;
        }

        Console.Write("Enter new Name (leave blank to keep current): ");
        var newName = Console.ReadLine();
        Console.Write("Enter new Role (leave blank to keep current): ");
        var newRole = Console.ReadLine();

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE Staff SET Name = @NewName, Role = @NewRole WHERE StaffID = @StaffID";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StaffID", staffId);
                    command.Parameters.AddWithValue("@NewName", newName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NewRole", newRole ?? (object)DBNull.Value);

                    var result = command.ExecuteNonQuery();
                    Console.WriteLine(result > 0 ? "Staff updated successfully." : "Staff not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        Console.Clear();
        ListStaff();
    }
    static void DeleteStaffMember()
    {
        Console.Clear();
        ListStaff();
        Console.Write("\nEnter Staff ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int staffId))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value for Staff ID.");
            return;
        }

        Console.WriteLine("Are you sure you want to delete this staff member? (yes/no)");
        string confirmation = Console.ReadLine().ToLower();
        if (confirmation != "yes")
        {
            Console.WriteLine("Staff member deletion canceled.");
            return;
        }

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Staff WHERE StaffID = @StaffID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StaffID", staffId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Staff member deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Staff member not found or could not be deleted.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}. Ensure the staff member is not tied to other records before attempting deletion.");
        }
        Console.Clear();
        ListStaff();
    }
    static void ManageGrades()
    {
        string option = "";
        while (option != "0")
        {
            Console.Clear();
            Console.WriteLine(" Grade Management");
            Console.WriteLine("1. Add Grade for Student");
            Console.WriteLine("2. List Grades");
            Console.WriteLine("3. View Grades*in another way?");
            Console.WriteLine("4. Update Grade");
            Console.WriteLine("5. Delete Grade");
            Console.WriteLine("6. Return to Main Menu");
            Console.Write("Select an option: ");
            option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddGradeForStudent();
                    break;
                case "2":
                    ListGrades(); 
                    break;
                case "3":
                    ViewGradesByCourse();
                    break;
                case "4":
                    UpdateGrade();
                    break;
                case "5":
                    DeleteGrade();
                    break;
            }
        }
    }
    static void AddGradeForStudent()
    {
        Console.Clear();
        ListStudents();
        Console.WriteLine("Enter Student ID:");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value for Student ID.");
            return;
        }

        Console.WriteLine("Enter Course ID:");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value for Course ID.");
            return;
        }

        Console.WriteLine("Enter Grade (1-5):");
        if (!int.TryParse(Console.ReadLine(), out int grade) || grade < 1 || grade > 5)
        {
            Console.WriteLine("Invalid input. Please enter a grade between 1 and 5.");
            return;
        }

        string sql = "INSERT INTO Grades (StudentID, CourseID, GradeValue) VALUES (@StudentID, @CourseID, @GradeValue)";

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@CourseID", courseId);
                    command.Parameters.AddWithValue("@GradeValue", grade);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Grade added successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        Console.Clear();
        ListStudents();
    }
    static void ListGrades()
    {
        Console.Clear();
        Console.WriteLine("Fetching and listing all grades...");

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = @"
SELECT g.GradeID, s.FirstName, s.LastName, c.CourseName, g.GradeValue
FROM Grades g
JOIN Students s ON g.StudentID = s.StudentID
JOIN Courses c ON g.CourseID = c.CourseID
ORDER BY g.GradeID";

                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("\nNo grades found.");
                        }
                        else
                        {
                            Console.WriteLine("\nGrades List:\n");
                            Console.WriteLine("GradeID | Student Name       | Course Name       | Grade");
                            Console.WriteLine("--------|--------------------|-------------------|-------");
                            while (reader.Read())
                            {
                                // Assuming CourseName is added to your SQL SELECT for better readability
                                Console.WriteLine($"{reader["GradeID"],-8} | {reader["FirstName"]} {reader["LastName"],-18} | {reader["CourseName"],-18} | {reader["GradeValue"]}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nAn error occurred: {ex.Message}");
        }

        Console.WriteLine("\nEnd of grades list. Press any key to continue...");
        Console.ReadKey();
    }
    static void UpdateGrade()
    {
        Console.Clear();
        ListGrades();
        Console.WriteLine("\nUpdating Grade:");
        Console.Write("Enter Grade ID: ");
        if (!int.TryParse(Console.ReadLine(), out int gradeId) || !GradeExists(gradeId))
        {
            Console.WriteLine("Invalid or non-existing Grade ID.");
            return;
        }

        Console.Write("Enter new Grade (1-5): ");
        if (!int.TryParse(Console.ReadLine(), out int newGrade) || newGrade < 1 || newGrade > 5)
        {
            Console.WriteLine("Invalid grade. Please enter a value between 1 and 5.");
            return;
        }

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "UPDATE Grades SET GradeValue = @NewGrade WHERE GradeID = @GradeID";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GradeID", gradeId);
                    command.Parameters.AddWithValue("@NewGrade", newGrade);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Grade updated successfully." : "Grade not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating the grade: {ex.Message}");
        }
        ListGrades();
    }

    static bool GradeExists(int gradeId)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var sql = "SELECT COUNT(1) FROM Grades WHERE GradeID = @GradeID";
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@GradeID", gradeId);
                var exists = (int)command.ExecuteScalar();
                return exists > 0;
            }
        }
    }
    static void DeleteGrade()
    {
        Console.Clear();
        ListGrades();
        Console.WriteLine("\nDeleting Grade:");
        Console.Write("Enter Grade ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int gradeId) || !GradeExists(gradeId))
        {
            Console.WriteLine("Invalid or non-existing Grade ID.");
            return;
        }

        Console.WriteLine("Are you sure you want to delete this grade? (yes/no)");
        if (Console.ReadLine().Trim().ToLower() != "yes")
        {
            Console.WriteLine("Grade deletion canceled.");
            return;
        }

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM Grades WHERE GradeID = @GradeID";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GradeID", gradeId);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Grade deleted successfully." : "Grade not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the grade: {ex.Message}");
        }
        Console.Clear();
        ListGrades();
    }
    static void ViewGradesByCourse()
    {
        Console.Clear();
        Console.WriteLine("Enter Course ID to view grades for a specific course, or press Enter to view all grades:");
        string input = Console.ReadLine();
        int courseId;

        string sql = string.IsNullOrWhiteSpace(input) ?
            "SELECT s.FirstName, s.LastName, c.CourseName, g.GradeValue FROM Grades g JOIN Students s ON g.StudentID = s.StudentID JOIN Courses c ON g.CourseID = c.CourseID ORDER BY c.CourseName, s.LastName" :
            "SELECT s.FirstName, s.LastName, c.CourseName, g.GradeValue FROM Grades g JOIN Students s ON g.StudentID = s.StudentID JOIN Courses c ON g.CourseID = c.CourseID WHERE c.CourseID = @CourseID ORDER BY s.LastName";

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out courseId))
                    {
                        command.Parameters.AddWithValue("@CourseID", courseId);
                    }
                    else if (!string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Invalid Course ID. Showing all grades.");
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No grades found.");
                        }
                        else
                        {
                            Console.WriteLine($"Grades for Course ID {input}:\n");
                            Console.WriteLine("{0,-20} {1,-20} {2,-15} {3}", "Student First Name", "Student Last Name", "Course Name", "Grade");
                            Console.WriteLine(new string('-', 70));
                            while (reader.Read())
                            {
                                Console.WriteLine("{0,-20} {1,-20} {2,-15} {3}",
                                    reader["FirstName"],
                                    reader["LastName"],
                                    reader["CourseName"],
                                    reader["GradeValue"]);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve grades: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    static void CalculateAverageGradesPerCourse()
    {
        Console.Clear();
        Console.WriteLine("Calculating Average, Highest, and Lowest Grades Per Course...");
        var sql = @"
SELECT c.CourseName, 
       AVG(g.GradeValue) AS AverageGrade, 
       MAX(g.GradeValue) AS HighestGrade, 
       MIN(g.GradeValue) AS LowestGrade
FROM Grades g
JOIN Courses c ON g.CourseID = c.CourseID
GROUP BY c.CourseName";

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("\nNo grades available for calculation.");
                        }
                        else
                        {
                            Console.WriteLine("\nResults:");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["CourseName"]}: " +
                                                  $"Average Grade = {reader["AverageGrade"]}, " +
                                                  $"Highest Grade = {reader["HighestGrade"]}, " +
                                                  $"Lowest Grade = {reader["LowestGrade"]}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nFailed to calculate average grades: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    static void CreateNewSystem()
    {
        Console.Clear();
        Console.WriteLine("Initializing the new system. This will create the necessary tables in the database.");
        Console.Write("Proceed? (Y/N): ");
        string proceed = Console.ReadLine();
        if (proceed.Trim().ToUpper() != "Y")
        {
            Console.WriteLine("Initialization canceled.");
        }
        else
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var commands = new[]
                    {
                    "IF OBJECT_ID('Grades', 'U') IS NOT NULL DROP TABLE Grades;",
                    "IF OBJECT_ID('Students', 'U') IS NOT NULL DROP TABLE Students;",
                    "IF OBJECT_ID('Courses', 'U') IS NOT NULL DROP TABLE Courses;",
                    "IF OBJECT_ID('Classes', 'U') IS NOT NULL DROP TABLE Classes;",
                    "IF OBJECT_ID('Staff', 'U') IS NOT NULL DROP TABLE Staff;",
                    "CREATE TABLE Classes (ClassID INT IDENTITY(1,1) PRIMARY KEY, ClassName NVARCHAR(100) NOT NULL);",
                    "CREATE TABLE Courses (CourseID INT IDENTITY(1,1) PRIMARY KEY, CourseName NVARCHAR(100) NOT NULL);",
                    @"CREATE TABLE Students (StudentID INT IDENTITY(1,1) PRIMARY KEY, FirstName NVARCHAR(100) NOT NULL, LastName NVARCHAR(100) NOT NULL, BirthDate DATE NOT NULL, ClassID INT, FOREIGN KEY (ClassID) REFERENCES Classes(ClassID));",
                    "CREATE TABLE Staff (StaffID INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(100) NOT NULL, Role NVARCHAR(100) NOT NULL);",
                    @"CREATE TABLE Grades (GradeID INT IDENTITY(1,1) PRIMARY KEY, StudentID INT NOT NULL, CourseID INT NOT NULL, GradeValue INT CHECK (GradeValue BETWEEN 1 AND 5), FOREIGN KEY (StudentID) REFERENCES Students(StudentID), FOREIGN KEY (CourseID) REFERENCES Courses(CourseID));"
                };

                    foreach (var cmdText in commands)
                    {
                        using (var command = new SqlCommand(cmdText, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("Database initialized successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Initialization failed: {ex.Message}");
            }
        }

        Console.WriteLine("Press any key to continue ...");
        Console.ReadKey();
    }
    static void ResetSystem()
    {
        Console.Clear();
        Console.WriteLine("WARNING: This will delete all data from the system. Proceed? (Y/N): ");
        string proceed = Console.ReadLine();
        if (proceed.Trim().ToUpper() != "Y")
        {
            Console.WriteLine("Reset canceled.");
        }
        else
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var tables = new[] { "Grades", "Students", "Courses", "Classes", "Staff" };
                    foreach (var table in tables)
                    {
                        var cmdText = $"DELETE FROM {table}; DBCC CHECKIDENT ('{table}', RESEED, 0);";
                        using (var command = new SqlCommand(cmdText, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("System reset successfully. All data has been deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reset failed: {ex.Message}");
            }
        }
        Console.WriteLine("Press any key to continue ...");
        Console.ReadKey();
    }
    static void DisplayRecommendedDatabaseSchema()
    {
        Console.Clear();
        Console.WriteLine("Recommended Database Schema:");
        Console.WriteLine(@"
CREATE TABLE Classes (
    ClassID INT IDENTITY(1,1) PRIMARY KEY,
    ClassName NVARCHAR(100) NOT NULL
);

CREATE TABLE Courses (
    CourseID INT IDENTITY(1,1) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL
);

CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL,
    ClassID INT,
    FOREIGN KEY (ClassID) REFERENCES Classes(ClassID)
);

CREATE TABLE Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Role NVARCHAR(100) NOT NULL
);

CREATE TABLE Grades (
    GradeID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    CourseID INT NOT NULL,
    GradeValue INT CHECK (GradeValue BETWEEN 1 AND 5),
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);
");

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    static void CheckDatabaseConnection()
    {
        Console.Clear();
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Database connection successful.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection failed: {ex.Message}");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
