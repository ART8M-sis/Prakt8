using AutoMapper;
using Classwork_17_10_2026.Data;
using Classwork_17_10_2026.Data.Models;
using Classwork_17_10_2026.DTO.User;
using Microsoft.EntityFrameworkCore;

namespace Classwork_17_10_2026.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private const string SubFolder = "avatars";

        public UserService(AppDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO dto)
        {
            var user = _mapper.Map<User>(dto);

            if (dto.Avatar != null && dto.Avatar.Length > 0)
            {
                var fileResult = await _fileService.SaveFileAsync(dto.Avatar, SubFolder);
                user.AvatarURL = fileResult.RelativePath;
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return _mapper.Map<UserDTO>(user);
        }
    }
}