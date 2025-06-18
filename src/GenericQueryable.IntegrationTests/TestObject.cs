using System.ComponentModel.DataAnnotations.Schema;

namespace GenericQueryable.IntegrationTests;

[Table(nameof(TestObject), Schema = "app")]
public class TestObject
{
    public Guid Id { get; set; }
}