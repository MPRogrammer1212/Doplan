using Doplan.Model;

namespace Doplan.Components.Pages
{
    public partial class LongTermTasks
    {

        private TermModel newTerm = new TermModel();

        private List<TaskModel> taskInputs = new();

        private List<TermModel>? Terms;

        private TaskModel newTask = new TaskModel();

        //---------------------------------------------------------
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }
        //---------------------------------------------------------


        private async Task AddTerm()
        {
            var error = taskInputs.Where(x => x.Description == null);
            if (error.Any())
            {
                await Application.Current.MainPage.DisplayAlert("description cant be null", "fill description", "ok");
                return;
            }
            newTerm.Tasks = taskInputs;
            newTerm.Term = TermEnum.LongTerm;
            await repository.AddTerm(newTerm);
            await LoadData();
            newTerm = new TermModel();
            taskInputs = new();
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

        private void AddInput()
        {
            taskInputs.Add(new TaskModel());
        }

        private void RemoveInput()
        {
            if (taskInputs.Count > 0)
            {
                taskInputs.RemoveAt(taskInputs.Count - 1);
            }
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
            Terms = await repository.GetAllLongTermsWithDetails();
        }
    }
}
