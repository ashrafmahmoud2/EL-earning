using ELearning.Data.Abstractions.Const;
using ELearning.Data.Entities;
using ELearning.Data.Enums;
using ELearning.Infrastructure;
using ELearning.Infrastructure.Base;
using ELearning.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Service.Service;
public class NotificationService(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    IEmailService emailService,
    IUnitOfWork unitOfWork) : INotificationService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailService _emailService = emailService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task SendNewCoursesNotification(Guid? courseId = null)
    {
        var courses = courseId.HasValue
            ? await _unitOfWork.Repository<Course>().FindAsync(x => x.CourseId == courseId && x.IsActive && x.CreatedOn == DateTime.Today )
            : await _unitOfWork.Repository<Course>().FindAsync(x => x.IsActive && x.CreatedOn == DateTime.Today);

        if (!courses.Any())
            return; // Exit if no active courses found.

        var users = await _userManager.GetUsersInRoleAsync(UserRole.Student);
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        foreach (var course in courses)
        {
            foreach (var user in users)
            {
                var placeholders = new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                { "{{pollTill}}", course.Title },
                { "{{endDate}}", course.CreatedOn.ToString("yyyy-MM-dd") },
                { "{{url}}", $"{origin}/polls/start/{course.CourseId}" }
            };

                var body = EmailBodyBuilder.GenerateEmailBody("NewCourseNotification", placeholders);
                await _emailService.SendEmailAsync(user.Email!, $"📣Undamy: New course - {course.Title}", body);
            }
        }
    }
}
