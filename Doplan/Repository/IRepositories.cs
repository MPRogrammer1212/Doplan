using Doplan.Model;

namespace Doplan.Repository;

public interface IRepositories
{
    Task<List<TermModel>> GetAllTermsWithDetails();
    Task<List<TermModel>> GetAllLongTermsWithDetails();
    Task<List<TermModel>> GetAllShortTermsWithDetails();
    Task<List<TermModel>> GetAllWeeklyTasksWithDetails();
    Task<List<TermModel>> GetAllDailyTasksWithDetails();

    Task AddTerm(TermModel term);
    Task DeleteTerm(int termId);
    Task UpdateTerm(TermModel term);

    Task AddTask(TaskModel task);
    Task UpdateTask(TaskModel task);
    Task DeleteTask(int taskId);

    Task<List<NoteModel>> GetAllNotes();
    Task AddNote(NoteModel note);
    Task DeleteNote(int noteId);
    Task UpdateNote(NoteModel note);

}
