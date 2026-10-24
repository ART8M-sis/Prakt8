using Classwork_17_10_2026.Data;
using Classwork_17_10_2026.DTO.User;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FileAvatar.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDTO>
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private static readonly string[] AllowedRoles = { "User", "Admin", "Manager" };
        private const long MaxFileSize = 2 * 1024 * 1024;

        private readonly AppDbContext _context;

        public CreateUserDtoValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.")
                .EmailAddress().WithMessage("Email format is invalid.")
                .MustAsync(BeUniqueEmail).WithMessage("A user with this email already exists.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => AllowedRoles.Contains(role))
                .WithMessage($"Role must be one of: {string.Join(", ", AllowedRoles)}.");

            RuleFor(x => x.Age)
                .InclusiveBetween(18, 120).WithMessage("Age must be between 18 and 120.");

            RuleFor(x => x.Avatar)
                .Must(HaveValidExtension)
                .WithMessage($"Invalid file extension. Allowed: {string.Join(", ", AllowedExtensions)}.")
                .Must(HaveValidSize)
                .WithMessage("Avatar size must not exceed 2 MB.")
                .When(x => x.Avatar != null);
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
            => !await _context.Users.AnyAsync(u => u.Email == email, ct);

        private static bool HaveValidExtension(IFormFile? file)
        {
            if (file == null) return true;
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return AllowedExtensions.Contains(ext);
        }

        private static bool HaveValidSize(IFormFile? file)
            => file == null || file.Length <= MaxFileSize;
    }
}