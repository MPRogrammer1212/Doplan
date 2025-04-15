using Doplan.Data;
using Doplan.Model;
using Doplan.Repository;
using SQLite;
using System.Threading.Tasks;

namespace Doplan.Services;

public class Repositories : IRepositories
{
    private readonly SQLiteAsyncConnection _connection;

    public Repositories(DatabaseService database)
    {
        _connection = database.GetConnection();
    }

    public async Task<List<TermModel>> GetAllTermsWithDetails()
    {
        var terms = await _connection.Table<TermModel>().ToListAsync();

        foreach (var term in terms)
        {
            term.Tasks = await _connection.Table<TaskModel>().Where(t => t.TermId == term.Id).ToListAsync();
        }
        return terms;
    }

    public async Task<List<TermModel>> GetAllLongTermsWithDetails()
    {
        var longTerms = await _connection.Table<TermModel>().Where(x => x.Term == TermEnum.LongTerm).OrderByDescending(t => t.Id).ToListAsync();

        foreach (var term in longTerms)
        {
            term.Tasks = await _connection.Table<TaskModel>().Where(t => t.TermId == term.Id).ToListAsync();
        }
        return longTerms;
    }

    public async Task<List<TermModel>> GetAllShortTermsWithDetails()
    {
        var shortTerm = await _connection.Table<TermModel>().Where(x => x.Term == TermEnum.ShortTerm).OrderByDescending(t => t.Id).ToListAsync();

        foreach (var term in shortTerm)
        {
            term.Tasks = await _connection.Table<TaskModel>().Where(t => t.TermId == term.Id).ToListAsync();
        }
        return shortTerm;
    }

    public async Task<List<TermModel>> GetAllWeeklyTasksWithDetails()
    {
        var weeklyTasks = await _connection.Table<TermModel>().Where(x => x.Term == TermEnum.Weekly).OrderByDescending(t => t.Id).ToListAsync();

        foreach (var term in weeklyTasks)
        {
            term.Tasks = await _connection.Table<TaskModel>().Where(t => t.TermId == term.Id).ToListAsync();
        }
        return weeklyTasks;
    }

    public async Task<List<TermModel>> GetAllDailyTasksWithDetails()
    {
        var dailyTasks = await _connection.Table<TermModel>().Where(x => x.Term == TermEnum.Daily).OrderByDescending(t => t.Id).ToListAsync();

        foreach (var term in dailyTasks)
        {
            term.Tasks = await _connection.Table<TaskModel>().Where(t => t.TermId == term.Id).ToListAsync();
        }
        return dailyTasks;
    }


    public async Task AddTerm(TermModel term)
    {
        try
        {
            await _connection.InsertAsync(term);
            if (term.Tasks != null)
            {
                foreach (var task in term.Tasks)
                {
                    task.TermId = term.Id;
                    await _connection.InsertAsync(task);
                }

            }
        }
        catch (Exception ex)
        {
            // Log or handle the error
            throw new InvalidOperationException("Failed to add term", ex);
        }
    }

    public async Task DeleteTerm(int termId)
    {
        var tasks = await _connection.Table<TaskModel>().Where(t => t.TermId == termId).ToListAsync();

        foreach (var task in tasks)
        {
            await _connection.DeleteAsync(task);
        }

        var term = await _connection.FindAsync<TermModel>(termId);
        if (term != null)
        {
            await _connection.DeleteAsync(term);
        }

    }

    public async Task UpdateTerm(TermModel term)
    {
        await _connection.UpdateAsync(term);
        if (term.Tasks != null)
        {
            foreach (var task in term.Tasks)
            {
                await _connection.UpdateAsync(task);
            }
        }
    }

    public async Task UpdateTask(TaskModel task)
    {
        await _connection.UpdateAsync(task);
    }

    public async Task AddTask(TaskModel task)
    {
        await _connection.InsertAsync(task);
    }

    public async Task DeleteTask(int taskId)
    {
        var task = await _connection.FindAsync<TaskModel>(taskId);
        if (task != null)
        {
            await _connection.DeleteAsync(task);
        }
    }


    public async Task<List<NoteModel>> GetAllNotes()
    {
        return await _connection.Table<NoteModel>().OrderByDescending(t => t.Id).ToListAsync();
    }

    public async Task AddNote(NoteModel note)
    {
        await _connection.InsertAsync(note);
    }

    public async Task UpdateNote(NoteModel note)
    {
        await _connection.UpdateAsync(note);
    }

    public async Task DeleteNote(int noteId)
    {
        var note = await _connection.FindAsync<NoteModel>(noteId);
        await _connection.DeleteAsync(note);
    }

}
