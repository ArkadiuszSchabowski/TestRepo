using _076.AngularExcercisesAPI.Database;
using _076.AngularExcercisesAPI.Database.Entities;
using _076.AngularExcercisesAPI.Exceptions;
using _076.AngularExcercisesAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _076.AngularExcercisesAPI.Services
{
    public interface IAccountService
        {
            void CreateAccount(CreateUserDto dto);
            string Login(LoginDto dto);
        }
        public class AccountService : IAccountService
        {
            private readonly MyDbContext _context;
            private readonly IPasswordHasher<User> _hasher;
            private readonly Authentication _authenticationSettings;

        public AccountService(MyDbContext context, Authentication authenticationSettings)
        {
            _context = context;
            _authenticationSettings = authenticationSettings;
        }

        public AccountService(MyDbContext context, IPasswordHasher<User> hasher, Authentication authenticationSettings)
            {
                _context = context;
                _hasher = hasher;
                _authenticationSettings = authenticationSettings;
            }
            public void CreateAccount(CreateUserDto dto)
            {
                var user = _context.Users.FirstOrDefault(x => x.Login == dto.Login);

                if (user != null)
                {
                    throw new ConflictException("Nazwa użytkownika jest już zajęta");
                }

                var userEmail = _context.Users.FirstOrDefault(e => e.Email == dto.Email);

                if (userEmail != null)
                {
                    throw new ConflictException("Taki e-mail istnieje już w bazie danych");
                }


                var newUser = new User
                {
                    Login = dto.Login,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    City = dto.City,
                    Country = dto.Country,
                };

                var hashedPassword = _hasher.HashPassword(newUser, dto.Password);

                newUser.HashPassword = hashedPassword;

                _context.Users.Add(newUser);
                _context.SaveChanges();
            }

            public string Login(LoginDto dto)
            {
                var user = _context.Users
                    .Include(x => x.Role)
                    .FirstOrDefault(x => x.Login == dto.Login);

                if (user == null)
                {
                    throw new BadRequestException("Niepoprawny login lub hasło");
                }

                PasswordVerificationResult result = _hasher.VerifyHashedPassword(user, user.HashPassword, dto.Password);

                if (PasswordVerificationResult.Failed == result)
                {
                    throw new BadRequestException("Niepoprawny login lub hasło");
                }

                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Locality, user.Country),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

                var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                    _authenticationSettings.JwtIssuer,
                    claims,
                    expires: expires,
                    signingCredentials: cred);

                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.WriteToken(token);

            }
        }

    }