using System.Collections.ObjectModel;

namespace FFDrop.Model;

internal sealed class PresetViewModel
{
    public required string Name { get; init; }
    public Preset? AssociatedPreset { get; init; }
    public required ObservableCollection<PresetViewModel> Children { get; init; }
}
