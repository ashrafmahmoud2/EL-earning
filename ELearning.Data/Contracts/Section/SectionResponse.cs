using ELearning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Section;
public record SectionResponse
(
 Guid SectionId,
 string Title,
 string Description,
 int OrderIndex,
 bool IsActive,
 Guid CourseId
    //string counres Name
//IEnumerable<Lesson> Lessons //cotain All Lesones id , name
 );


