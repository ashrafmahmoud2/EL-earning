// Project-specific namespaces
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

// Third-party libraries
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using HangfireBasicAuthenticationFilter;
global using ELearning.Core.MediatrHandlers.Student.Queries.GetStudentById;

// Packages
global using MediatR;
global using Hangfire;
