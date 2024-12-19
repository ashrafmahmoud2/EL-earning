using System.Linq.Expressions;
using E_Learning.Tests;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Students;
using ELearning.Data.Entities;
using ELearning.Data.Errors;
using ELearning.Infrastructure.Base;
using ELearning.Service.IService;
using ELearning.Service.Service;
using FakeItEasy;
using MapsterMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Hybrid;

public class StudentServiceTests
{

    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly HybridCache _mockHybridCache;
    private readonly ICacheService _mockCacheService;
    private readonly IGenericRepository<Student> _StudentRepository;
    private readonly IGenericRepository<Lesson> _lessonRepository;
    private readonly IGenericRepository<ApplicationUser> _userRepository;
    private readonly StudentService _sut;
    private readonly IMapper _mapper;
    private readonly InMemoryDbContext _dbContext;

    public StudentServiceTests()
    {
        _mockUnitOfWork = A.Fake<IUnitOfWork>();
        _mockHybridCache= A.Fake<HybridCache>();
        _mockCacheService = A.Fake<ICacheService>();
        _mapper = A.Fake<IMapper>();
        _StudentRepository = A.Fake<IGenericRepository<Student>>();
        _lessonRepository = A.Fake<IGenericRepository<Lesson>>();
        _userRepository = A.Fake<IGenericRepository<ApplicationUser>>();

        _dbContext = new InMemoryDbContext();

        A.CallTo(() => _mockUnitOfWork.Repository<Student>()).Returns(_StudentRepository);
        A.CallTo(() => _mockUnitOfWork.Repository<Lesson>()).Returns(_lessonRepository);
        A.CallTo(() => _mockUnitOfWork.Repository<ApplicationUser>()).Returns(_userRepository);

        _sut = new StudentService(_dbContext, _mockUnitOfWork, _mockCacheService);
    }

    //GetStudentByIdAsync

    [Fact]
    public async Task GetStudentByIdAsync_WhenStudentDoesNotExist_ShouldReturnStudentNotFound()
    {
        // Arrange
        var StudentId = Guid.NewGuid();

        // Mock the repository call to return null when looking for a Student with the given ID
        A.CallTo(() => _mockUnitOfWork.Repository<Student>()
                                   .FirstOrDefaultAsync(A<Expression<Func<Student, bool>>>._,
                                                        A<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>._,
                                                        CancellationToken.None))
          .Returns(Task.FromResult<Student>(null));

        // Act
        var result = await _sut.GetStudentByIdAsync(StudentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(StudentsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task GetStudentByIdAsync_WhenStudentExists_ShouldReturnStudent()
    {
        // Arrange
        var StudentId = Guid.NewGuid();
        var Student = new Student
        {
            StudentId = StudentId,
            IsActive = true
        };

        // Mock the repository call to return the Student when searching by the given ID
        A.CallTo(() => _mockUnitOfWork.Repository<Student>()
                                       .FirstOrDefaultAsync(A<Expression<Func<Student, bool>>>.That.Matches(x => x.Compile().Invoke(Student)),
                                                            A<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>._,
                                                            CancellationToken.None))
          .Returns(Task.FromResult(Student));

        // Act
        var result = await _sut.GetStudentByIdAsync(StudentId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value); // Ensure the result contains a non-null value
        Assert.Equal(Student.StudentId, result.Value.StudentId); // Ensure the correct Student is returned
    }



    //CreateStudent
  
    [Fact]
    public async Task CreateStudentAsync_WhenUserNotEsisit_ShouldReturnNotFoundUser()
    {
        // Arrange

        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
          .Returns(false);



        var applicationUser = new ApplicationUser
        {
            FirstName = "John",
            LastName = "Doe",
            IsDisabled = false,
            Email = "john.doe@example.com",
            UserName = "john.doe"
        };
        //Act

        var resutl = await _sut.CreateStudentAsync(applicationUser);

        //Assert

        Assert.False(resutl.IsSuccess);
        Assert.Equal(UserErrors.UserNotFound, resutl.Error);

    }

    [Fact]
    public async Task CreateStudentAsync_WhenAllValidationsPass_ShouldCreateStudentSuccessfully()
    {
        // Arrange
        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
          .Returns(value: true);

        var applicationUser = new ApplicationUser
        {
            FirstName = "John",
            LastName = "Doe",
            IsDisabled = false,
            Email = "john.doe@example.com",
            UserName = "john.doe"
        };
        //Act

        var resutl = await _sut.CreateStudentAsync(applicationUser);

        //Assert

        Assert.True(resutl.IsSuccess);



    }




    //UpdateStudentAsync

    [Fact]
    public async Task UpdateStudentAsync_WhenUserNotEsisit_ShouldReturnNotFoundUser()
    {
        // Arrange
        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
         .Returns(false);

        var StudentId = Guid.NewGuid();
        var studentRequest = new StudentRequest(FirstName: "John", LastName: "Doe", Email: "john.doe@example.com");

        // Act
        var result = await _sut.UpdateStudentAsync(StudentId, studentRequest);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(StudentsErrors.NotFound, result.Error);
    }
    [Fact]
    public async Task UpdateStudentAsync_WhenAllValidationsPass_ShouldUpdateStudentSuccessfully()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var existingStudent = new Student
        {
            StudentId = studentId,
            IsActive = true,
            UserId = "existing-user-id",
            User = new ApplicationUser { UserName = "existing-user" }
        };

        // Mock the repository call to return the existing student when searching by ID
        A.CallTo(() => _mockUnitOfWork.Repository<Student>()
            .FirstOrDefaultAsync(A<Expression<Func<Student, bool>>>.That.Matches(x => x.Compile().Invoke(existingStudent)),
                                 A<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>._,
                                 CancellationToken.None))
            .Returns(Task.FromResult(existingStudent));

        // Mock the check for user existence
        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
            .Returns(true);

        // Mock the repository save action
        // A.CallTo(() => A.Fake<_mockUnitOfWork.CompleteAsync(A<CancellationToken>._)())
        //.Returns(Task.CompletedTask);




        // Prepare the student request
        var studentRequest = new StudentRequest("John", "Doe", "john.doe@example.com");

        // Act
        var result = await _sut.UpdateStudentAsync(studentId, studentRequest);

        // Assert
        Assert.True(result.IsSuccess);  // Assert the operation was successful
        A.CallTo(() => _mockUnitOfWork.CompleteAsync(A<CancellationToken>._))  // Ensure the save method was called
            .MustHaveHappenedOnceExactly();
    }




    //ToggleStatusAsync

    [Fact]
    public async Task ToggleStatusAsync_WhenStudentDoesNotExist_ShouldReturnStudentNotFound()
    {
        // Arrange
        A.CallTo(() => _mockUnitOfWork.Repository<Student>()
                             .FirstOrDefaultAsync(A<Expression<Func<Student, bool>>>._,
                                                  A<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>._,
                                                  CancellationToken.None))
    .Returns(Task.FromResult<Student>(null));


        var StudentId = Guid.NewGuid();

        // Act
        var result = await _sut.ToggleStatusAsync(StudentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(StudentsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task ToggleStatusAsync_WhenToggleStatus_ShouldReturnSuccess()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var existingStudent = new Student
        {
            StudentId = Guid.NewGuid(),
            IsActive = true
        };

        A.CallTo(() => _mockUnitOfWork.Repository<Student>()
                                      .FirstOrDefaultAsync(A<Expression<Func<Student, bool>>>._,
                                                           A<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>._,
                                                           CancellationToken.None))
            .Returns(existingStudent);


        // Act
        var result = await _sut.ToggleStatusAsync(lessonId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(existingStudent.IsActive); // Ensure the status was toggled
        A.CallTo(() => _mockUnitOfWork.CompleteAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }



    // GetAllStudentsAsync
    //[Fact]
    //public async Task GetAllStudentsAsync_ReturnsStudentResponses()
    //{
    //    // Arrange
    //    var students = new List<Student>
    //{
    //    new Student
    //    {
    //        StudentId = Guid.NewGuid(),
    //        User = new ApplicationUser { UserName = "User1", Email = "user1@example.com" },
    //        UserId = "user1-id",
    //        IsActive = true,
    //        Enrollments = new List<Enrollment>(), // Empty collections for Enrollments
    //        QuizAttempts = new List<QuizAttempt>(), // Empty collections for QuizAttempts
    //    },
    //    new Student
    //    {
    //        StudentId = Guid.NewGuid(),
    //        User = new ApplicationUser { UserName = "User2", Email = "user2@example.com" },
    //        UserId = "user2-id",
    //        IsActive = true,
    //        Enrollments = new List<Enrollment>(), // Empty collections for Enrollments
    //        QuizAttempts = new List<QuizAttempt>(), // Empty collections for QuizAttempts
    //    }
    //};

    //    // Mocking the cache service to simulate cache miss (it should return failure, cache not found)
    //    A.CallTo(() => _mockCacheService.GetCacheAsync<IEnumerable<StudentResponse>>("Student:GetAll"))
    //        .Returns(Result.Failure<IEnumerable<StudentResponse>>(CashErrors.NotFound));

    //    // Mocking the FindAsync method to return the students from the repository
    //    A.CallTo(() => _mockUnitOfWork.Repository<Student>().FindAsync(
    //            A<Expression<Func<Student, bool>>>(), // Correct predicate for active students
    //            A<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>._,
    //            A<CancellationToken>._))
    //        .Returns(Task.FromResult(students.AsQueryable())); // Return students as IQueryable

    //    // Mapping Student to StudentResponse manually as we do with Mapster in production code
    //    var studentResponses = new List<StudentResponse>
    //{
    //    new StudentResponse(
    //        StudentId: students[0].StudentId,
    //        StudentName: "Student 1",
    //        CreatedBy: students[0].User.UserName, // Using UserName for CreatedBy
    //        CreatedOn: DateTime.UtcNow, // Assuming current time for CreatedOn
    //        Email: students[0].User.Email, // Using Email from User object
    //        IsActive: students[0].IsActive
    //    ),
    //    new StudentResponse(
    //        StudentId: students[1].StudentId,
    //        StudentName: "Student 2",
    //        CreatedBy: students[1].User.UserName, // Using UserName for CreatedBy
    //        CreatedOn: DateTime.UtcNow, // Assuming current time for CreatedOn
    //        Email: students[1].User.Email, // Using Email from User object
    //        IsActive: students[1].IsActive
    //    )
    //};

    //    // Mocking the Mapster's Adapt method to return the studentResponses
    //    A.CallTo(() => _mapper.Map<IEnumerable<StudentResponse>>(students))
    //        .Returns(studentResponses);


    //    // Act
    //    var result = await _sut.GetAllStudentsAsync(CancellationToken.None);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.True(result.IsSuccess);
    //    Assert.Equal(2, result.Value.Count());
    //    Assert.Equal("Student 1", result.Value.First().StudentName);
    //    Assert.Equal("Student 2", result.Value.Last().StudentName);
    //    Assert.Equal("user1@example.com", result.Value.First().Email);

    //    // Verify that the cache service's SetCacheAsync was called to cache the result
    //    A.CallTo(() => _mockCacheService.SetCacheAsync("Student:GetAll", studentResponses, A<TimeSpan>._))
    //        .MustHaveHappenedOnceExactly();

    //    // Verify that the FindAsync method was called to fetch the students
    //    A.CallTo(() => _mockUnitOfWork.Repository<Student>().FindAsync(
    //            A<Expression<Func<Student, bool>>>.That.Matches(x => x.Compile()(s.IsActive)), // Check the correct predicate
    //            A<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>._,
    //            A<CancellationToken>._))
    //        .MustHaveHappenedOnceExactly();
    //}

}
