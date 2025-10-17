using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStudentApi.Controllers;
using MyStudentApi.Data;
using YourNamespace.Models;
using FluentAssertions;
using Xunit;

namespace MyStudentApi.Tests
{
    public class StudentLookupControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetStudentByIdOrAsurite_WithValidStudentId_ReturnsOkResultWithStudent()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var student = new StudentLookup
            {
                Student_ID = 12345,
                ASUrite = "jdoe1",
                First_Name = "John",
                Last_Name = "Doe",
                ASU_Email_Adress = "jdoe1@asu.edu",
                Cumulative_GPA = 3.5,
                Current_GPA = 3.6,
                Acad_Prog = "ESCSEBS",
                Acad_Prog_Descr = "Computer Science",
                Acad_Career = "UGRD",
                Acad_Group = "ENGR",
                Acad_Org = "FSE",
                Acad_Plan = "CSEBS",
                Plan_Descr = "Computer Science (BS)",
                Degree = "BS",
                Transcript_Description = "Bachelor of Science in Computer Science",
                Plan_Type = "Major",
                Acad_Lvl_BOT = "Junior",
                Acad_Lvl_EOT = "Senior",
                Prog_Status = "Active",
                Campus = "TEMPE",
                Deans_List = "Yes"
            };
            context.StudentLookups.Add(student);
            await context.SaveChangesAsync();

            var controller = new StudentLookupController(context);

            // Act
            var result = await controller.GetStudentByIdOrAsurite("12345");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var returnedStudent = okResult.Value as StudentLookup;
            returnedStudent.Should().NotBeNull();
            returnedStudent.Student_ID.Should().Be(12345);
            returnedStudent.ASUrite.Should().Be("jdoe1");
            returnedStudent.First_Name.Should().Be("John");
        }

        [Fact]
        public async Task GetStudentByIdOrAsurite_WithValidAsurite_ReturnsOkResultWithStudent()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var student = new StudentLookup
            {
                Student_ID = 67890,
                ASUrite = "jsmith2",
                First_Name = "Jane",
                Last_Name = "Smith",
                ASU_Email_Adress = "jsmith2@asu.edu",
                Cumulative_GPA = 3.8,
                Current_GPA = 3.9,
                Acad_Prog = "ESCSEBS",
                Acad_Prog_Descr = "Computer Science",
                Acad_Career = "UGRD",
                Acad_Group = "ENGR",
                Acad_Org = "FSE",
                Acad_Plan = "CSEBS",
                Plan_Descr = "Computer Science (BS)",
                Degree = "BS",
                Transcript_Description = "Bachelor of Science in Computer Science",
                Plan_Type = "Major",
                Acad_Lvl_BOT = "Senior",
                Acad_Lvl_EOT = "Senior",
                Prog_Status = "Active",
                Campus = "TEMPE",
                Deans_List = "Yes"
            };
            context.StudentLookups.Add(student);
            await context.SaveChangesAsync();

            var controller = new StudentLookupController(context);

            // Act
            var result = await controller.GetStudentByIdOrAsurite("jsmith2");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var returnedStudent = okResult.Value as StudentLookup;
            returnedStudent.Should().NotBeNull();
            returnedStudent.Student_ID.Should().Be(67890);
            returnedStudent.ASUrite.Should().Be("jsmith2");
            returnedStudent.First_Name.Should().Be("Jane");
        }

        [Fact]
        public async Task GetStudentByIdOrAsurite_WithAsuriteCaseInsensitive_ReturnsOkResultWithStudent()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var student = new StudentLookup
            {
                Student_ID = 11111,
                ASUrite = "TestUser",
                First_Name = "Test",
                Last_Name = "User",
                ASU_Email_Adress = "testuser@asu.edu",
                Cumulative_GPA = 3.0,
                Current_GPA = 3.1,
                Acad_Prog = "ESCSEBS",
                Acad_Prog_Descr = "Computer Science",
                Acad_Career = "UGRD",
                Acad_Group = "ENGR",
                Acad_Org = "FSE",
                Acad_Plan = "CSEBS",
                Plan_Descr = "Computer Science (BS)",
                Degree = "BS",
                Transcript_Description = "Bachelor of Science in Computer Science",
                Plan_Type = "Major",
                Acad_Lvl_BOT = "Sophomore",
                Acad_Lvl_EOT = "Junior",
                Prog_Status = "Active",
                Campus = "TEMPE",
                Deans_List = "No"
            };
            context.StudentLookups.Add(student);
            await context.SaveChangesAsync();

            var controller = new StudentLookupController(context);

            // Act - searching with lowercase
            var result = await controller.GetStudentByIdOrAsurite("testuser");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var returnedStudent = okResult.Value as StudentLookup;
            returnedStudent.Should().NotBeNull();
            returnedStudent.ASUrite.Should().Be("TestUser");
        }

        [Fact]
        public async Task GetStudentByIdOrAsurite_WithInvalidStudentId_ReturnsNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new StudentLookupController(context);

            // Act
            var result = await controller.GetStudentByIdOrAsurite("99999");

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("No student found with identifier: 99999");
        }

        [Fact]
        public async Task GetStudentByIdOrAsurite_WithInvalidAsurite_ReturnsNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new StudentLookupController(context);

            // Act
            var result = await controller.GetStudentByIdOrAsurite("nonexistent");

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("No student found with identifier: nonexistent");
        }

        [Fact]
        public async Task GetStudentByIdOrAsurite_WithMultipleStudents_ReturnsCorrectStudent()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var student1 = new StudentLookup
            {
                Student_ID = 11111,
                ASUrite = "student1",
                First_Name = "Student",
                Last_Name = "One",
                ASU_Email_Adress = "student1@asu.edu",
                Cumulative_GPA = 3.0,
                Current_GPA = 3.0,
                Acad_Prog = "PROG1",
                Acad_Prog_Descr = "Program 1",
                Acad_Career = "UGRD",
                Acad_Group = "GRP1",
                Acad_Org = "ORG1",
                Acad_Plan = "PLAN1",
                Plan_Descr = "Plan 1",
                Degree = "BS",
                Transcript_Description = "Bachelor of Science",
                Plan_Type = "Major",
                Acad_Lvl_BOT = "Junior",
                Acad_Lvl_EOT = "Senior",
                Prog_Status = "Active",
                Campus = "TEMPE",
                Deans_List = "No"
            };

            var student2 = new StudentLookup
            {
                Student_ID = 22222,
                ASUrite = "student2",
                First_Name = "Student",
                Last_Name = "Two",
                ASU_Email_Adress = "student2@asu.edu",
                Cumulative_GPA = 3.5,
                Current_GPA = 3.6,
                Acad_Prog = "PROG2",
                Acad_Prog_Descr = "Program 2",
                Acad_Career = "GRAD",
                Acad_Group = "GRP2",
                Acad_Org = "ORG2",
                Acad_Plan = "PLAN2",
                Plan_Descr = "Plan 2",
                Degree = "MS",
                Transcript_Description = "Master of Science",
                Plan_Type = "Major",
                Acad_Lvl_BOT = "Graduate",
                Acad_Lvl_EOT = "Graduate",
                Prog_Status = "Active",
                Campus = "TEMPE",
                Deans_List = "Yes"
            };

            context.StudentLookups.AddRange(student1, student2);
            await context.SaveChangesAsync();

            var controller = new StudentLookupController(context);

            // Act
            var result = await controller.GetStudentByIdOrAsurite("22222");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var returnedStudent = okResult.Value as StudentLookup;
            returnedStudent.Should().NotBeNull();
            returnedStudent.Student_ID.Should().Be(22222);
            returnedStudent.ASUrite.Should().Be("student2");
        }
    }
}
