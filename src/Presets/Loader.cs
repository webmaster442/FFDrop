using FFDrop.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FFDrop.Presets;

internal class Loader
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    private PresetsRoot? _presetsRoot;

    public void LoadPresets(string presetsFile)
    {
        if (File.Exists(presetsFile))
        {
            _presetsRoot = JsonSerializer.Deserialize<PresetsRoot>(File.ReadAllText(presetsFile), _options);
        }
    }

    public Dialogdefinition[] GetDialogDefinitions()
    {
        if (_presetsRoot?.Dialogdefinitions is null)
        {
            return Array.Empty<Dialogdefinition>();
        }
        return _presetsRoot.Dialogdefinitions;
    }

    public IEnumerable<PresetViewModel> GetPresets()
    {
        var results = new ObservableCollection<PresetViewModel>();

        if (_presetsRoot?.Presets is null)
        {
            return results;
        }

        // Build hierarchy based on path segments
        var rootDict = new Dictionary<string, PresetViewModel>();

        foreach (var preset in _presetsRoot.Presets)
        {
            var paths = preset.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, PresetViewModel> currentLevel = rootDict;
            ObservableCollection<PresetViewModel>? children = null;
            PresetViewModel? parent = null;

            for (int i = 0; i < paths.Length; i++)
            {
                var segment = paths[i];
                if (!currentLevel.TryGetValue(segment, out var node))
                {
                    node = new PresetViewModel
                    {
                        Name = segment,
                        AssociatedPreset = null,
                        Children = new ObservableCollection<PresetViewModel>()
                    };
                    currentLevel[segment] = node;
                    if (parent != null)
                    {
                        parent.Children.Add(node);
                    }
                    else if (i == 0)
                    {
                        results.Add(node);
                    }
                }
                parent = node;
                children = node.Children;
                // Prepare for next level
                currentLevel = new Dictionary<string, PresetViewModel>();
                foreach (var child in node.Children)
                {
                    currentLevel[child.Name] = child;
                }
            }

            // Add the actual preset as a leaf node
            if (parent != null)
            {
                parent.Children.Add(new PresetViewModel
                {
                    Name = preset.Name,
                    AssociatedPreset = preset,
                    Children = new ObservableCollection<PresetViewModel>()
                });
            }
        }

        return results;
    }
}
