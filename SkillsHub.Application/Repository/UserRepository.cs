using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Repository;

public class UserRepository : IUserRepository
{
    protected readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    protected readonly IQueryable<ApplicationUser> _fullInclude;
    public UserRepository(ApplicationDbContext context,

         IMapper mapper)
    {
        _mapper = mapper;
        _context = context;

        _fullInclude = _context?.Users?.AsNoTracking()
            .Include(x => x.UserTeacher)
            .Include(x => x.UserTeacher).ThenInclude(x => x.Groups)
            .Include(x => x.UserStudent).ThenInclude(x => x.Groups).ThenInclude(x => x.Group).ThenInclude(x => x.Lessons)
            .Include(x => x.UserStudent).ThenInclude(x => x.Lessons)
            .Include(x => x.ConnectedUsersInfo).ThenInclude(x => x.BaseUserInfo)
            .OrderBy(x => x.RegistrationDate);

    }

    public async Task<ApplicationUser> GetAsync(Guid id)
    {
        if (id == Guid.Empty) return null;//throw new Exception("Id not set");
        var user = await _fullInclude.FirstOrDefaultAsync(x => x.Id == id);
        _context.ChangeTracker.Clear();
        if (user == null) return null;//throw new Exception("User not found");

        try
        {
            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Detached;
        }
        catch (Exception ex) { }

        return user;
    }
    public async Task<IQueryable<ApplicationUser>> GetItemsAsync()
    {
        var users = _fullInclude.OrderBy(x => x.Id);
        return users;

    }
    public async Task<ApplicationUser> CreateAsync(UserCreateDTO item)
    {
        var user = _mapper.Map<ApplicationUser>(item);
        var userInfo = _mapper.Map<BaseUserInfo>(item);

        #region Validators
        var userRegisterValidator = new UserCreateDTOValidator(string.Empty);
        var userValidationResult = await userRegisterValidator.ValidateAsync(item);
        if (!userValidationResult.IsValid)
        {
            var errors = userValidationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
        var users = _context.Users.ToList();
        if (users.FirstOrDefault(x => x.UserName == user.UserName) != null) throw new Exception("User with this login already exists");
        #endregion

        if (string.IsNullOrEmpty(item.Password)) throw new Exception("Password not set");
        string hashedPassword = HashProvider.ComputeHash(item.Password.Trim());
        user.OwnHashedPassword = hashedPassword;
        user.SecurityStamp = Guid.NewGuid().ToString();

        var result = await _context.Users.AddAsync(user);
        //await _userManager.UpdateSecurityStampAsync(user);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<ApplicationUser> UpdateAsync(UserCreateDTO item)
    {
        #region CreateUser
        var dbUser = await GetAsync(item.Id);
        if (dbUser == null) throw new Exception("User was not found in database");

        var connectedUserInfo = dbUser;
        var user = _mapper.Map<ApplicationUser>(item);

        #region Validators
        var userRegisterValidator = new UserCreateDTOValidator(dbUser.OwnHashedPassword);
        var userValidationResult = await userRegisterValidator.ValidateAsync(item);
        if (!userValidationResult.IsValid)
        {
            var errors = userValidationResult.Errors;
            var errorsString = string.Concat(errors);
            throw new Exception(errorsString);
        }
        //if (_context.Users.FirstOrDefault(x => x.UserName == user.UserName) != null) throw new Exception("User with this login already exists");
        #endregion

        user.SecurityStamp = Guid.NewGuid().ToString();

        if (item.IsCheckPassword) user.OwnHashedPassword = HashProvider.ComputeHash(item.Password.Trim());
        if (!string.IsNullOrEmpty(item.PasswordChanged)) user.OwnHashedPassword = HashProvider.ComputeHash(item.PasswordChanged.Trim());

  
        var result = _context.Users.Update(user);
        var saved = false;
        while (!saved)
        {
            try
            {
                _context.SaveChanges();
                saved = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is ApplicationUser)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            if (proposedValue != null && !proposedValue.Equals(databaseValue))
                            {
                                proposedValues[property] = proposedValue;
                            }
                        }
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
            }
        }

        #endregion


        return user;

    }

    #region Delete

    public async Task<ApplicationUser> SoftDeleteAsync(ApplicationUser item)
    {
        bool isHardDelete = false;
        if (item == null) throw new Exception("User not found");
        if (!item.IsDeleted || isHardDelete)
        {
            item.IsDeleted = true;
            _context.Users.Update(item);
            await _context.SaveChangesAsync();
            var teacher = await _context.Teachers.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.ApplicationUser.Id == item.Id);
            var student = await _context.Students.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.ApplicationUser.Id == item.Id);
            if (teacher != null) teacher.IsDeleted = true;
            if (student != null) student.IsDeleted = true;
        }

        return item;
    }

    public async Task<ApplicationUser> Restore(ApplicationUser item)
    {
        item.IsDeleted = false;
        _context.Users.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    #endregion


}
