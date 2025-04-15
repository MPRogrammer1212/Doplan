using Doplan.Model;

namespace Doplan.Components.Pages;

public partial class Notes
{
    private NoteModel newNote = new NoteModel();

    private List<NoteModel>? notes;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }


    private async Task AddNote()
    {
        if (newNote.Title == null || newNote.Description == null)
        {
            await Application.Current.MainPage.DisplayAlert("Title and description cant be null", "Fill Fields", "OK");
            return;
        }

        await repository.AddNote(newNote);
        await LoadData();
        newNote = new NoteModel();
    }

    private async Task DeleteNote(int noteId)
    {
        await repository.DeleteNote(noteId);
        await LoadData();
    }

    private async Task LoadData()
    {
        notes = await repository.GetAllNotes();
    }
}
