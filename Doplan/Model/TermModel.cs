using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Doplan.Model;

public class TermModel
{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsEnd { get; set; } = false;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TermEnum Term { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<TaskModel>? Tasks { get; set; }

}

