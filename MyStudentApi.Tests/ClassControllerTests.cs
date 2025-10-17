using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStudentApi.Controllers;
using MyStudentApi.Data;
using MyStudentApi.Models;
using FluentAssertions;
using Xunit;

namespace MyStudentApi.Tests
{
    public class ClassControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        #region GetSubjects Tests

        [Fact]
        public async Task GetSubjects_WithTerm2254_ReturnsDistinctSubjects()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.ClassSchedule2254.AddRange(
                new ClassSchedule2254
                {
                    ClassNum = "12345",
                    Term = "2254",
                    Session = "C",
                    Subject = "CSE",
                    CatalogNum = 110,
                    SectionNum = 1,
                    Title = "Principles of Programming",
                    InstructorFirstName = "John",
                    InstructorLastName = "Doe",
                    InstructorEmail = "jdoe@asu.edu",
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD"
                },
                new ClassSchedule2254
                {
                    ClassNum = "12346",
                    Term = "2254",
                    Session = "C",
                    Subject = "CSE",
                    CatalogNum = 240,
                    SectionNum = 1,
                    Title = "Data Structures",
                    InstructorFirstName = "Jane",
                    InstructorLastName = "Smith",
                    InstructorEmail = "jsmith@asu.edu",
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD"
                },
                new ClassSchedule2254
                {
                    ClassNum = "12347",
                    Term = "2254",
                    Session = "C",
                    Subject = "IEE",
                    CatalogNum = 380,
                    SectionNum = 1,
                    Title = "Probability and Statistics",
                    InstructorFirstName = "Bob",
                    InstructorLastName = "Johnson",
                    InstructorEmail = "bjohnson@asu.edu",
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD"
                }
            );
            await context.SaveChangesAsync();

            var controller = new ClassController(context);

            // Act
            var result = await controller.GetSubjects("2254");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var subjects = okResult.Value as List<string>;
            subjects.Should().NotBeNull();
            subjects.Should().HaveCount(2);
            subjects.Should().Contain("CSE");
            subjects.Should().Contain("IEE");
        }

        [Fact]
        public async Task GetSubjects_WithTerm2251_ReturnsDistinctSubjects()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.ClassLookups.AddRange(
                new ClassLookup
                {
                    ClassNum = "11111",
                    Term = "2251",
                    Session = "A",
                    Subject = "CIS",
                    CatalogNum = 210,
                    SectionNum = 1,
                    Component = "LEC",
                    Title = "Systems Analysis",
                    InstructorID = 1,
                    InstructorFirstName = "Alice",
                    InstructorLastName = "Williams",
                    InstructorEmail = "awilliams@asu.edu",
                    Location = "TEMPE"
                },
                new ClassLookup
                {
                    ClassNum = "11112",
                    Term = "2251",
                    Session = "A",
                    Subject = "CSE",
                    CatalogNum = 310,
                    SectionNum = 1,
                    Component = "LEC",
                    Title = "Algorithms",
                    InstructorID = 2,
                    InstructorFirstName = "Charlie",
                    InstructorLastName = "Brown",
                    InstructorEmail = "cbrown@asu.edu",
                    Location = "TEMPE"
                }
            );
            await context.SaveChangesAsync();

            var controller = new ClassController(context);

            // Act
            var result = await controller.GetSubjects("2251");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var subjects = okResult.Value as List<string>;
            subjects.Should().NotBeNull();
            subjects.Should().HaveCount(2);
            subjects.Should().Contain("CIS");
            subjects.Should().Contain("CSE");
        }

        [Fact]
        public async Task GetSubjects_WithEmptyTerm_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ClassController(context);

            // Act
            var result = await controller.GetSubjects("");

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("Term is required.");
        }

        [Fact]
        public async Task GetSubjects_WithInvalidTerm_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ClassController(context);

            // Act
            var result = await controller.GetSubjects("9999");

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("Invalid term value.");
        }

        #endregion

        #region GetCatalogNumbers Tests

        [Fact]
        public async Task GetCatalogNumbers_WithValidTermAndSubject_ReturnsDistinctCatalogNumbers()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.ClassSchedule2254.AddRange(
                new ClassSchedule2254
                {
                    ClassNum = "12345",
                    Term = "2254",
                    Session = "C",
                    Subject = "CSE",
                    CatalogNum = 110,
                    SectionNum = 1,
                    Title = "Principles of Programming",
                    InstructorFirstName = "John",
                    InstructorLastName = "Doe",
                    InstructorEmail = "jdoe@asu.edu",
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD"
                },
                new ClassSchedule2254
                {
                    ClassNum = "12346",
                    Term = "2254",
                    Session = "C",
                    Subject = "CSE",
                    CatalogNum = 240,
                    SectionNum = 1,
                    Title = "Data Structures",
                    InstructorFirstName = "Jane",
                    InstructorLastName = "Smith",
                    InstructorEmail = "jsmith@asu.edu",
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD"
                }
            );
            await context.SaveChangesAsync();

            var controller = new ClassController(context);

            // Act
            var result = await controller.GetCatalogNumbers("2254", "CSE");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var catalogs = okResult.Value as List<string>;
            catalogs.Should().NotBeNull();
            catalogs.Should().HaveCount(2);
            catalogs.Should().Contain("110");
            catalogs.Should().Contain("240");
        }

        [Fact]
        public async Task GetCatalogNumbers_WithMissingTerm_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ClassController(context);

            // Act
            var result = await controller.GetCatalogNumbers("", "CSE");

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("Term and Subject are required.");
        }

        [Fact]
        public async Task GetCatalogNumbers_WithMissingSubject_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ClassController(context);

            // Act
            var result = await controller.GetCatalogNumbers("2254", "");

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("Term and Subject are required.");
        }

        #endregion

        #region GetClassNumbers Tests

        [Fact]
        public async Task GetClassNumbers_WithValidParameters_ReturnsDistinctClassNumbers()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.ClassSchedule2254.AddRange(
                new ClassSchedule2254
                {
                    ClassNum = "12345",
                    Term = "2254",
                    Session = "C",
                    Subject = "CSE",
                    CatalogNum = 110,
                    SectionNum = 1,
                    Title = "Principles of Programming",
                    InstructorFirstName = "John",
                    InstructorLastName = "Doe",
                    InstructorEmail = "jdoe@asu.edu",
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD"
                },
                new ClassSchedule2254
                {
                    ClassNum = "12346",
                    Term = "2254",
                    Session = "C",
                    Subject = "CSE",
                    CatalogNum = 110,
                    SectionNum = 2,
                    Title = "Principles of Programming",
                    InstructorFirstName = "Jane",
                    InstructorLastName = "Smith",
                    InstructorEmail = "jsmith@asu.edu",
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD"
                }
            );
            await context.SaveChangesAsync();

            var controller = new ClassController(context);

            // Act
            var result = await controller.GetClassNumbers("2254", "CSE", "110");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var classNumbers = okResult.Value as List<string>;
            classNumbers.Should().NotBeNull();
            classNumbers.Should().HaveCount(2);
            classNumbers.Should().Contain("12345");
            classNumbers.Should().Contain("12346");
        }

        [Fact]
        public async Task GetClassNumbers_WithInvalidCatalogNum_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ClassController(context);

            // Act
            var result = await controller.GetClassNumbers("2254", "CSE", "abc");

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("CatalogNum must be numeric.");
        }

        [Fact]
        public async Task GetClassNumbers_WithMissingParameters_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ClassController(context);

            // Act
            var result = await controller.GetClassNumbers("", "CSE", "110");

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("Term, Subject, and CatalogNum are required.");
        }

        #endregion

        #region GetClassDetails Tests

        [Fact]
        public async Task GetClassDetails_WithValidClassNum_ReturnsClassDetails()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.ClassSchedule2254.Add(
                new ClassSchedule2254
                {
                    ClassNum = "12345",
                    Term = "2254",
                    Session = "C",
                    Subject = "CSE",
                    CatalogNum = 110,
                    SectionNum = 1,
                    Title = "Principles of Programming",
                    InstructorID = 123,
                    InstructorFirstName = "John",
                    InstructorLastName = "Doe",
                    InstructorEmail = "jdoe@asu.edu",
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD"
                }
            );
            await context.SaveChangesAsync();

            var controller = new ClassController(context);

            // Act
            var result = await controller.GetClassDetails("12345", "2254");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().NotBeNull();

            // Use reflection to check anonymous type properties
            var classDetails = okResult.Value;
            var sessionProp = classDetails.GetType().GetProperty("Session");
            sessionProp.GetValue(classDetails).Should().Be("C");

            var instructorFirstNameProp = classDetails.GetType().GetProperty("InstructorFirstName");
            instructorFirstNameProp.GetValue(classDetails).Should().Be("John");
        }

        [Fact]
        public async Task GetClassDetails_WithInvalidClassNum_ReturnsNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ClassController(context);

            // Act
            var result = await controller.GetClassDetails("99999", "2254");

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetClassDetails_WithEmptyClassNum_ReturnsBadRequest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new ClassController(context);

            // Act
            var result = await controller.GetClassDetails("", "2254");

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("ClassNum and Term are required.");
        }

        [Fact]
        public async Task GetClassDetails_WithTerm2251_ReturnsClassDetailsFromClassLookups()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.ClassLookups.Add(
                new ClassLookup
                {
                    ClassNum = "11111",
                    Term = "2251",
                    Session = "A",
                    Subject = "CSE",
                    CatalogNum = 310,
                    SectionNum = 1,
                    Component = "LEC",
                    Title = "Algorithms",
                    InstructorID = 456,
                    InstructorFirstName = "Alice",
                    InstructorLastName = "Johnson",
                    InstructorEmail = "ajohnson@asu.edu",
                    Location = "TEMPE"
                }
            );
            await context.SaveChangesAsync();

            var controller = new ClassController(context);

            // Act
            var result = await controller.GetClassDetails("11111", "2251");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().NotBeNull();

            var classDetails = okResult.Value;
            var instructorEmailProp = classDetails.GetType().GetProperty("InstructorEmail");
            instructorEmailProp.GetValue(classDetails).Should().Be("ajohnson@asu.edu");
        }

        #endregion
    }
}
