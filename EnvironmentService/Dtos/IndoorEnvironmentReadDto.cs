using System.ComponentModel.DataAnnotations;

namespace EnvironmentService.Dtos;

public class IndoorEnvironmentReadDto 
{

public int Id { get; set; }
[Required] 
public string Name { get; set; }


}
