namespace Application.Dto;

public record StudentRequestDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public List<CourseRequestDto>? Courses { get; set; }
}