// Global namespaces
global using ELearning.Core;
global using ELearning.Infrastructure;
global using ELearning.Service;
global using ELearning.Api;
global using ELearning.Data.Abstractions.Extensions;
global using ELearning.Data.Abstractions.ResultPattern;
global using ELearning.Data.Contracts.Auth;
global using ELearning.Data.Contracts.Roles;
global using ELearning.Data.Contracts.Users;
global using ELearning.Service.IService;
global using ELearning.Core.MediatrHandlers.Student.Queries.GetStudentById;

// Third-party namespaces
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using HangfireBasicAuthenticationFilter;
global using Hangfire;
global using MediatR;


// Scoped namespaces
global using ELearning.Core.MediatrHandlers.Student.Queries.GetAllStudents;
global using ELearning.Api.Base;
global using ELearning.Core.MediatrHandlers.Student.Commands.UpdateStudent;
global using ELearning.Infrastructure.Base;
global using ELearning.Service.Service;
global using ELearning.Data.Contracts.Students;
global using ELearning.Data.Contracts.Section;
global using ELearning.Data.Contracts.Quiz;
global using ELearning.Data.Contracts.QuizAttempt;
global using ELearning.Data.Contracts.Question;
global using ELearning.Data.Contracts.Payment;
global using ELearning.Data.Contracts.Lesson;
global using ELearning.Data.Entities;
global using ELearning.Data.Enums;
global using ELearning.Data.Consts;
global using System.Threading;
global using Microsoft.AspNetCore.RateLimiting;

