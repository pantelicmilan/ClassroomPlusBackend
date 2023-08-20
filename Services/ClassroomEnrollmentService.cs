using AutoMapper;
using ClassroomPlus.DTOs.UsersClassroomsDTOs;
using ClassroomPlus.Exceptions;
using ClassroomPlus.Repositories.Interfaces;
using ClassroomPlus.Services.Interfaces;

namespace ClassroomPlus.Services;

public class ClassroomEnrollmentService : IClassroomEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClassroomEnrollmentRepository _classroomEnrollmentRepository;
    private readonly IClassroomRepository _classroomRepository;
    private readonly IMapper _mapper;

    public ClassroomEnrollmentService(
        IUnitOfWork unitOfWork,
        IClassroomEnrollmentRepository classroomEnrollmentRepository,
        IClassroomRepository classroomRepository,
        IMapper mapper
        ) 
    { 
        _unitOfWork = unitOfWork;
        _classroomEnrollmentRepository = classroomEnrollmentRepository;
        _classroomRepository = classroomRepository;
        _mapper = mapper;
    }

    public async Task<ResponseClassroomEnrollmentDTO> createClassroomEnrollment(CreateClassroomEnrollmentDTO classroomEnrollmentDTO)
    { 
        var classroom = await _classroomRepository.getClassroomWithGuidAsync(classroomEnrollmentDTO.JoinCode);
        if (classroom == null)
            throw new NotFoundException("Invalid classroom join code!");
        if (classroom.CreatorId == classroomEnrollmentDTO.UserId)
            throw new UnauthorizedContentException("You are already owner!");
        var userWithSameCombination = await _classroomEnrollmentRepository
            .getClassroomEnrollmentWhereUserIdAndClassroomIdAsync
            (classroomEnrollmentDTO.UserId, classroom.Id);
        if (userWithSameCombination !=  null) 
        {
            throw new LimitedCountException("You are already in this classroom!");
        }
        var classroomEnrollment = await _classroomEnrollmentRepository.createClassroomEnrollmentAsync(
            new Entities.ClassroomEnrollment { ClassroomId = classroom.Id, UserId = classroomEnrollmentDTO.UserId }
            );  
        await _unitOfWork.Save();
        return _mapper.Map<ResponseClassroomEnrollmentDTO>(classroomEnrollment);
    }

    public async Task<ResponseClassroomEnrollmentDTO> deleteClassroomEnrollment(int id, int currentUserId)
    {
        var classroomEnrollment = await _classroomEnrollmentRepository.getClassroomEnrollmentByIdAsync(id);
        if (classroomEnrollment.UserId == currentUserId)
            throw new UnauthorizedContentException("This relationship does not exist!");
        if (classroomEnrollment == null)
            throw new NotFoundException("This realtionship does not exist!");
        var deletedUser = _classroomEnrollmentRepository
            .deleteClassroomEnrollment(classroomEnrollment);
        await _unitOfWork.Save();
        return _mapper.Map<ResponseClassroomEnrollmentDTO>(deletedUser);
    }

    public async Task<ResponseClassroomEnrollmentDTO> deleteClassroomEnrollmentWhereClassroomIdAndUserId(int classroomId, int currentUserId)
    {
        var classroomEnrollment = await _classroomEnrollmentRepository.getClassroomEnrollmentWhereUserIdAndClassroomIdAsync(userId: currentUserId, classroomId: classroomId);
        if(classroomEnrollment == null)
        {
            throw new NotFoundException("You are not a member of this classroom!");
        }
        var delete = _classroomEnrollmentRepository.deleteClassroomEnrollment(classroomEnrollment);
        await _unitOfWork.Save();
        return _mapper.Map<ResponseClassroomEnrollmentDTO>(delete);
    }

    public async Task<IEnumerable<ResponseClassroomEnrollmentDTO>> getAll()
    { 
        var usersClassrooms = await _classroomEnrollmentRepository.getAll();
        if (usersClassrooms.Count() == 0)
            throw new NotFoundException("Realtionship not found!");
        return usersClassrooms.Select(uc => _mapper.Map<ResponseClassroomEnrollmentDTO>(uc));
    }

    public async Task<IEnumerable<ResponseClassroomEnrollmentDTO>> getClassroomsEnrollmentsByClassroomId(int classroomId)
    {
        var classroomEnrollment = await _classroomEnrollmentRepository.getClassroomEnrollmentByClassroomIdAsync(classroomId);
        return classroomEnrollment.Select(uc => _mapper.Map<ResponseClassroomEnrollmentDTO>(uc));
    }

    public async Task<ResponseClassroomEnrollmentDTO> getClassroomEnrollmentById(int id)
    {
        var classroomEnrollment = await _classroomEnrollmentRepository.getClassroomEnrollmentByIdAsync(id);
        if (classroomEnrollment == null)
            throw new NotFoundException("Realtionship not found!");
        return _mapper.Map<ResponseClassroomEnrollmentDTO>(classroomEnrollment);
    }

    public async Task<IEnumerable<ResponseClassroomEnrollmentDTO>> getClassroomsEnrollmentsByUserId(int userId)
    {
       var classroomsEnrollments = await _classroomEnrollmentRepository.getClassroomEnrollmentByUserIdAsync(userId);
       if(classroomsEnrollments == null)
       {
        return Enumerable.Empty<ResponseClassroomEnrollmentDTO>();
       }
       return classroomsEnrollments.Select(uc => _mapper.Map<ResponseClassroomEnrollmentDTO>(uc));
    }

}
