using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStudentApi.Controllers;
using MyStudentApi.Data;
using MyStudentApi.Models;
using MyStudentApi.DTO;
using FluentAssertions;
using Xunit;
using YourNamespace.Models;

namespace MyStudentApi.Tests
{
    public class StudentClassAssignmentControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        #region GetAssignments Tests

        [Fact]
        public async Task GetAssignments_ReturnsAllAssignments()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.StudentClassAssignments.AddRange(
                new StudentClassAssignment
                {
                    Id = 1,
                    Student_ID = 12345,
                    ASUrite = "jdoe1",
                    Position = "IA",
                    WeeklyHours = 10,
                    FultonFellow = "No",
                    Email = "jdoe1@asu.edu",
                    EducationLevel = "MS",
                    Subject = "CSE",
                    CatalogNum = 110,
                    ClassSession = "C",
                    ClassNum = "12345",
                    Term = "2254",
                    InstructorFirstName = "John",
                    InstructorLastName = "Smith",
                    Compensation = 2640.0,
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD",
                    CostCenterKey = "TEMPE-TEMPE-UGRD-IA"
                },
                new StudentClassAssignment
                {
                    Id = 2,
                    Student_ID = 67890,
                    ASUrite = "jsmith2",
                    Position = "TA",
                    WeeklyHours = 20,
                    FultonFellow = "Yes",
                    Email = "jsmith2@asu.edu",
                    EducationLevel = "PHD",
                    Subject = "IEE",
                    CatalogNum = 380,
                    ClassSession = "C",
                    ClassNum = "12346",
                    Term = "2254",
                    InstructorFirstName = "Jane",
                    InstructorLastName = "Doe",
                    Compensation = 5280.0,
                    Location = "TEMPE",
                    Campus = "TEMPE",
                    AcadCareer = "UGRD",
                    CostCenterKey = "TEMPE-TEMPE-UGRD-TA"
                }
            );
            await context.SaveChangesAsync();

            var controller = new StudentClassAssignmentController(context);

            // Act
            var result = await controller.GetAssignments();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var assignments = okResult.Value as List<StudentClassAssignment>;
            assignments.Should().NotBeNull();
            assignments.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAssignments_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new StudentClassAssignmentController(context);

            // Act
            var result = await controller.GetAssignments();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var assignments = okResult.Value as List<StudentClassAssignment>;
            assignments.Should().NotBeNull();
            assignments.Should().BeEmpty();
        }

        #endregion

        #region GetAssignment Tests

        [Fact]
        public async Task GetAssignment_WithValidId_ReturnsAssignment()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var assignment = new StudentClassAssignment
            {
                Id = 1,
                Student_ID = 12345,
                ASUrite = "jdoe1",
                Position = "IA",
                WeeklyHours = 10,
                FultonFellow = "No",
                Email = "jdoe1@asu.edu",
                EducationLevel = "MS",
                Subject = "CSE",
                CatalogNum = 110,
                ClassSession = "C",
                ClassNum = "12345",
                Term = "2254",
                InstructorFirstName = "John",
                InstructorLastName = "Smith",
                Compensation = 2640.0,
                Location = "TEMPE",
                Campus = "TEMPE",
                AcadCareer = "UGRD",
                CostCenterKey = "TEMPE-TEMPE-UGRD-IA"
            };
            context.StudentClassAssignments.Add(assignment);
            await context.SaveChangesAsync();

            var controller = new StudentClassAssignmentController(context);

            // Act
            var result = await controller.GetAssignment(1);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var returnedAssignment = okResult.Value as StudentClassAssignment;
            returnedAssignment.Should().NotBeNull();
            returnedAssignment.Id.Should().Be(1);
            returnedAssignment.ASUrite.Should().Be("jdoe1");
            returnedAssignment.Position.Should().Be("IA");
        }

        [Fact]
        public async Task GetAssignment_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new StudentClassAssignmentController(context);

            // Act
            var result = await controller.GetAssignment(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region GetTotalAssignedHours Tests

        [Fact]
        public async Task GetTotalAssignedHours_WithValidStudentId_ReturnsTotalHours()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.StudentClassAssignments.AddRange(
                new StudentClassAssignment
                {
                    Id = 1,
                    Student_ID = 12345,
                    ASUrite = "jdoe1",
                    Position = "IA",
                    WeeklyHours = 10,
                    FultonFellow = "No",
                    Email = "jdoe1@asu.edu",
                    EducationLevel = "MS",
                    Subject = "CSE",
                    CatalogNum = 110,
                    ClassSession = "C",
                    ClassNum = "12345",
                    Term = "2254",
                    Compensation = 2640.0
                },
                new StudentClassAssignment
                {
                    Id = 2,
                    Student_ID = 12345,
                    ASUrite = "jdoe1",
                    Position = "TA",
                    WeeklyHours = 15,
                    FultonFellow = "No",
                    Email = "jdoe1@asu.edu",
                    EducationLevel = "MS",
                    Subject = "IEE",
                    CatalogNum = 380,
                    ClassSession = "C",
                    ClassNum = "12346",
                    Term = "2254",
                    Compensation = 3960.0
                }
            );
            await context.SaveChangesAsync();

            var controller = new StudentClassAssignmentController(context);

            // Act
            var result = await controller.GetTotalAssignedHours(12345);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var totalHours = (int)okResult.Value;
            totalHours.Should().Be(25);
        }

        [Fact]
        public async Task GetTotalAssignedHours_WithNoAssignments_ReturnsZero()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new StudentClassAssignmentController(context);

            // Act
            var result = await controller.GetTotalAssignedHours(99999);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var totalHours = (int)okResult.Value;
            totalHours.Should().Be(0);
        }

        [Fact]
        public async Task GetTotalAssignedHours_WithMultipleStudents_ReturnsOnlySpecificStudentHours()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.StudentClassAssignments.AddRange(
                new StudentClassAssignment
                {
                    Id = 1,
                    Student_ID = 12345,
                    WeeklyHours = 10,
                    ASUrite = "jdoe1",
                    Position = "IA",
                    Email = "jdoe1@asu.edu",
                    CatalogNum = 110,
                    Term = "2254"
                },
                new StudentClassAssignment
                {
                    Id = 2,
                    Student_ID = 67890,
                    WeeklyHours = 20,
                    ASUrite = "jsmith2",
                    Position = "TA",
                    Email = "jsmith2@asu.edu",
                    CatalogNum = 210,
                    Term = "2254"
                },
                new StudentClassAssignment
                {
                    Id = 3,
                    Student_ID = 12345,
                    WeeklyHours = 5,
                    ASUrite = "jdoe1",
                    Position = "Grader",
                    Email = "jdoe1@asu.edu",
                    CatalogNum = 310,
                    Term = "2254"
                }
            );
            await context.SaveChangesAsync();

            var controller = new StudentClassAssignmentController(context);

            // Act
            var result = await controller.GetTotalAssignedHours(12345);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var totalHours = (int)okResult.Value;
            totalHours.Should().Be(15);
        }

        #endregion

        #region CreateAssignment Tests

        [Fact]
        public async Task CreateAssignment_WithValidAssignment_ReturnsCreatedAtAction()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new StudentClassAssignmentController(context);
            var newAssignment = new StudentClassAssignment
            {
                Student_ID = 12345,
                ASUrite = "jdoe1",
                Position = "IA",
                WeeklyHours = 10,
                FultonFellow = "No",
                Email = "jdoe1@asu.edu",
                EducationLevel = "MS",
                Subject = "CSE",
                CatalogNum = 110,
                ClassSession = "C",
                ClassNum = "12345",
                Term = "2254",
                InstructorFirstName = "John",
                InstructorLastName = "Smith",
                Compensation = 2640.0,
                Location = "TEMPE",
                Campus = "TEMPE",
                AcadCareer = "UGRD",
                CostCenterKey = "TEMPE-TEMPE-UGRD-IA"
            };

            // Act
            var result = await controller.CreateAssignment(newAssignment);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(controller.GetAssignment));
            var returnedAssignment = createdResult.Value as StudentClassAssignment;
            returnedAssignment.Should().NotBeNull();
            returnedAssignment.ASUrite.Should().Be("jdoe1");
            returnedAssignment.Position.Should().Be("IA");

            // Verify it was saved to the database
            var savedAssignment = await context.StudentClassAssignments.FirstOrDefaultAsync();
            savedAssignment.Should().NotBeNull();
            savedAssignment.ASUrite.Should().Be("jdoe1");
        }

        #endregion

        #region UpdateAssignment Tests

        [Fact]
        public async Task UpdateAssignment_WithValidId_UpdatesFieldsAndReturnsNoContent()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var assignment = new StudentClassAssignment
            {
                Id = 1,
                Student_ID = 12345,
                ASUrite = "jdoe1",
                Position = "IA",
                WeeklyHours = 10,
                FultonFellow = "No",
                Email = "jdoe1@asu.edu",
                EducationLevel = "MS",
                Subject = "CSE",
                CatalogNum = 110,
                ClassSession = "C",
                ClassNum = "12345",
                Term = "2254",
                Compensation = 2640.0,
                Position_Number = null,
                I9_Sent = false,
                SSN_Sent = false,
                Offer_Sent = false,
                Offer_Signed = false
            };
            context.StudentClassAssignments.Add(assignment);
            await context.SaveChangesAsync();

            var controller = new StudentClassAssignmentController(context);
            var updateDto = new StudentAssignmentUpdateDto
            {
                Position_Number = "POS123",
                I9_Sent = true,
                SSN_Sent = true,
                Offer_Sent = true,
                Offer_Signed = false
            };

            // Act
            var result = await controller.UpdateAssignment(1, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            // Verify the update was applied
            var updatedAssignment = await context.StudentClassAssignments.FindAsync(1);
            updatedAssignment.Position_Number.Should().Be("POS123");
            updatedAssignment.I9_Sent.Should().BeTrue();
            updatedAssignment.SSN_Sent.Should().BeTrue();
            updatedAssignment.Offer_Sent.Should().BeTrue();
            updatedAssignment.Offer_Signed.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAssignment_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new StudentClassAssignmentController(context);
            var updateDto = new StudentAssignmentUpdateDto
            {
                Position_Number = "POS123",
                I9_Sent = true
            };

            // Act
            var result = await controller.UpdateAssignment(999, updateDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateAssignment_OnlyUpdatesSpecifiedFields()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var assignment = new StudentClassAssignment
            {
                Id = 1,
                Student_ID = 12345,
                ASUrite = "jdoe1",
                Position = "IA",
                WeeklyHours = 10,
                FultonFellow = "No",
                Email = "jdoe1@asu.edu",
                EducationLevel = "MS",
                Subject = "CSE",
                CatalogNum = 110,
                ClassSession = "C",
                ClassNum = "12345",
                Term = "2254",
                Compensation = 2640.0
            };
            context.StudentClassAssignments.Add(assignment);
            await context.SaveChangesAsync();

            var controller = new StudentClassAssignmentController(context);
            var updateDto = new StudentAssignmentUpdateDto
            {
                Position_Number = "POS456"
            };

            // Act
            await controller.UpdateAssignment(1, updateDto);

            // Assert
            var updatedAssignment = await context.StudentClassAssignments.FindAsync(1);
            // These fields should remain unchanged
            updatedAssignment.ASUrite.Should().Be("jdoe1");
            updatedAssignment.Position.Should().Be("IA");
            updatedAssignment.WeeklyHours.Should().Be(10);
            updatedAssignment.Subject.Should().Be("CSE");
            // Only Position_Number should be updated
            updatedAssignment.Position_Number.Should().Be("POS456");
        }

        #endregion

        #region DownloadTemplate Tests

        [Fact]
        public void DownloadTemplate_ReturnsCsvFileResult()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new StudentClassAssignmentController(context);

            // Act
            var result = controller.DownloadTemplate();

            // Assert
            result.Should().BeOfType<FileContentResult>();
            var fileResult = result as FileContentResult;
            fileResult.ContentType.Should().Be("text/csv");
            fileResult.FileDownloadName.Should().Be("BulkUploadTemplate.csv");

            // Verify content contains headers
            var content = System.Text.Encoding.UTF8.GetString(fileResult.FileContents);
            content.Should().Contain("Position");
            content.Should().Contain("FultonFellow");
            content.Should().Contain("WeeklyHours");
            content.Should().Contain("Student_ID");
        }

        #endregion
    }
}
