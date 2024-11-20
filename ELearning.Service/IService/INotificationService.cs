using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Service.IService;
public interface INotificationService
{
    Task SendNewCoursesNotification(Guid? courseId = null);
}
