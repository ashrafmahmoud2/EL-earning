using ELearning.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Service.Service;
public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Student> CreateStudentAsync(Student student, CancellationToken cancellationToken = default)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        await _unitOfWork.Repository<Student>().AddAsync(student, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return student;
    }

    public async Task<Student> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var students = await _unitOfWork.Repository<Student>()
            .FindAsync(s => s.Id == id, include: q => q.Include(s => s.ApplicationUser), cancellationToken);
        var student = students.FirstOrDefault();

        if (student == null)
            throw new KeyNotFoundException($"Student with ID {id} not found.");

        return student;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Repository<Student>()
            .FindAsync(s => true, include: q => q.Include(s => s.ApplicationUser), cancellationToken);
    }

    public async Task<Student> UpdateStudentAsync(Student student, CancellationToken cancellationToken = default)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        var existingStudent = await GetStudentByIdAsync(student.Id, cancellationToken);

        // Update properties
        existingStudent.ApplicationUser.FirstName = student.ApplicationUser.FirstName;
        existingStudent.ApplicationUser.LastName = student.ApplicationUser.LastName;
        existingStudent.ApplicationUser.Email = student.ApplicationUser.Email;
        existingStudent.ApplicationUser.UpdatedAt = DateTime.UtcNow;
        // Add more property updates as needed

        await _unitOfWork.Repository<Student>().UpdateAsync(existingStudent, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return existingStudent;
    }

    public async Task DeleteStudentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await GetStudentByIdAsync(id, cancellationToken);
        await _unitOfWork.Repository<Student>().RemoveAsync(student, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}