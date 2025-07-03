using ChatAppMini.Models;
using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;
using ChatAppMini.DTOs.User;
using Utils;

namespace ChatAppMini.Services;

public interface IUserService
{
    Task<ServiceResult<List<ResponseUserDto>>> GetUsersAsync();
    Task<ServiceResult<ResponseUserDto>> CreateUserAsync(RequestUserDto user);
    Task<ServiceResult<ResponseUserDto>> GetUsersByIdAsync(Guid id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<ServiceResult<List<ResponseUserDto>>> GetUsersAsync()
    {
        List<ResponseUserDto> users = await _repo.GetUsersAsync();

        return ServiceResult<List<ResponseUserDto>>.Success(users);
    }

    public async Task<ServiceResult<ResponseUserDto>> CreateUserAsync(RequestUserDto user)
    {   
        if (user == null ||
            string.IsNullOrEmpty(user.Name) ||
            string.IsNullOrEmpty(user.Email) ||
            string.IsNullOrEmpty(user.Password) ||
            string.IsNullOrEmpty(user.ConfirmPassword))
        {
            return ServiceResult<ResponseUserDto>.Fail("Invalid user data provided.");
        }

        if (user.Password != user.ConfirmPassword)
        {
            return ServiceResult<ResponseUserDto>.Fail("Passwords do not match.");
        }

        await _repo.CreateUserAsync(user);
        await _repo.SaveChangesAsync();

        ResponseUserDto responseUser = new ResponseUserDto
        {
            Name = user.Name,
            Email = user.Email
        };

        return ServiceResult<ResponseUserDto>.Success(responseUser);
    }

    public async Task<ServiceResult<ResponseUserDto>> GetUsersByIdAsync(Guid id)
    {
        ResponseUserDto? user = await _repo.GetUsersByIdAsync(id);

        return user != null 
            ? ServiceResult<ResponseUserDto>.Success(user) 
            : ServiceResult<ResponseUserDto>.Fail("User not found.");
    }
}