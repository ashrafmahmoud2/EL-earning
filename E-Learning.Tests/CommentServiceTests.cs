using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using E_Learning.Tests;
using ELearning.Data.Contracts.Comment;
using ELearning.Data.Entities;
using ELearning.Data.Errors;
using ELearning.Infrastructure.Base;
using ELearning.Service.IService;
using ELearning.Service.Service;
using FakeItEasy;
using Mapster;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Hybrid;
using Xunit;
using Xunit.Sdk;

public class CommentServiceTests
{

    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly HybridCache _mockHybridCache;
    private readonly IGenericRepository<Comment> _commentRepository;
    private readonly IGenericRepository<Lesson> _lessonRepository;
    private readonly IGenericRepository<ApplicationUser> _userRepository;
    private readonly CommentService _sut;
    private readonly InMemoryDbContext _dbContext;

    public CommentServiceTests()
    {
        _mockUnitOfWork = A.Fake<IUnitOfWork>();
        _mockHybridCache = A.Fake<HybridCache>();
        _commentRepository = A.Fake<IGenericRepository<Comment>>();
        _lessonRepository = A.Fake<IGenericRepository<Lesson>>();
        _userRepository = A.Fake<IGenericRepository<ApplicationUser>>();

        _dbContext = new InMemoryDbContext();

        A.CallTo(() => _mockUnitOfWork.Repository<Comment>()).Returns(_commentRepository);
        A.CallTo(() => _mockUnitOfWork.Repository<Lesson>()).Returns(_lessonRepository);
        A.CallTo(() => _mockUnitOfWork.Repository<ApplicationUser>()).Returns(_userRepository);

        _sut = new CommentService(_dbContext, _mockUnitOfWork, _mockHybridCache);
    }

    //GetCommentByIdAsync

    [Fact]
    public async Task GetCommentByIdAsync_WhenCommentDoesNotExist_ShouldReturnCommentNotFound()
    {
        // Arrange
        var commentId = Guid.NewGuid();

        // Mock the repository call to return null when looking for a comment with the given ID
        A.CallTo(() => _mockUnitOfWork.Repository<Comment>()
                                   .FirstOrDefaultAsync(A<Expression<Func<Comment, bool>>>._,
                                                        A<Func<IQueryable<Comment>, IIncludableQueryable<Comment, object>>>._,
                                                        CancellationToken.None))
          .Returns(Task.FromResult<Comment>(null));

        // Act
        var result = await _sut.GetCommentByIdAsync(commentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CommentsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task GetCommentByIdAsync_WhenCommentExists_ShouldReturnComment()
    {
        // Arrange
        var commentId = Guid.NewGuid();
        var comment = new Comment
        {
            CommentId = commentId,
            IsActive = true
        };

        // Mock the repository call to return the comment when searching by the given ID
        A.CallTo(() => _mockUnitOfWork.Repository<Comment>()
                                       .FirstOrDefaultAsync(A<Expression<Func<Comment, bool>>>.That.Matches(x => x.Compile().Invoke(comment)),
                                                            A<Func<IQueryable<Comment>, IIncludableQueryable<Comment, object>>>._,
                                                            CancellationToken.None))
          .Returns(Task.FromResult(comment));

        // Act
        var result = await _sut.GetCommentByIdAsync(commentId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value); // Ensure the result contains a non-null value
        Assert.Equal(comment.CommentId, result.Value.CommentId); // Ensure the correct comment is returned
    }



    //UpdateCommentAsync

    [Fact]
    public async Task UpdateCommentAsync_WhenLessonDoesNotExist_ShouldReturnLessonNotFound()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
            .Returns(false);

        var commentId = Guid.NewGuid();
        var commentRequest = new CommentRequest("Test Comment", "This is a test", Guid.NewGuid(), "user123");

        // Act
        var result = await _sut.UpdateCommentAsync(commentId, commentRequest);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(LessonsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task UpdateCommentAsync_WhenUserDoesNotExist_ShouldReturnUserNotFound()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
            .Returns(false);

        //A.CallTo(() => _commentRepository.AnyAsync(A<Expression<Func<Comment, bool>>>._, A<CancellationToken>._))
        //  .Returns(false);

        var commentRequest = new CommentRequest("Test Comment", "This is a test", Guid.NewGuid(), "user123");

        var commentId = Guid.NewGuid();
        //Act

        var resutl = await _sut.UpdateCommentAsync(commentId, commentRequest);

        //Assert

        Assert.False(resutl.IsSuccess);
        Assert.Equal(UserErrors.UserNotFound, resutl.Error);

    }

    [Fact]
    public async Task UpdateCommentAsync_WhenCommentIdNotFound_ShouldReturnNotFound()
    {
        // Arrange

        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
          .Returns(true);

        A.CallTo(() => _mockUnitOfWork.Repository<Comment>()
                                    .FirstOrDefaultAsync(A<Expression<Func<Comment, bool>>>._,
                                                         A<Func<IQueryable<Comment>, IIncludableQueryable<Comment, object>>>._,
                                                         CancellationToken.None))
           .Returns(Task.FromResult<Comment>(null));


        var commentRequest = new CommentRequest("Test Comment", "This is a test", Guid.NewGuid(), "user123");
        var commentId = Guid.NewGuid();
        //Act

        var resutl = await _sut.UpdateCommentAsync(commentId, commentRequest);

        //Assert

        Assert.False(resutl.IsSuccess);
        Assert.Equal(CommentErrors.CommentNotFound, resutl.Error);

    }

    [Fact]
    public async Task UpdateCommentAsync_WhenRequestIsNull_ShouldReturnNotFoundError()
    {
        // Act
        var commentId = Guid.NewGuid();
        var result = await _sut.UpdateCommentAsync(commentId, null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(LessonsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task UpdateCommentAsync_WhenAllValidationsPass_ShouldCreateCommentSuccessfully()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
          .Returns(true);

        A.CallTo(() => _mockUnitOfWork.Repository<Comment>()
                                    .FirstOrDefaultAsync(A<Expression<Func<Comment, bool>>>._,
                                                         A<Func<IQueryable<Comment>, IIncludableQueryable<Comment, object>>>._,
                                                         CancellationToken.None))
           .Returns(Task.FromResult<Comment>(A.Fake<Comment>()));

        var commentRequest = new CommentRequest("Test Comment", "This is a test", Guid.NewGuid(), "user123");

        var commentId = Guid.NewGuid();
        //Act

        var resutl = await _sut.UpdateCommentAsync(commentId, commentRequest);

        //Assert

        Assert.True(resutl.IsSuccess);
    }


    //CreateComment

    [Fact]
    public async Task CreateCommentAsync_WhenLessonDoesNotExist_ShouldReturnLessonNotFound()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
            .Returns(false);

        var commentRequest = new CommentRequest("Test Comment", "This is a test", Guid.NewGuid(), "user123");

        // Act
        var result = await _sut.CreateCommentAsync(commentRequest);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(LessonsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task CreateCommentAsync_WhenUserDoesNotExist_ShouldReturnUserNotFound()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
            .Returns(false);

        A.CallTo(() => _commentRepository.AnyAsync(A<Expression<Func<Comment, bool>>>._, A<CancellationToken>._))
          .Returns(false);

        var commentRequest = new CommentRequest("Test Comment", "This is a test", Guid.NewGuid(), "user123");

        //Act

        var resutl = await _sut.CreateCommentAsync(commentRequest);

        //Assert

        Assert.False(resutl.IsSuccess);
        Assert.Equal(UserErrors.UserNotFound, resutl.Error);

    }

    [Fact]
    public async Task CreateCommentAsync_WhenDuplicateComment_ShouldReturnDuplicatedComment()
    {
        // Arrange

        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
          .Returns(true);

        A.CallTo(() => _commentRepository.AnyAsync(A<Expression<Func<Comment, bool>>>._, A<CancellationToken>._))
            .Returns(true);

        var commentRequest = new CommentRequest("Test Comment", "This is a test", Guid.NewGuid(), "user123");

        //Act

        var resutl = await _sut.CreateCommentAsync(commentRequest);

        //Assert

        Assert.False(resutl.IsSuccess);
        Assert.Equal(CommentErrors.DuplicatedComment, resutl.Error);

    }

    [Fact]
    public async Task CreateCommentAsync_WhenRequestIsNull_ShouldReturnNotFoundError()
    {
        // Act
        var result = await _sut.CreateCommentAsync(null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(LessonsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task CreateCommentAsync_WhenAllValidationsPass_ShouldCreateCommentSuccessfully()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _userRepository.AnyAsync(A<Expression<Func<ApplicationUser, bool>>>._, A<CancellationToken>._))
          .Returns(true);

        A.CallTo(() => _commentRepository.AnyAsync(A<Expression<Func<Comment, bool>>>._, A<CancellationToken>._))
            .Returns(false);

        var commentRequest = new CommentRequest("Test Comment", "This is a test", Guid.NewGuid(), "user123");

        //Act

        var resutl = await _sut.CreateCommentAsync(commentRequest);

        //Assert

        Assert.True(resutl.IsSuccess);



    }



    //ToggleStatusAsync

    [Fact]
    public async Task ToggleStatusAsync_WhenCommentDoesNotExist_ShouldReturnCommentNotFound()
    {
        // Arrange
        A.CallTo(() => _mockUnitOfWork.Repository<Comment>()
                             .FirstOrDefaultAsync(A<Expression<Func<Comment, bool>>>._,
                                                  A<Func<IQueryable<Comment>, IIncludableQueryable<Comment, object>>>._,
                                                  CancellationToken.None))
    .Returns(Task.FromResult<Comment>(null));


        var commentId = Guid.NewGuid();

        // Act
        var result = await _sut.ToggleStatusAsync(commentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CommentsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task ToggleStatusAsync_WhenToggleStatus_ShouldReturnSuccess()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var existingComment = new Comment
        {
            CommentId = Guid.NewGuid(),
            IsActive = true
        };

        A.CallTo(() => _mockUnitOfWork.Repository<Comment>()
                                      .FirstOrDefaultAsync(A<Expression<Func<Comment, bool>>>._,
                                                           A<Func<IQueryable<Comment>, IIncludableQueryable<Comment, object>>>._,
                                                           CancellationToken.None))
            .Returns(existingComment);


        // Act
        var result = await _sut.ToggleStatusAsync(lessonId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(existingComment.IsActive); // Ensure the status was toggled
        A.CallTo(() => _mockUnitOfWork.CompleteAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

   
    
    //CountCommentsForLesson
    [Fact]
    public async Task CountCommentsForLesson_WhenCommentExist_ReturnsCorrectCount()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();

        // Add test comments to the in-memory database
        _dbContext.Comments.Add(new Comment { CommentId = Guid.NewGuid(), LessonId = lessonId });
        _dbContext.Comments.Add(new Comment { CommentId = Guid.NewGuid(), LessonId = lessonId });
        _dbContext.Comments.Add(new Comment { CommentId = Guid.NewGuid(), LessonId = lessonId });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.CountCommentsForLesson(lessonId, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value); // There should be 3 comments for the lesson
    }

    [Fact]
    public async Task CountCommentsForLesson_ReturnsZero_WhenNoCommentsFound()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();


        // Act
        var result = await _sut.CountCommentsForLesson(lessonId, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value); // No comments for the lesson
       }















        // GetAllCommentsAsync
        //[Fact]
        //public async Task GetAllCommentsAsync_WhenHaveComments_ShouldReturnComments()
        //{
        //    // Arrange
        //    var comments = new List<Comment>
        //{
        //    new Comment
        //    {
        //        CommentId = Guid.NewGuid(),
        //        Title = "First Comment",
        //        CommentText = "This is the first comment.",
        //        LessonId = Guid.NewGuid(),
        //        CommentedByUserId = Guid.NewGuid().ToString(),
        //        IsActive = true
        //    },
        //    new Comment
        //    {
        //        CommentId = Guid.NewGuid(),
        //        Title = "Second Comment",
        //        CommentText = "This is the second comment.",
        //        LessonId = Guid.NewGuid(),
        //        CommentedByUserId = Guid.NewGuid().ToString(),
        //        IsActive = true
        //    }
        //};

        //    // Mock the repository call to return the comments list
        //    A.CallTo(() => _mockUnitOfWork.Repository<Comment>()
        //                                   .FindAsync(
        //                                       A<Expression<Func<Comment, bool>>>._, // Ignore predicate matching
        //                                       A<Func<IQueryable<Comment>, IIncludableQueryable<Comment, object>>>._,
        //                                       CancellationToken.None
        //                                   ))
        //      .Returns(Task.FromResult(comments.AsEnumerable()));

        //    // Act
        //    var result = await _sut.GetAllCommentsAsync();

        //    // Assert
        //    Assert.NotNull(result); // Ensure the result is not null
        //    Assert.True(result.IsSuccess); // Ensure the service operation was successful
        //    Assert.NotNull(result.Value); // Ensure the Value property is not null
        //    Assert.Equal(comments.Count, result.Value.Count()); // Ensure the correct number of comments is returned

        //    // Validate the mapping
        //    var firstComment = result.Value.First();
        //    Assert.Equal(comments[0].CommentId, firstComment.CommentId);
        //    Assert.Equal(comments[0].Title, firstComment.Title);
        //    Assert.Equal(comments[0].CommentText, firstComment.CommentText);
        //    Assert.Equal(comments[0].LessonId, firstComment.LessonId);
        //    Assert.Equal(comments[0].CommentedByUserId, firstComment.CommentedByUserId);
        //    Assert.Equal(comments[0].IsActive, firstComment.IsActive);
        //}



    }
