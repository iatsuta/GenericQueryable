using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericQueryable.IntegrationTests.Domain;

[Table(nameof(TestObject), Schema = "app")]
public class TestObject
{
    [Key]
    public Guid Id { get; set; }

    
    public virtual FetchObject? FetchObject { get; set; }

    public virtual ICollection<DeepFetchObject> DeepFetchObjects { get; set; } = [];
}