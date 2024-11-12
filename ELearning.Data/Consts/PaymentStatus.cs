using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Consts;
public static class PaymentStatus
{
    public const string Completed = "Completed";
    public const string Failed = "Failed";
    public const string Refunded = "Refunded";


    public const string CanceledForDifferentCourse = "CanceledForDifferentCourse";
    public const string CanceledForDifferentStudent = "CanceledForDifferentStudent";

    public const string PaymentForDifferentCourse = "PaymentForDifferentCourse";
    public const string PaymentForDifferentStudent = "PaymentForDifferentStudent";

}
