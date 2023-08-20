using AutoMapper;
using ClassroomPlus.DTOs.UserDTOs;
using ClassroomPlus.Entities;
using ClassroomPlus.Exceptions;
using ClassroomPlus.Repositories.Interfaces;
using ClassroomPlus.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;

namespace ClassroomPlus.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnviroment;

    public UserService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnviroment
        )
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
        _webHostEnviroment = webHostEnviroment;
    }

    private readonly int minimumUsernameLen = 4;
    private readonly int maximumUsernameLen = 20;

    private readonly int minimumPasswordLen = 7;
    private readonly int maximumPasswordLen = 30;


    public async Task<IEnumerable<ResponseUserDTO>> getAllUsersAsync() 
    {
        var users = await _userRepository.getAllAsync();
        if (users.Count() == 0)
        {
            throw new NotFoundException("Users not found!");
        }
        return users.Select(u => _mapper.Map<ResponseUserDTO>(u));
    }

    public async Task<ResponseUserDTO> getUserByIdAsync(int id)
    {
        var user = await _userRepository.getUserByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException("User not found!");
        }
        return _mapper.Map<ResponseUserDTO>(user);
    }

    public async Task<ResponseLoginUserDTO> loginUserAsync(LoginUserDTO userDTO)
    {
        if (
            userDTO.Username == null || 
            userDTO.Password == null || 
            userDTO.Username == "" || 
            userDTO.Password == "") throw new BadRequestException("Login data can't be blank!");


        var user = await _userRepository.getUserByUsernameAsync(userDTO.Username);
        if (user == null)
            throw new NotFoundException("User not exist!");
        if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, user.HashedPassword))
        {
            throw new WrongPasswordException("Wrong password!");
        }
        string token = createToken(user);
        return new ResponseLoginUserDTO { JwtToken = token };
    }

    public async Task<ResponseUserDTO> editUserInformationAsync(EditUserDTO userDTO)
    {
        if (userDTO.Name == null || userDTO.Surname == null || userDTO.Id == null) throw new BadRequestException("Invalid edit data");
        var currentUser = await _userRepository.getUserByIdAsync(userDTO.Id);
        if (currentUser == null) 
        {
            throw new NotFoundException("User not found!");
        }
        var changedUserMappedToUserEntity = _mapper.Map<User>(userDTO);
        changedUserMappedToUserEntity.ProfileImageUrl = currentUser.ProfileImageUrl;

        var result = await _userRepository.editUserAsync(changedUserMappedToUserEntity);
        await _unitOfWork.Save();
        return _mapper.Map<ResponseUserDTO>(result);
    }

    public async Task<ResponseUserDTO> deleteUserAsync(int id)
    {
        if (id == null) throw new BadRequestException("Invalid delete request");
        var user = await _userRepository.getUserByIdAsync(id);
        if (user == null)
            throw new NotFoundException("User not found!");
        _userRepository.deleteUser(user);
        await _unitOfWork.Save();
        return _mapper.Map<ResponseUserDTO>(user);
    }

    public async Task<ResponseUserDTO> getUserByUsername(string username)
    { 
        var currentUser = await _userRepository.getUserByUsernameAsync(username);
        if (currentUser == null)
            throw new NotFoundException("Korisnik ne postoji u nasoj bazi!");
        return _mapper.Map<ResponseUserDTO>(currentUser);
    }

    public async Task<ResponseUserDTO> registerUserAsync(RegisterUserDTO userDTO)
    {
        if(userDTO.Username.Count() < minimumUsernameLen || userDTO.Username.Count() > maximumUsernameLen)
        {
            throw new InvalidDataException($"Username must have {minimumUsernameLen} - {maximumUsernameLen} characters!");
        }

        if (userDTO.Password.Count() < minimumPasswordLen || userDTO.Password.Count() > maximumPasswordLen)
        {
            throw new ShortPasswordException($"Password must have {minimumUsernameLen} - {maximumUsernameLen} characters!");
        }

        if (!IsValidEmail(userDTO.Email))
        {
            throw new EmailNotValidException("Email not valid!");
        }
        var sameUsername = await _userRepository.getUserByUsernameAsync(userDTO.Username);
        var sameEmail = await _userRepository.getUserByEmailAsync(userDTO.Email);
        if (sameUsername != null)
        {
            throw new UsernameAlreadyExistException("Username already exist :(");
        }
        
        if (sameEmail != null)
        {
            throw new EmailAlreadyExistException("Email already exist!");
        }
        string newFileName= SaveImage(userDTO.imageFile); 
        var user = _mapper.Map<User>(userDTO);
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
        user.ProfileImageUrl =  newFileName;
        User createdUser = await _userRepository.createUserAsync(user);
        await _unitOfWork.Save();
        return _mapper.Map<ResponseUserDTO>(user);
    }


    public async Task<ResponseUserDTO> editUserProfilePictureAsync(EditUserProfilePictureDTO userDTO, int currentUserId) 
    {
        string defaultProfilePicture = "default.png";

        var user = await _userRepository.getUserByIdAsync(currentUserId);
        if (user == null) 
        {
            throw new NotFoundException("User not found!");
        }

        string newImageName = user.ProfileImageUrl;
        if (user.ProfileImageUrl!=defaultProfilePicture && userDTO.ImageFile != null)
        {
            DeleteImage(user.ProfileImageUrl);
        }

        if(userDTO.ImageFile != null)
        {
            newImageName = SaveImage(userDTO.ImageFile);
        }

        if (userDTO.ImageFile == null)
        {
            user.ProfileImageUrl = defaultProfilePicture;
            newImageName = defaultProfilePicture;
        }

        user.ProfileImageUrl = newImageName;
        var editedUser = await _userRepository.editUserAsync(user);
        await _unitOfWork.Save();
        return _mapper.Map<ResponseUserDTO>(editedUser);
    }

    private void DeleteImage(string imageName)
    {
        string imagePath = Path.Combine(_webHostEnviroment.ContentRootPath, "images", imageName);
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }
    }

    public async Task<ResponseUserDTO> deleteCustomProfilePictureAsync(int currentUserId)
    {
        var user = await _userRepository.getUserByIdAsync(currentUserId);
        if (user == null) throw new NotFoundException("User does not exist!");
        if(user.ProfileImageUrl != "default.png")
        {
            DeleteImage(user.ProfileImageUrl);
        }
        user.ProfileImageUrl = "default.png";
        var editedUser =  await _userRepository.editUserAsync(user);
        await _unitOfWork.Save();
        return _mapper.Map<ResponseUserDTO>(editedUser);
    }

    public Task RefreshJwtTokenWithRefreshToken(string refreshToken)
    {
        return Task.CompletedTask;
    }  

    private string SaveImage(IFormFile imageFile)
    {
        int maxSizeInBytes = 5 * 1024 * 1024;
        string newFileName = "default.png";
        string imagePath = Path.Combine(_webHostEnviroment.ContentRootPath, "images");
        if (
        imageFile == null ||
        (imageFile.ContentType == "image/jpeg" ||
        imageFile.ContentType == "image/png") &&
        imageFile.Length <= maxSizeInBytes
        )
        {
            // Generisanje putanje do ciljanog direktorijuma i datoteke

            if(imageFile != null)
            {
                newFileName = GetUniqueFileName(imageFile.FileName);
                string filePath = Path.Combine(imagePath, newFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                return newFileName;
            }
            return newFileName;
            
        }
        else
        {
            throw new WrongImageException("Wrong image format!");
        }
    }

    private string createToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "user"),
            new Claim("Id", user.Id.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: cred,
            expires: DateTime.Now.AddYears(1),
            audience: "aud",
            issuer: "https://localhost:7296"
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }


    private string GetUniqueFileName(string fileName)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string fileExtension = Path.GetExtension(fileName);
        return $"{fileNameWithoutExtension}_{System.Guid.NewGuid()}{fileExtension}";
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

}
