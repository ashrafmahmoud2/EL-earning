/*
|:::::::::::::|NEED TO DO|:::::::::::::|

- make the ApplictionDbcontext as interface like in section 41/4 in bookfiy.
- just use CQRS In models user alot in db(Comments,Doucments).
- cofing with strip payment https://github.com/stripe/stripe-dotnet Ask gpt
- End Point To Update Index in Section,lessone,queiz,questions
- revivew all respone then do mapping ;
- DO All steps in project steps
- Do Autherzation;
- Put Docker in prject;
- optmize the order prop Entity 
- convert isActive to isDelete with make IRepoistory return wiout Is Deleted;





|:::::::::::::|NEED TO FIX|:::::::::::::::|

 - fix the send email send :: he cant find Data project just find Api prjct
 - FIX THE AddAsync2 IN RolesController
 - fix permissions: (Defualt User,roles)
 - donet git data not is active toggle status
 - fix UpdateCourseAsync in couress services;
 -  fix the documation of swagger ui
 - Add Mapping EnrollmentResponse for sudent and coures Name
 - fix get all enrollmetn 
- instructo create request
 




|:::::::::::::|NEED TO REVIEW|:::::::::::::|

- see the Rep section in bookfiy
- Guid in c# OR CREATE Vervier 7 the vido in code refectory
- is ICollection in mkae fronie key , what it's,is it best prictec;
-Is it importent to put the ForeignKey cofig if if oredy in the sql db


  if (!await _unitOfWork.Repository<Question>().AnyAsync(x => x.QuestionId == request.QuestionId))
            return Result.Failure<AnswerResponse>(QuestionErrors.QuestionNotFound);
{
  "email": "johndoe@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe"
}

 */



