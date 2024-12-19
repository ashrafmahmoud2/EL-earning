using System.Linq.Expressions;
using E_Learning.Tests;
using ELearning.Data.Contracts.Document;
using ELearning.Data.Entities;
using ELearning.Data.Errors;
using ELearning.Infrastructure.Base;
using ELearning.Service.IService;
using ELearning.Service.Service;
using FakeItEasy;
using Mailjet.Client.Resources;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Hybrid;

public class DocumentServiceTests
{

    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IGenericRepository<Document> _DocumentRepository;
    private readonly IGenericRepository<Lesson> _lessonRepository;
    private readonly DocumentService _sut;
    private readonly IMapper _mapper;
    private readonly InMemoryDbContext _dbContext;

    public DocumentServiceTests()
    {
        _mockUnitOfWork = A.Fake<IUnitOfWork>();
        _mapper = A.Fake<IMapper>();
        _DocumentRepository = A.Fake<IGenericRepository<Document>>();
        _lessonRepository = A.Fake<IGenericRepository<Lesson>>();

        _dbContext = new InMemoryDbContext();

        A.CallTo(() => _mockUnitOfWork.Repository<Document>()).Returns(_DocumentRepository);
        A.CallTo(() => _mockUnitOfWork.Repository<Lesson>()).Returns(_lessonRepository);

        _sut = new DocumentService(_dbContext, _mockUnitOfWork);
    }

    //GetDocumentByIdAsync

    [Fact]
    public async Task GetDocumentByIdAsync_WhenDocumentDoesNotExist_ShouldReturnDocumentNotFound()
    {
        // Arrange
        var DocumentId = Guid.NewGuid();

        // Mock the repository call to return null when looking for a Document with the given ID
        A.CallTo(() => _mockUnitOfWork.Repository<Document>()
                                   .FirstOrDefaultAsync(A<Expression<Func<Document, bool>>>._,
                                                        A<Func<IQueryable<Document>, IIncludableQueryable<Document, object>>>._,
                                                        CancellationToken.None))
          .Returns(Task.FromResult<Document>(null));

        // Act
        var result = await _sut.GetDocumentByIdAsync(DocumentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DocumentsErrors.DocumentNotFound, result.Error);
    }

    [Fact]
    public async Task GetDocumentByIdAsync_WhenDocumentExists_ShouldReturnDocument()
    {
        // Arrange
        var DocumentId = Guid.NewGuid();
        var Document = new Document
        {
            DocumentId = DocumentId,
            IsActive = true
        };

        // Mock the repository call to return the Document when searching by the given ID
        A.CallTo(() => _mockUnitOfWork.Repository<Document>()
                                       .FirstOrDefaultAsync(A<Expression<Func<Document, bool>>>.That.Matches(x => x.Compile().Invoke(Document)),
                                                            A<Func<IQueryable<Document>, IIncludableQueryable<Document, object>>>._,
                                                            CancellationToken.None))
          .Returns(Task.FromResult(Document));

        // Act
        var result = await _sut.GetDocumentByIdAsync(DocumentId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value); // Ensure the result contains a non-null value
        Assert.Equal(Document.DocumentId, result.Value.DocumentId); // Ensure the correct Document is returned
    }



    //CreateDocument

    [Fact]
    public async Task CreateDocumentAsync_WhenLessonDoesNotExist_ShouldReturnLessonNotFound()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
            .Returns(false);

        var documentRequest = new DocumentRequest("Sample Document Title", "A brief description of the document", "/path/to/document.pdf", Guid.NewGuid());

        // Act
        var result = await _sut.CreateDocumentAsync(documentRequest);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(LessonsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task CreateDocumentAsync_WhenDuplicateDocument_ShouldReturnDuplicatedDocument()
    {
        // Arrange

        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);


        A.CallTo(() => _DocumentRepository.AnyAsync(A<Expression<Func<Document, bool>>>._, A<CancellationToken>._))
            .Returns(true);

        var documentRequest = new DocumentRequest("Sample Document Title", "A brief description of the document", "/path/to/document.pdf", Guid.NewGuid());

        //Act

        var resutl = await _sut.CreateDocumentAsync(documentRequest);

        //Assert

        Assert.False(resutl.IsSuccess);
        Assert.Equal(DocumentErrors.DuplicatedDocument, resutl.Error);

    }

    [Fact]
    public async Task CreateDocumentAsync_WhenRequestIsNull_ShouldReturnNotFoundError()
    {
        // Act
        var result = await _sut.CreateDocumentAsync(null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(LessonsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task CreateDocumentAsync_WhenAllValidationsPass_ShouldCreateDocumentSuccessfully()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _DocumentRepository.AnyAsync(A<Expression<Func<Document, bool>>>._, A<CancellationToken>._))
            .Returns(false);

        var documentRequest = new DocumentRequest("Sample Document Title", "A brief description of the document", "/path/to/document.pdf", Guid.NewGuid());

        //Act

        var resutl = await _sut.CreateDocumentAsync(documentRequest);

        //Assert

        Assert.True(resutl.IsSuccess);



    }




    //UpdateDocumentAsync

    [Fact]
    public async Task UpdateDocumentAsync_WhenLessonDoesNotExist_ShouldReturnLessonNotFound()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
            .Returns(false);

        var DocumentId = Guid.NewGuid();
        var documentRequest = new DocumentRequest( "Sample Document Title", "A brief description of the document", "/path/to/document.pdf", Guid.NewGuid()
        );

        // Act
        var result = await _sut.UpdateDocumentAsync(DocumentId, documentRequest);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(LessonsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task UpdateDocumentAsync_WhenDocumentIdNotFound_ShouldReturnNotFound()
    {
        // Arrange

        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _mockUnitOfWork.Repository<Document>()
                                    .FirstOrDefaultAsync(A<Expression<Func<Document, bool>>>._,
                                                         A<Func<IQueryable<Document>, IIncludableQueryable<Document, object>>>._,
                                                         CancellationToken.None))
           .Returns(Task.FromResult<Document>(null));


        var documentRequest = new DocumentRequest("Sample Document Title", "A brief description of the document",
                    "/path/to/document.pdf", Guid.NewGuid());
        var DocumentId = Guid.NewGuid();
        //Act

        var resutl = await _sut.UpdateDocumentAsync(DocumentId, documentRequest);

        //Assert

        Assert.False(resutl.IsSuccess);
        Assert.Equal(DocumentErrors.DocumentNotFound, resutl.Error);

    }

    [Fact]
    public async Task UpdateDocumentAsync_WhenRequestIsNull_ShouldReturnNotFoundError()
    {
        // Act
        var DocumentId = Guid.NewGuid();
        var result = await _sut.UpdateDocumentAsync(DocumentId, null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(LessonsErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task UpdateDocumentAsync_WhenAllValidationsPass_ShouldCreateDocumentSuccessfully()
    {
        // Arrange
        A.CallTo(() => _lessonRepository.AnyAsync(A<Expression<Func<Lesson, bool>>>._, A<CancellationToken>._))
           .Returns(true);

        A.CallTo(() => _mockUnitOfWork.Repository<Document>()
                                    .FirstOrDefaultAsync(A<Expression<Func<Document, bool>>>._,
                                                         A<Func<IQueryable<Document>, IIncludableQueryable<Document, object>>>._,
                                                         CancellationToken.None))
           .Returns(Task.FromResult<Document>(A.Fake<Document>()));

        var documentRequest = new DocumentRequest("Sample Document Title", "A brief description of the document", "/path/to/document.pdf", Guid.NewGuid());

        var DocumentId = Guid.NewGuid();
        //Act

        var resutl = await _sut.UpdateDocumentAsync(DocumentId, documentRequest);

        //Assert

        Assert.True(resutl.IsSuccess);
    }



    //ToggleStatusAsync

    [Fact]
    public async Task ToggleStatusAsync_WhenDocumentDoesNotExist_ShouldReturnDocumentNotFound()
    {
        // Arrange
        A.CallTo(() => _mockUnitOfWork.Repository<Document>()
                             .FirstOrDefaultAsync(A<Expression<Func<Document, bool>>>._,
                                                  A<Func<IQueryable<Document>, IIncludableQueryable<Document, object>>>._,
                                                  CancellationToken.None))
    .Returns(Task.FromResult<Document>(null));


        var DocumentId = Guid.NewGuid();

        // Act
        var result = await _sut.ToggleStatusAsync(DocumentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DocumentsErrors.DocumentNotFound, result.Error);
    }

    [Fact]
    public async Task ToggleStatusAsync_WhenToggleStatus_ShouldReturnSuccess()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var existingDocument = new Document
        {
            DocumentId = Guid.NewGuid(),
            IsActive = true
        };

        A.CallTo(() => _mockUnitOfWork.Repository<Document>()
                                      .FirstOrDefaultAsync(A<Expression<Func<Document, bool>>>._,
                                                           A<Func<IQueryable<Document>, IIncludableQueryable<Document, object>>>._,
                                                           CancellationToken.None))
            .Returns(existingDocument);


        // Act
        var result = await _sut.ToggleStatusAsync(lessonId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(existingDocument.IsActive); // Ensure the status was toggled
        A.CallTo(() => _mockUnitOfWork.CompleteAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }



    // GetAllDocumentsAsync
    [Fact]
    public async Task GetAllDocumentsAsync_ReturnsDocumentResponses()
    {
        // Arrange
        var documents = new List<Document>
    {
        new Document
        {
            DocumentId = Guid.NewGuid(),
            Title = "Document 1",
            IsActive = true,
            DocumentPath = "path/to/document1",
            Lesson = new Lesson { LessonId = Guid.NewGuid(), Title = "Lesson 1" },
            CreatedBy = A.Fake<ApplicationUser>()
        },
        new Document
        {
            DocumentId = Guid.NewGuid(),
            Title = "Document 2",
            IsActive = true,
            DocumentPath = "path/to/document2",
            Lesson = new Lesson { LessonId = Guid.NewGuid(), Title = "Lesson 2" },
            CreatedBy = A.Fake<ApplicationUser>()
        }
    };

        // Using FakeItEasy to set up FindAsync method to return documents
        A.CallTo(() => _mockUnitOfWork.Repository<Document>().FindAsync(
                A<Expression<Func<Document, bool>>>.That.Matches(x => x.Compile()(documents[0])), // Predicate match
                A<Func<IQueryable<Document>, IIncludableQueryable<Document, object>>>._,
                A<CancellationToken>._))
            .Returns(documents);

        // Mocking the Adapt method from Mapster to convert Document to DocumentResponse
        var documentResponses = new List<DocumentResponse>
    {
        new DocumentResponse(
            DocumentId: documents[0].DocumentId,
            Title: documents[0].Title,
            Description: "Description of Document 1",
            DocumentPath: documents[0].DocumentPath,
            IsActive: documents[0].IsActive,
            LessonId: documents[0].Lesson.LessonId,
            LessonTitle: documents[0].Lesson.Title,
            CreatedBy: documents[0].CreatedBy.UserName
        ),
        new DocumentResponse(
            DocumentId: documents[1].DocumentId,
            Title: documents[1].Title,
            Description: "Description of Document 2",
            DocumentPath: documents[1].DocumentPath,
            IsActive: documents[1].IsActive,
            LessonId: documents[1].Lesson.LessonId,
            LessonTitle: documents[1].Lesson.Title,
            CreatedBy: documents[1].CreatedBy.UserName
        )
    };



        //A.CallTo(() => documents.Adapt<IEnumerable<DocumentResponse>>())
        //      .Returns(documentResponses);

        A.CallTo(() => _mapper.Map<IEnumerable<DocumentResponse>>(documents))
       .Returns(documentResponses);





        // Act
        var result = await _sut.GetAllDocumentsAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Document 1", result.First().Title);
        Assert.Equal("Document 2", result.Last().Title);
        Assert.Equal("path/to/document1", result.First().DocumentPath);
    }

}
