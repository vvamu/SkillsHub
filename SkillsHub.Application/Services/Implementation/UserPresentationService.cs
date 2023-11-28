using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SkillsHub.Application.Services.Implementation;

public class UserPresentationService : IUserPresentationService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserPresentationService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //For user - by group
    //For teacher - by group
    public PagedList<TeacherDTO> GetAllTeachers(QueryStringParameters ownerParameters)
    {
        var items = _context.Teachers.Include(x => x.Lessons).Include(x=>x.ApplicationUser).OrderBy(on => on.Id);
        var users = _context.Users.Include(x=>x.UserTeacher).Where(x=>x.UserTeacher != null).OrderBy(on => on.Id);

        var itemsArray = items.ToArray();
        var usersArray = users.ToArray();


        var mappingItems = _mapper.Map<List<TeacherDTO>>(items).AsQueryable();
        mappingItems = _mapper.Map < List < TeacherDTO >>(users).AsQueryable();

        var mappingItemsArray = mappingItems.ToArray();

        for(int i = 0; i < itemsArray.Length; i++)
        {
            mappingItemsArray[i].ApplicationUser = usersArray[i];

        }
        //mappingItems = mappingItems.Include(x => x.ApplicationUser).Include(x=>x.PossibleCources);

        var pageList = PagedList<TeacherDTO>.ToPagedList(mappingItemsArray.AsQueryable(),
            ownerParameters.PageNumber,
            ownerParameters.PageSize);
        return pageList;

    }

    //not complete
    public async Task<IQueryable<ApplicationUser>> FilterUsersAsync(IQueryable<ApplicationUser> users, string filterColumn, string filterValue)
    {
        //FILTERING - FILTER BY COLUMN
        Expression<Func<ApplicationUser, object>> keySelectorFilter = filterColumn?.ToLower() switch
        {
            "firstname" => item => item.FirstName,
            "lastname" => item => item.LastName,
            "surname" => item => item.Surname,
            "sex" => item => item.Sex,
            "birthday" => item => item.BirthDate,
            "registrationdate" => item => item.RegistrationDate,
            "phone" => item => item.Phone,
            "email" => item => item.Email,
            "soucefindcompany" => item => item.SourceFindCompany,
            "isdeleted" => item => item.IsDeleted,
            "" => item => ""
        };

        if (keySelectorFilter != null)
        {

        }
        return users;

    }


    //not complete
    //fulltext
    public async Task<IQueryable<ApplicationUser>> SearchUserAsync(IQueryable<ApplicationUser> users)
    {
        //by firstName, lastName, surName
        return users;
    }

    //not complete
    public async Task<IQueryable<ApplicationUser>> OrderUserAsync(IQueryable<ApplicationUser> users, string orderColumn, string orderValue)
    {
        if (orderColumn == null || orderValue == null) return users;
        //SORTING -  SORT BY COLUMN
        Expression<Func<ApplicationUser, object>> keySelectorOrder = orderColumn.ToLower() switch
        {
            "firstname" => item => item.FirstName,
            "lastname" => item => item.LastName,
            "surname" => item => item.Surname,
            "sex" => item => item.Sex,
            "birthday" => item => item.BirthDate,
            "registrationdate" => item => item.RegistrationDate,
            "phone" => item => item.Phone,
            "email" => item => item.Email,
            "soucefindcompany" => item => item.SourceFindCompany,
            "isdeleted" => item => item.IsDeleted,
            "" => item => ""
        };

        if (keySelectorOrder != null)
        {

            //await items.ContainsAsync(keySelector);

        }
        return users;

    }
    public async Task<PagedList<StudentDTO>> GetAllStudentsAsync(QueryStringParameters ownerParameters)
    {
        var items = _context.Students.Include(x => x.Lessons).OrderBy(on => on.Id);
        var users = _context.Users.Include(x => x.UserStudent).Where(x => x.UserStudent != null).OrderBy(on => on.Id);
        var mappingItems = _mapper.Map<List<StudentDTO>>(items).AsQueryable();
        mappingItems = _mapper.Map<List<StudentDTO>>(users).AsQueryable();
        mappingItems = mappingItems.Include(x => x.ApplicationUser);

        var itemsArray = items.ToArray();
        var usersArray = users.ToArray();

        var mappingItemsArray = mappingItems.ToArray();

        for (int i = 0; i < itemsArray.Length; i++)
        {
            mappingItemsArray[i].ApplicationUser = usersArray[i];

        }


        var pageList = PagedList<StudentDTO>.ToPagedList(mappingItemsArray.AsQueryable(),
            ownerParameters.PageNumber,
            ownerParameters.PageSize);
        return pageList;
    }
    public async Task<TeacherDTO> GetTeacherAsync(Guid id)
    {
        var item = await _context.Teachers.FindAsync(id);
        var result = _mapper.Map<TeacherDTO>(item);
        return result == null ? throw new ArgumentException() : result;
    }
    public async Task<StudentDTO> GetStudentAsync(Guid id)
    {
        var item = await _context.Students.FindAsync(id);
        var result = _mapper.Map<StudentDTO>(item);
        return result == null ? throw new ArgumentException() : result;
    }

    public async Task<PagedList<ApplicationUser>> GetNotVerifiedUsers(QueryStringParameters ownerParameters)
    {
        var items = _context.Users.Where(x => x.IsVerified == false);
        var pageList = PagedList<ApplicationUser>.ToPagedList(items,
        ownerParameters.PageNumber,
        ownerParameters.PageSize);
        return pageList;
    }

}
