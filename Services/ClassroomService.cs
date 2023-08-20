using AutoMapper;
using ClassroomPlus.DTOs.ClassroomDTOs;
using ClassroomPlus.Entities;
using ClassroomPlus.Exceptions;
using ClassroomPlus.Repositories.Interfaces;
using ClassroomPlus.Services.Interfaces;

namespace ClassroomPlus.Services;

public class ClassroomService : IClassroomService
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly IClassroomEnrollmentRepository _classroomEnrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly uint minimumClassroomNameLength = 6;
    private readonly uint maximumClassroomNameLength = 20;

    public ClassroomService(
        IClassroomRepository classroomRepository,
        IClassroomEnrollmentRepository classroomEnrollmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) 
    {
        _classroomRepository = classroomRepository;
        _classroomEnrollmentRepository = classroomEnrollmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseClassroomDTO> createClassroomAsync(CreateClassroomDTO classroomDTO)
    {
        var newClassroomNameLength = classroomDTO.Name.Count();
        if(
            newClassroomNameLength < minimumClassroomNameLength || 
            newClassroomNameLength > maximumClassroomNameLength
            ) 
        {
            throw new WrongPropertyLength($"Name {newClassroomNameLength} letters long, minimum " +
                $"{minimumClassroomNameLength}, max {maximumClassroomNameLength}");
        }
        var classroom = _mapper.Map<Classroom>(classroomDTO);
        var guid = Guid.NewGuid().ToString("N").Substring(0, 8);
        var isExistClassroomWithGeneratedGuid = await _classroomRepository.getClassroomWithGuidAsync(guid);
        while (isExistClassroomWithGeneratedGuid != null)
        {
            guid = Guid.NewGuid().ToString("N").Substring(0, 8);
            isExistClassroomWithGeneratedGuid = await _classroomRepository.getClassroomWithGuidAsync(guid);
        }
        classroom.JoinCode = guid;
        await _classroomRepository.createClassroomAsync(classroom);
        await _unitOfWork.Save();
        ResponseClassroomDTO classroomDTOResponse = _mapper.Map<ResponseClassroomDTO>(classroom);
        return classroomDTOResponse;
    }

    public async Task<ResponseClassroomDTO> deleteClassroom(int id, int currentUserId)
    {
        var classroom =  await _classroomRepository.getClassroomByIdAsync(id);
        if (classroom == null)
            throw new NotFoundException("Classroom not found!");
        if (classroom.CreatorId != currentUserId)
            throw new UnauthorizedContentException("You do not have a access for delete this classroom!");
        _classroomRepository.deleteClassroom(classroom);
        await _unitOfWork.Save();
        ResponseClassroomDTO classroomDTOResponse = _mapper.Map<ResponseClassroomDTO>(classroom);
        return classroomDTOResponse;
    }

    public async Task<ResponseClassroomDTO> editClassroomAsync(EditClassroomDTO classroomDTO)
    {
        if(
            classroomDTO.Name.Count() < minimumClassroomNameLength || 
            classroomDTO.Name.Count() > maximumClassroomNameLength || 
            classroomDTO.Name == null
            ) 
        {
            throw new LimitedCountException($"Classroom name must have min: {minimumClassroomNameLength} " +
                $", max: {maximumClassroomNameLength} characters and can't be empty:/");
        }
        var classroom = await _classroomRepository.getClassroomByIdAsync(classroomDTO.Id);
        if (classroom == null)
            throw new NotFoundException("Classroom does not exist!");
        if (classroom.CreatorId != classroomDTO.CreatorId)
            throw new UnauthorizedContentException("You are not owner of this classroom!");
        classroom.Name = classroomDTO.Name;
        await _unitOfWork.Save();
        ResponseClassroomDTO classroomDTOResponse = _mapper.Map<ResponseClassroomDTO>(classroom);
        return classroomDTOResponse;
    }

    public async Task<IEnumerable<ResponseClassroomDTO>> getAllAsync()
    {
        var classrooms = await _classroomRepository.getAllAsync();
        if (classrooms.Count() == 0)
            throw new NotFoundException("Ne postoje classroomovi u bazi!");
        IEnumerable<ResponseClassroomDTO> classroomDTOsResponse = classrooms
            .Select(classroom => _mapper.Map<ResponseClassroomDTO>(classroom));
        return classroomDTOsResponse.ToList();
    }

    //problematicna metoda
    public async Task<ResponseClassroomDTO> getClassroomByIdAsync(int id, int currentUserId, int itemsPerPage)
    {
        const int initialPostsPage = 1;
        var classroomEnrollment = await _classroomEnrollmentRepository
            .getClassroomEnrollmentWhereUserIdAndClassroomIdAsync(currentUserId, id);
        if (classroomEnrollment == null)
        {
            var classroom = await _classroomRepository.getClassroomByIdAsync(id, initialPostsPage, itemsPerPage);
            if (classroom == null) throw new NotFoundException("Classroom does not exist!");
            if (classroom.CreatorId != currentUserId) throw new NotFoundException("You are not in this classroom!");
            var classroomDTO = _mapper.Map<ResponseClassroomDTO>(classroom);
            return classroomDTO;
        }
        var finalClassroom = _mapper.Map<ResponseClassroomDTO>(await _classroomRepository.getClassroomByIdAsync(id, initialPostsPage, itemsPerPage));
        return finalClassroom;
    }

    public async Task<ResponseClassroomWithCreatorDTO> getClassroomByJoinCodeAsync(string joinCode, int currentUserId)
    {
        var classroom = await _classroomRepository.getClassroomByJoinCodeAsync(joinCode);
        if (classroom == null)
            throw new NotFoundException("Classroom with this join code not found!");
        if (classroom.CreatorId == currentUserId)
            throw new YouAlreadyOwnerException("You are already owner!");
        var classroomEnrollmentForCurrentClassroom = await _classroomEnrollmentRepository
            .getClassroomEnrollmentWhereUserIdAndClassroomIdAsync(currentUserId, classroom.Id);
        bool isUserMemberOfClassroom = classroomEnrollmentForCurrentClassroom != null;
        if (isUserMemberOfClassroom) 
        {
            throw new YouAlreadyMemberException("You are already member!");
        }
        var classroomDTOResponse = _mapper.Map<ResponseClassroomWithCreatorDTO>(classroom);
        return classroomDTOResponse;
    }

    public async Task<IEnumerable<ResponseClassroomOwnerDTO>> getClassroomsByUserIdAsync(int id)
    {
        var classrooms = await _classroomRepository.getClassroomsByUserIdAsync(id);
        if (classrooms.Count() == 0)
            return Enumerable.Empty<ResponseClassroomOwnerDTO>();
        IEnumerable<ResponseClassroomOwnerDTO> classroomDTO = classrooms.Select(c => _mapper.Map<ResponseClassroomOwnerDTO>(c) );
        return classroomDTO;
    }

}
