using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericQueryable.IntegrationTests.Domain;

[Table(nameof(DeepFetchObject), Schema = "app")]
public class DeepFetchObject
{
    [Key]
    public virtual Guid Id { get; set; }

    public virtual FetchObject? FetchObject { get; set; }
}