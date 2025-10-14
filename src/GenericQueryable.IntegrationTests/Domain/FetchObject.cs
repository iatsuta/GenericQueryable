using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericQueryable.IntegrationTests.Domain;

[Table(nameof(FetchObject), Schema = "app")]
public class FetchObject
{
    [Key]
    public Guid Id { get; set; }
}