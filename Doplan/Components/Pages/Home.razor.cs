using Doplan.Model;

namespace Doplan.Components.Pages;

public partial class Home
{

    private bool isOn = false;

    private TaskModel newTask = new TaskModel();

    private List<TermModel>? Terms;
    private List<TermModel>? filteredTerms;

    private string completion;
    public string Completion
    {
        get { return completion; }
        set
        {
            completion = value;
            SearchProgram();
        }
    }



    private TermEnum? selectedTerm;
    public TermEnum? SelectedTerm
    {
        get { return selectedTerm; }
        set
        {
            selectedTerm = value;
            SearchProgram();
        }
    }

    private string? searchTerm;
    public string? SearchTerm
    {
        get { return searchTerm; }
        set
        {
            searchTerm = value;
            SearchProgram();
        }
    }

    private DateTime? startDateFilter;
    public DateTime? StartDateFilter
    {
        get { return startDateFilter; }
        set
        {
            startDateFilter = value;
            SearchProgram();
            StateHasChanged();
        }
    }

    private DateTime? endDateFilter;
    public DateTime? EndDateFilter
    {
        get { return endDateFilter; }
        set
        {
            endDateFilter = value;
            SearchProgram();
        }
    }

    //---------------------------------------------------------
    protected override async Task OnInitializedAsync()
    {
        await LoadData();
        completion = "all";
        isOn = false;
        await OrderByModified();
    }
    //---------------------------------------------------------

    private async Task SearchProgram()
    {
        filteredTerms = Terms.Where(t =>
        (string.IsNullOrWhiteSpace(SearchTerm) ||
        (!string.IsNullOrEmpty(t.Title) && t.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))) &&
        (!SelectedTerm.HasValue || t.Term == SelectedTerm) &&
        (!StartDateFilter.HasValue || t.StartDate.HasValue && t.StartDate.Value.Date >= StartDateFilter.Value.Date) &&
        (!EndDateFilter.HasValue || t.EndDate.HasValue && t.EndDate.Value.Date <= EndDateFilter.Value.Date) &&
        (completion == "all" || 
        (completion == "uncompleted" && t.IsEnd == false) || 
        (completion == "completed" && t.IsEnd == true))
        ).ToList();
    }

    private async Task ToggleState()
    {
        isOn = !isOn;
        await OrderByModified();
    }

    private async Task OrderByModified()
    {
        if (isOn == true)
        {
            await SearchProgram();
        }

        if (isOn == false)
        {
            filteredTerms = filteredTerms.OrderByDescending(t => t.Id).ToList();
        }
    }

    private async Task AddTask(int termId)
    {
        if (newTask.Description == null)
        {
            await Application.Current.MainPage.DisplayAlert("description cant be null", "fill description", "ok");
            return;
        }
        newTask.TermId = termId;
        await repository.AddTask(newTask);
        await LoadData();
        newTask = new TaskModel();
    }

    private async Task DeleteTerm(int id)
    {
        bool confrimation = await Application.Current.MainPage.DisplayAlert("Confrim Delete",
                "Are you sure you want to delete this program?", "yes", "no");
        if (!confrimation)
        {
            return;
        }
        await repository.DeleteTerm(id);
        await LoadData();
    }

    private async Task DeleteTask(int id)
    {
        await repository.DeleteTask(id);
        await LoadData();
    }

    private async Task ToggleTaskCompletion(TaskModel task)
    {
        try
        {
            task.IsEnd = !task.IsEnd;
            await repository.UpdateTask(task);
        }
        catch
        {
            await Application.Current.MainPage.DisplayAlert("ERROR", "Changes didnt applied", "ok");
        }
    }

    private async Task ToggleTermCompletion(TermModel term)
    {
        try
        {
            term.IsEnd = !term.IsEnd;
            await repository.UpdateTerm(term);
        }
        catch
        {
            await Application.Current.MainPage.DisplayAlert("ERROR", "Changes didnt applied", "ok");
        }
    }


    private async Task LoadData()
    {
        Terms = await repository.GetAllTermsWithDetails();
        filteredTerms = Terms;
    }
}
