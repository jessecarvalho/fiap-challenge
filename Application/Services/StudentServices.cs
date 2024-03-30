using Application.Dto;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

using Infrastructure.Repositories;

public class StudentServices : IStudentServices
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentServices(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentResponseDto>>(students);
    }

    public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        return _mapper.Map<StudentResponseDto>(student);
    }

    public async Task<StudentResponseDto?> AddStudentAsync(StudentRequestDto studentRequestDto)
    {
        var student = _mapper.Map<Student>(studentRequestDto);

        var passwordHasher = new PasswordHasher<Student>();

        student.Password = passwordHasher.HashPassword(student, studentRequestDto.Password);
        
        var newStudent = await _studentRepository.AddAsync(student);
        
        return _mapper.Map<StudentResponseDto>(newStudent);
    }

    public async Task<StudentResponseDto?> UpdateStudentAsync(int id, StudentRequestDto studentRequestDto)
    {
        
        var studentExists = await _studentRepository.GetByIdAsync(id);

        if (studentExists == null)
        {
            return null;
        }

        var student = _mapper.Map<Student>(studentRequestDto);
        
        var passwordHasher = new PasswordHasher<Student>();

        student.Id = id;

        student.Password = passwordHasher.HashPassword(student, studentRequestDto.Password);
        
        var updatedStudent = await _studentRepository.UpdateAsync(student);

        return _mapper.Map<StudentResponseDto>(updatedStudent);
        
    }

    public async Task<bool?> DeleteStudentAsync(int id)
    {
        var studentToDelete = await _studentRepository.GetByIdAsync(id);

        if (studentToDelete == null)
        {
            return null;
        }

        return await _studentRepository.DeleteAsync(id);
    }
}