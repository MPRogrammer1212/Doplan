using SQLite;
using SQLiteNetExtensions.Attributes;
namespace Doplan.Model;

public class TaskModel
{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsEnd { get; set; } = false;

    [ForeignKey(typeof(TermModel))]
    public int TermId { get; set; }
    [ManyToOne]
    public TermModel Term { get; set; }
}

