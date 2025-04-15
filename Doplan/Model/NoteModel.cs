using SQLite;

namespace Doplan.Model
{
    public class NoteModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
