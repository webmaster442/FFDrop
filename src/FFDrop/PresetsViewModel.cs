using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FFDrop.Model;

namespace FFDrop;

internal sealed partial class PresetsViewModel : ObservableObject
{
    internal partial class PresetCategory : ObservableObject
    {
        public string Name { get; set; }

        [ObservableProperty]
        public partial bool IsChecked { get; set; }

        public PresetCategory(string name, bool isChecked)
        {
            Name = name;
            IsChecked = isChecked;
        }
    }

    private readonly PresetViewModel[] _allpresets;

    public BindingList<PresetViewModel> Presets { get; }

    public BindingList<PresetCategory> PresetCategories { get; }

    public PresetsViewModel(IEnumerable<PresetViewModel> presets)
    {
        _allpresets = presets.ToArray();
        var categories = _allpresets
            .Select(p => new PresetCategory(p.Name, true))
            .DistinctBy(c => c.Name)
            .OrderBy(c => c.Name);

        PresetCategories = new(categories.ToArray());
        Presets = new();
        ApplyFiltering();
    }

    [RelayCommand]
    public void ApplyFiltering()
    {
        var toInclude = PresetCategories.Where(p => p.IsChecked).Select(p => p.Name);
        var filteredPresets = _allpresets.Where(p => toInclude.Contains(p.Name));
        Presets.RaiseListChangedEvents = false;
        Presets.Clear();
        foreach (var preset in filteredPresets)
        {
            Presets.Add(preset);
        }
        Presets.RaiseListChangedEvents = true;
        Presets.ResetBindings();
        ShowAllCommand.NotifyCanExecuteChanged();
    }

    public bool CanShowAll()
        => PresetCategories.Any(p => !p.IsChecked);

    [RelayCommand(CanExecute = nameof(CanShowAll))]
    public void ShowAll()
    {
        foreach (var category in PresetCategories)
        {
            category.IsChecked = true;
        }
        ApplyFiltering();
    }
}
