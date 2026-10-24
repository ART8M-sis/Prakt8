using Classwork_17_10_2026.DTO.User;

namespace Classwork_17_10_2026.Services
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO dto);
    }
}